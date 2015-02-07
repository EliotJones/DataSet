namespace EliotJones.DataTable
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParallelDataTableResolver : IDataTableResolver
    {
        public IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings) where T : new()
        {
            if (dataTable == null || dataTypeConverter == null || mappings == null) throw new NullReferenceException();

            ConcurrentBag<T> objectList = new ConcurrentBag<T>();

            Parallel.For(0, dataTable.Rows.Count, (rowIndex) =>
            {
                T returnObject = new T();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(dataTable.Rows[rowIndex][mapping.ColumnIndex], mapping.PropertyInfo.PropertyType, settings);
                    mapping.PropertyInfo.SetValue(returnObject, value);
                }

                objectList.Add(returnObject);
            });

            return objectList.ToList();
        }
    }
}
