namespace EliotJones.DataTable
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DataTableResolver;
    using DataTypeConverter;
    using Enums;
    using MappingResolvers;
    using Types;

    internal class ConversionManager
    {
        private readonly DataTableParserSettings dataTableParserSettings;
        private readonly IDataTableResolver dataTableResolver;
        private readonly IMappingResolver mappingResolver;
        private readonly IDataTypeConverter dataTypeConverter;

        public ConversionManager(DataTableParserSettings dataTableParserSettings,
            IMappingResolver mappingResolver,
            IDataTableResolver dataTableResolver,
            IDataTypeConverter dataTypeConverter)
        {
            this.dataTableParserSettings = dataTableParserSettings;
            this.mappingResolver = mappingResolver;
            this.dataTableResolver = dataTableResolver;
            this.dataTypeConverter = dataTypeConverter;
        }

        public virtual IEnumerable<T> ConvertToType<T>(DataTable dataTable)
        {
            if (ReturnNullFromInput(dataTable))
            {
                return null;
            }

            if (InputIsEmpty(dataTable))
            {
                return ReturnFromEmptyInput<T>(dataTable);
            }

            ICollection<ExtendedPropertyInfo> mappedProperties = mappingResolver.GetPropertyMappings<T>(dataTable, 
                dataTableParserSettings);

            return dataTableResolver.ToObjects<T>(dataTable, 
                dataTypeConverter, 
                mappedProperties, 
                dataTableParserSettings);
            }

        private bool InputIsEmpty(DataTable dataTable)
        {
            return dataTable.Rows.Count == 0;
        }

        protected virtual bool ReturnNullFromInput(DataTable dataTable)
        {
            if (dataTable == null)
            {
                switch (dataTableParserSettings.NullInputHandling)
                {
                    case NullInputHandling.Error:
                        throw new ArgumentNullException();
                    default:
                        return true;
                }
            }
            return false;
        }

        protected virtual IEnumerable<T> ReturnFromEmptyInput<T>(DataTable dataTable)
        {
            switch (dataTableParserSettings.EmptyInputHandling)
            {
                case EmptyInputHandling.ReturnNull:
                    return null;
                case EmptyInputHandling.Error:
                    throw new InvalidOperationException();
                default:
                    return new T[0];
            }
        }
    }
}
