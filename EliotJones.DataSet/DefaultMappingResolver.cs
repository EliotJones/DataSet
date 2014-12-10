namespace EliotJones.DataSet
{
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

            int totalProperties = typeProperties.Length;

            // which do we use first?
            GenerateMappingsFromAttributes(ref mappedProperties, typeProperties, dataTable);

            GenerateMappingsFromProperties(ref mappedProperties, typeProperties, dataTable);

            return mappedProperties;
        }

        private void GenerateMappingsFromAttributes(ref List<ExtendedPropertyInfo> mappedProperties, PropertyInfo[] properties, DataTable dataTable)
        {
        }

        private void GenerateMappingsFromProperties(ref List<ExtendedPropertyInfo> mappedProperties, PropertyInfo[] properties, DataTable dataTable)
        {
            foreach (PropertyInfo property in properties)
            {
                bool mappingFound = false;

                //TODO: Is this case insensitive?
                if (dataTable.Columns.Contains(property.Name))
                {
                    mappedProperties.Add(new ExtendedPropertyInfo(fieldName: property.Name, propertyInfo: property, columnIndex: dataTable.Columns.IndexOf(property.Name)));
                    mappingFound = true;
                }

                // Special case handling for Id columns
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
