namespace EliotJones.DataTable
{
    using DataTypeConverter;
    using Factories;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParallelDataTableResolver : IDataTableResolver
    {
        public IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataTable);
            Guard.ArgumentNotNull(dataTypeConverter);
            Guard.ArgumentNotNull(mappings);
            Guard.ArgumentNotNull(settings);

            ConcurrentBag<T> objectList = new ConcurrentBag<T>();
            var dbNullConverter = new DbNullConverter(settings);

            Parallel.For(0, dataTable.Rows.Count, (rowIndex) =>
            {
                T returnObject = ObjectInstantiator<T>.CreateNew();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(dataTable.Rows[rowIndex][mapping.ColumnIndex], mapping.PropertyInfo.PropertyType, settings, dbNullConverter);
                    mapping.PropertyInfo.SetValue(returnObject, value);
                }

                objectList.Add(returnObject);
            });

            return objectList.ToList();
        }
    }
}
