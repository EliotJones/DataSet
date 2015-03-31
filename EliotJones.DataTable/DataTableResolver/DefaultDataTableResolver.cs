namespace EliotJones.DataTable.DataTableResolver
{
    using System;
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
            ExtendedPropertyInfo[] mappings, 
            DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataTable);
            Guard.ArgumentNotNull(dataTypeConverter);
            Guard.ArgumentNotNull(mappings);
            Guard.ArgumentNotNull(settings);

            VerifyMappingIndexIntegrity<T>(dataTable.Columns, mappings);

            var dbNullConverter = GetDbNullConverter(settings);

            var objectList = new T[dataTable.Rows.Count];

            DataRow[] datarows = new DataRow[dataTable.Rows.Count];
            dataTable.Rows.CopyTo(datarows, 0);

            for (int rowIndex = 0; rowIndex < datarows.Length; rowIndex++)
            {
                var returnObject = ObjectInstantiator<T>.CreateNew();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(datarows[rowIndex][mapping.ColumnIndex], 
                        mapping.PropertyInfo.PropertyType, 
                        settings, 
                        dbNullConverter);

                    mapping.PropertyInfo.SetValue(returnObject, value);
                }

                objectList[rowIndex] = returnObject;
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
