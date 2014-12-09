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

        public IDataTypeConverter DataTypeConverter 
        {
            get { return dataTypeConverter; }
            set { dataTypeConverter = value; }
        }

        public DataTableConverter(DataTableParserSettings dataTableParserSettings)
        {
            this.dataTableParserSettings = dataTableParserSettings;
        }

        public IEnumerable<T> ConvertToType<T>(DataTable dataTable) where T : new()
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

            List<ExtendedPropertyInfo> mappedProperties = GetPropertyMappings<T>(dataTable);

            List<T> initializedObjects = new List<T>();

            return initializedObjects;
        }

        private List<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable) where T : new()
        {
            var mappedProperties = new List<ExtendedPropertyInfo>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                string fieldName = ((ColumnMapping)property.GetCustomAttributes(typeof(ColumnMapping), false)[0]).Name;
                if (dataTable.Columns.Contains(fieldName))
                {
                    mappedProperties.Add(new ExtendedPropertyInfo(fieldName: fieldName, propertyInfo: property, columnIndex: dataTable.Columns.IndexOf(fieldName)));
                }
            }

            return new List<ExtendedPropertyInfo>();
        }
    }
}
