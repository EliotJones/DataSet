namespace EliotJones.DataTable
{
    using DataTypeConverter;
    using Exceptions;
    using Factories;
    using System.Collections.Generic;
    using System.Data;

    public class DefaultDataTableResolver : IDataTableResolver
    {
        public virtual IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataTable);
            Guard.ArgumentNotNull(dataTypeConverter);
            Guard.ArgumentNotNull(mappings);
            Guard.ArgumentNotNull(settings);

            VerifyMappingIndexIntegrity<T>(dataTable.Columns, ref mappings);
            var dbNullConverter = GetDbNullConverter(settings);

            List<T> objectList = new List<T>(capacity: dataTable.Rows.Count);

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                T returnObject = ObjectInstantiator<T>.CreateNew();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(dataTable.Rows[rowIndex][mapping.ColumnIndex], mapping.PropertyInfo.PropertyType, settings, dbNullConverter);
                    mapping.PropertyInfo.SetValue(returnObject, value);
                }

                objectList.Add(returnObject);
            }

            return objectList;
        }

        protected virtual void VerifyMappingIndexIntegrity<T>(DataColumnCollection columns, ref IEnumerable<ExtendedPropertyInfo> mappings)
        {
            int columnsCount = columns.Count;

            foreach (var mapping in mappings)
            {
                if (mapping == null)
                {
                    throw new InvalidMappingException<T>("Null mapping.");
                }

                if (mapping.ColumnIndex < 0 || mapping.ColumnIndex >= columnsCount)
                {
                    if (columns.Contains(mapping.FieldName))
                    {
                        mapping.ColumnIndex = columns.IndexOf(mapping.FieldName);
                    }
                    else
                    {
                        throw new InvalidMappingException<T>("Incorrectly mapped Field: " + mapping.FieldName);
                    }
                }
            }
        }

        protected virtual DbNullConverter GetDbNullConverter(DataTableParserSettings settings)
        {
            return new DbNullConverter(settings);
        }
    }
}
