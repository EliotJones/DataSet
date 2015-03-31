namespace EliotJones.DataTable.MappingResolvers
{
    using Enums;
    using Exceptions;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Resolves mappings from an object's properties to DataTable columns.
    /// </summary>
    internal class DefaultMappingResolver : MappingResolver
    {
        /// <summary>
        /// Gets property mappings for a specified type from a DataTable.
        /// </summary>
        /// <typeparam name="T">The type of object to map.</typeparam>
        /// <param name="dataTable">The DataTable to map from.</param>
        /// <param name="settings">The settings to use while mapping.</param>
        /// <returns>A list of the mappings.</returns>
        public override ICollection<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable, DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataTable);
            Guard.ArgumentNotNull(settings);

            var mappedProperties = new List<ExtendedPropertyInfo>();

            var attributeResolver = new AttributeResolverHelper();
            var propertyResolver = new PropertyResolverHelper();

            var typeProperties = GetPropertiesForType<T>(settings.InheritMappings);

            var mappingObjects = new MappingObjects(typeProperties, dataTable, settings);

            // For non-overwriting mappings the execution order is important.
            switch (mappingObjects.Settings.MappingMatchOrder)
            {
                case MappingMatchOrder.PropertyNameFirst:
                    propertyResolver.GenerateMappingsFromProperties(ref mappedProperties, mappingObjects, Id);
                    attributeResolver.GenerateMappingsFromAttributes(ref mappedProperties, mappingObjects);
                    break;
                case MappingMatchOrder.AttributeValueFirst:
                    attributeResolver.GenerateMappingsFromAttributes(ref mappedProperties, mappingObjects);
                    propertyResolver.GenerateMappingsFromProperties(ref mappedProperties, mappingObjects, Id);
                    break;
                case MappingMatchOrder.IgnorePropertyNames:
                    attributeResolver.GenerateMappingsFromAttributes(ref mappedProperties, mappingObjects);
                    break;
                case MappingMatchOrder.IgnoreAttributes:
                    propertyResolver.GenerateMappingsFromProperties(ref mappedProperties, mappingObjects, Id);
                    break;
            }

            // Error in mapping handling, checks for missing mappings.
            if (mappingObjects.Settings.MissingMappingHandling == MissingMappingHandling.Error
                && mappedProperties.Count < typeProperties.Length)
            {
                throw new MissingMappingException<T>();
            }

            // Checks for duplicate mappings in the list.
            if (!mappingObjects.Settings.AllowDuplicateMappings
                && mappedProperties.Select(p => p.ColumnIndex).Distinct().Count() != mappedProperties.Count)
            {
                throw new DuplicateMappingException<T>();
            }

            return mappedProperties;
        }

        private PropertyInfo[] GetPropertiesForType<T>(bool inheritMappings)
        {
            // If we shouldn't inherit properties we need to declare the binding flags to ignore inherited properties.
            // All 3 flags are required for correct return.
            return inheritMappings ? typeof(T).GetProperties()
                : typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
