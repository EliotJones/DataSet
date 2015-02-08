namespace EliotJones.DataTable
{
    using EliotJones.DataTable.DataTypeConverter;
    using Enums;
    using MappingResolvers;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class DataTableConverter
    {
        private DataTableParserSettings dataTableParserSettings;
        private IDataTableResolver dataTableResolver;
        private MappingResolver mappingResolver;
        private IDataTypeConverter dataTypeConverter;

        public DataTableConverter(DataTableParserSettings dataTableParserSettings, MappingResolver mappingResolver, IDataTableResolver dataTableResolver, IDataTypeConverter dataTypeConverter)
        {
            this.dataTableParserSettings = dataTableParserSettings;
            this.mappingResolver = mappingResolver;
            this.dataTableResolver = dataTableResolver;
            this.dataTypeConverter = dataTypeConverter;
        }

        public virtual IEnumerable<T> ConvertToType<T>(DataTable dataTable) where T : new()
        {
            if (dataTable == null)
            {
                switch (dataTableParserSettings.NullInputHandling)
                {
                    case NullInputHandling.ReturnNull:
                        return null;
                    case NullInputHandling.Error:                    
                    default:
                        throw new ArgumentNullException();
                }
            }

            if (dataTable.Rows.Count == 0)
            {
                switch (dataTableParserSettings.EmptyInputHandling)
                {
                    case EmptyInputHandling.ReturnEmptyEnumerable:
                        return new List<T>();
                    case EmptyInputHandling.ReturnNull:
                        return null;
                    case EmptyInputHandling.Error:
                    default:
                        throw new InvalidOperationException();
                }
            }

            ICollection<ExtendedPropertyInfo> mappedProperties = mappingResolver.GetPropertyMappings<T>(dataTable, dataTableParserSettings);

            IList<T> initializedObjects = dataTableResolver.ToObjects<T>(dataTable, dataTypeConverter, mappedProperties, dataTableParserSettings);

            return initializedObjects;
        }
    }
}
