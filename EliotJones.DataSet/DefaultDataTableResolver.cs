namespace EliotJones.DataSet
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class DefaultDataTableResolver : IDataTableResolver
    {
        public DefaultDataTableResolver()
        {
        }

        public IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings) where T : new()
        {
            if (dataTable == null || dataTypeConverter == null || mappings == null || settings == null) throw new NullReferenceException();

            List<T> objectList = new List<T>(capacity: dataTable.Rows.Count);

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                T returnObject = new T();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(dataTable.Rows[rowIndex][mapping.ColumnIndex], mapping.PropertyInfo.PropertyType, settings);
                    mapping.PropertyInfo.SetValue(returnObject, value);
                }

                objectList.Add(returnObject);
            }

            return objectList;
        }
    }
}
