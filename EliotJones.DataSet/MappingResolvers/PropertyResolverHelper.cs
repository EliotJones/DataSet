namespace EliotJones.DataSet.MappingResolvers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    internal class PropertyResolverHelper
    {
        public virtual void GenerateMappingsFromProperties(ref List<ExtendedPropertyInfo> mappedProperties, MappingObjects mappingObjects, string id)
        {
            bool isFirstMapper = mappedProperties.Count == 0;

            foreach (PropertyInfo property in mappingObjects.Properties)
            {
                // If we must avoid overwrites we do so here.
                if (!isFirstMapper && !mappingObjects.Settings.SubsequentMappingsShouldOverwrite)
                {
                    if (mappedProperties.Count(p => p.PropertyInfo.Name == property.Name) > 0) continue;
                }

                bool mappingFound = false;

                if (mappingObjects.DataTable.Columns.Contains(property.Name))
                {
                    mappedProperties.Add(new ExtendedPropertyInfo(fieldName: property.Name, propertyInfo: property, columnIndex: mappingObjects.DataTable.Columns.IndexOf(property.Name)));
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

                    if (mappingObjects.DataTable.Columns.Contains(searchTerm))
                    {
                        mappedProperties.Add(new ExtendedPropertyInfo(fieldName: searchTerm, propertyInfo: property, columnIndex: mappingObjects.DataTable.Columns.IndexOf(searchTerm)));
                    }
                }
            }
        }
    }
}
