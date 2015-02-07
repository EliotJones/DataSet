namespace EliotJones.DataTable.MappingResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    internal class AttributeResolverHelper
    {
        public virtual void GenerateMappingsFromAttributes(ref List<ExtendedPropertyInfo> mappedProperties, MappingObjects mappingObjects)
        {
            bool isFirstMapper = mappedProperties.Count == 0;

            foreach (PropertyInfo property in mappingObjects.Properties)
            {
                // If we must avoid overwrites we do so here.
                if (!isFirstMapper && !mappingObjects.Settings.SubsequentMappingsShouldOverwrite)
                {
                    if (mappedProperties.Count(p => p.PropertyInfo.Name == property.Name) > 0)
                    {
                        continue;
                    }
                }

                // Use the static method in order to inspect inherited properties.
                Attribute[] attributes = Attribute.GetCustomAttributes(property, typeof(ColumnMapping), mappingObjects.Settings.InheritMappings);

                if (attributes.Length == 0)
                {
                    continue;
                }

                // Find the matching attribute if it exists, null if not.
                ColumnMapping matchedAttribute = FindMappedAttribute(attributes, mappingObjects.DataTable.Columns);

                if (matchedAttribute != null)
                {
                    mappedProperties.Add(new ExtendedPropertyInfo(
                            fieldName: matchedAttribute.Name,
                            propertyInfo: property,
                            columnIndex: mappingObjects.DataTable.Columns.IndexOf(matchedAttribute.Name)));
                }
            }
        }

        protected ColumnMapping FindMappedAttribute(Attribute[] attributes, DataColumnCollection columns)
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
    }
}
