﻿namespace EliotJones.DataSet
{
    using EliotJones.DataSet.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Resolves mappings from an object's properties to DataTable columns.
    /// </summary>
    public class DefaultMappingResolver : IMappingResolver
    {
        private const string id = "id";

        /// <summary>
        /// Gets property mappings for a specified type from a DataTable.
        /// </summary>
        /// <typeparam name="T">The type of object to map.</typeparam>
        /// <param name="dataTable">The DataTable to map from.</param>
        /// <param name="settings">The settings to use while mapping.</param>
        /// <returns>A list of the mappings.</returns>
        public virtual ICollection<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable, DataTableParserSettings settings) where T : new()
        {
            if (dataTable == null || settings == null) throw new NullReferenceException();

            var mappedProperties = new List<ExtendedPropertyInfo>();

            PropertyInfo[] typeProperties = typeof(T).GetProperties();
            
            // For non-overwriting mappings the execution order is important.
            // The other 2 enum values are dealt with in the private methods.
            switch (settings.MappingMatchOrder)
            {
                case MappingMatchOrder.PropertyNameFirst:
                    GenerateMappingsFromProperties(ref mappedProperties, typeProperties, dataTable, settings);
                    GenerateMappingsFromAttributes(ref mappedProperties, typeProperties, dataTable, settings);
                    break;
                case MappingMatchOrder.AttributeValueFirst:
                    GenerateMappingsFromAttributes(ref mappedProperties, typeProperties, dataTable, settings);
                    GenerateMappingsFromProperties(ref mappedProperties, typeProperties, dataTable, settings);
                    break;
                default:
                    break;
            }

            // Error in mapping handling, checks for missing mappings.
            if (settings.MissingMappingHandling == MissingMappingHandling.Error
                && mappedProperties.Count < typeProperties.Length) throw new Exception();

            // Checks for duplicate mappings in the list.
            if (!settings.AllowDuplicateMappings
                && mappedProperties.Select(p => p.ColumnIndex).Distinct().Count() != mappedProperties.Count) throw new Exception();

            return mappedProperties;
        }

        private void GenerateMappingsFromAttributes(ref List<ExtendedPropertyInfo> mappedProperties, PropertyInfo[] properties, DataTable dataTable, DataTableParserSettings settings)
        {
            if (settings.MappingMatchOrder == MappingMatchOrder.IgnoreAttributes) return;

            bool isFirstMapper = mappedProperties.Count == 0;

            foreach (PropertyInfo property in properties)
            {
                // If we must avoid overwrites we do so here.
                if (!isFirstMapper && !settings.SubsequentMappingsShouldOverwrite)
                {
                    if (mappedProperties.Count(p => p.PropertyInfo.Name == property.Name) > 0) continue;
                }

                // Use the static method in order to inspect inherited properties.
                Attribute[] attributes = Attribute.GetCustomAttributes(property, typeof(ColumnMapping), settings.InheritMappings);

                if (attributes.Length == 0) continue;

                // Find the matching attribute if it exists, null if not.
                ColumnMapping matchedAttribute = FindMappedAttribute(attributes, dataTable.Columns);

                if (matchedAttribute != null)
                {
                    mappedProperties.Add(new ExtendedPropertyInfo(fieldName: matchedAttribute.Name, 
                        propertyInfo: property, 
                        columnIndex: dataTable.Columns.IndexOf(matchedAttribute.Name)));
                }
            }
        }

        private ColumnMapping FindMappedAttribute(Attribute[] attributes, DataColumnCollection columns)
        {
            ColumnMapping returnColumnMapping = null;

            foreach (var attribute in attributes)
            {
                ColumnMapping columnMapping = attribute as ColumnMapping;

                if (columnMapping != null && columns.Contains(columnMapping.Name))
                {
                    returnColumnMapping = columnMapping;
                    break;
                }
            }

            return returnColumnMapping;
        }

        private void GenerateMappingsFromProperties(ref List<ExtendedPropertyInfo> mappedProperties, PropertyInfo[] properties, DataTable dataTable, DataTableParserSettings settings)
        {
            if (settings.MappingMatchOrder == MappingMatchOrder.IgnorePropertyNames) return;

            bool isFirstMapper = mappedProperties.Count == 0;

            foreach (PropertyInfo property in properties)
            {
                // If we must avoid overwrites we do so here.
                if (!isFirstMapper && !settings.SubsequentMappingsShouldOverwrite)
                {
                    if (mappedProperties.Count(p => p.PropertyInfo.Name == property.Name) > 0) continue;
                }

                bool mappingFound = false;

                if (dataTable.Columns.Contains(property.Name))
                {
                    mappedProperties.Add(new ExtendedPropertyInfo(fieldName: property.Name, propertyInfo: property, columnIndex: dataTable.Columns.IndexOf(property.Name)));
                    mappingFound = true;
                }

                // Special case handling for Id columns/properties.
                if (!mappingFound && property.Name.ToLowerInvariant().Contains(id))
                {
                    string searchTerm = null;

                    if (property.Name.ToLowerInvariant() == id)
                    {
                        searchTerm = property.DeclaringType.Name + id;
                    }
                    else
                    {
                        searchTerm = id;
                    }

                    if (dataTable.Columns.Contains(searchTerm))
                    {
                        mappedProperties.Add(new ExtendedPropertyInfo(fieldName: searchTerm, propertyInfo: property, columnIndex: dataTable.Columns.IndexOf(searchTerm)));
                    }
                }
            }
        }
    }
}