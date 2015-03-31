namespace EliotJones.DataTable.DataTableResolver
{
    using System.Collections.Generic;
    using System.Data;
    using DataTypeConverter;
    using Exceptions;
    using Factories;
    using Types;

    internal class DefaultDataTableResolver : IDataTableResolver
    {
        public virtual IList<T> ToObjects<T>(DataTable dataTable, 
            IDataTypeConverter dataTypeConverter, 
            IList<ExtendedPropertyInfo> mappings, 
            DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataTable);
            Guard.ArgumentNotNull(dataTypeConverter);
            Guard.ArgumentNotNull(mappings);
            Guard.ArgumentNotNull(settings);

            VerifyMappingIndexIntegrity<T>(dataTable.Columns, mappings);

            var dbNullConverter = GetDbNullConverter(settings);

            var objectList = new List<T>(capacity: dataTable.Rows.Count);

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                var returnObject = ObjectInstantiator<T>.CreateNew();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(dataTable.Rows[rowIndex][mapping.ColumnIndex], 
                        mapping.PropertyInfo.PropertyType, 
                        settings, 
                        dbNullConverter);

                    mapping.PropertyInfo.SetValue(returnObject, value);
                }

                objectList.Add(returnObject);
            }

            return objectList;
        }

        protected virtual void VerifyMappingIndexIntegrity<T>(DataColumnCollection columns, 
            IList<ExtendedPropertyInfo> mappings)
        {
            int columnsCount = columns.Count;

            foreach (var mapping in mappings)
            {
                if (mapping == null)
                {
                    throw new InvalidMappingException<T>();
                }

                if (mapping.ColumnIndex < 0 || mapping.ColumnIndex >= columnsCount)
                {
                    if (columns.Contains(mapping.FieldName))
                    {
                        mapping.ColumnIndex = columns.IndexOf(mapping.FieldName);
                    }
                    else
                    {
                        throw new InvalidMappingException<T>();
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
