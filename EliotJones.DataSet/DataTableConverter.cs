namespace EliotJones.DataSet
{
    using EliotJones.DataSet.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public class DataTableConverter
    {
        private DataTableParserSettings dataTableParserSettings;
        private IDataTypeConverter dataTypeConverter = new DefaultDataTypeConverter();
        private IMappingResolver mappingResolver;

        public IDataTypeConverter DataTypeConverter 
        {
            get { return dataTypeConverter; }
            set { dataTypeConverter = value; }
        }

        public DataTableConverter(DataTableParserSettings dataTableParserSettings, IMappingResolver mappingResolver)
        {
            this.dataTableParserSettings = dataTableParserSettings;
            this.mappingResolver = mappingResolver;
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

            List<T> initializedObjects = new List<T>();

            return initializedObjects;
        }
    }
}
