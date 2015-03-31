namespace EliotJones.DataTable
{
    using DataTypeConverter;
    using MappingResolvers;
    using System.Collections.Generic;
    using System.Data;
    using DataTableResolver;

    /// <summary>
    /// Class responsible for converting <see cref="DataTable"/> to list of specified type with default or custom conversion settings./>
    /// </summary>
    public class DataTableParser
    {
        private DataTableParserSettings dataTableParserSettings = new DataTableParserSettings();
        private MappingResolver mappingResolver = new DefaultMappingResolver();
        private IDataTableResolver dataTableResolver = new DefaultDataTableResolver();
        private IDataTypeConverter dataTypeConverter = new DefaultDataTypeConverter();

        /// <summary>
        /// Gets or sets the settings for parsing a DataTable.
        /// </summary>
        public virtual DataTableParserSettings DataTableParserSettings
        {
            get { return dataTableParserSettings; }
            set { dataTableParserSettings = value; }
        }

        public virtual IDataTypeConverter DataTypeConverter
        {
            get { return dataTypeConverter; }
            set { dataTypeConverter = value; }
        }

        /// <summary>
        /// Gets or sets the resolver used to map columns to properties.
        /// </summary>
        public virtual MappingResolver MappingResolver
        {
            get { return mappingResolver; }
            set
            {
                mappingResolver = value;
            }
        }

        public virtual IDataTableResolver DataTableResolver
        {
            get { return dataTableResolver; }
            set { dataTableResolver = value; }
        }

        public static DataTableParser Create()
        {
            return new DataTableParser();
        }

        public static DataTableParser Create(DataTableParserSettings settings)
        {
            var parser = new DataTableParser {DataTableParserSettings = settings};

            return parser;
        }

        /// <summary>
        /// Converts DataTable to object enumerable.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to convert.</param>
        /// <returns>An IEnumerable&lt;T&gt; with objects initialized.</returns>
        public virtual IEnumerable<T> ToObjects<T>(DataTable table)
        {
            return ToObjectsInternal<T>(table, dataTableParserSettings);
        }

        public virtual IEnumerable<T> ToObjects<T>(DataTable table, DataTableParserSettings dataTableParserSettings)
        {
            return ToObjectsInternal<T>(table, dataTableParserSettings);
        }

        protected virtual IEnumerable<T> ToObjectsInternal<T>(DataTable table, DataTableParserSettings dataTableParserSettings)
        {
            ConversionManager conversionManager = GetConverter(dataTableParserSettings);

            return conversionManager.ConvertToType<T>(table);
        }

        private ConversionManager GetConverter(DataTableParserSettings dataTableParserSettingsLocal)
        {
            return new ConversionManager(dataTableParserSettings, mappingResolver, dataTableResolver, dataTypeConverter);
        }
    }
}
