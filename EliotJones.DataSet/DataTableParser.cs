namespace EliotJones.DataSet
{
    using MappingResolvers;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Class responsible for converting <see cref="DataTable"/> to list of specified type with default or custom conversion settings./>
    /// </summary>
    public class DataTableParser
    {
        private DataTableParserSettings dataTableParserSettings = new DataTableParserSettings();
        private MappingResolver mappingResolver = new DefaultMappingResolver();
        private IDataTableResolver dataTableResolver = new DefaultDataTableResolver();
        private IDataTypeConverter dataTypeConverter = new DefaultDataTypeConverter();

        public DataTableParser()
        {
        }

        /// <summary>
        /// Gets or sets the settings for passing a DataTable.
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
            set { mappingResolver = MappingResolver; }
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
            var parser = new DataTableParser();

            parser.DataTableParserSettings = settings;

            return parser;
        }

        public virtual IEnumerable<T> AddToExistingCollection<T>(DataTable table, IEnumerable<T> enumerable) where T : new()
        {
            return this.ToObjects<T>(table);
        }

        /// <summary>
        /// Converts DataTable to object enumerable.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to convert.</param>
        /// <returns>An IEnumerable&lt;T&gt; with objects initialized.</returns>
        public virtual IEnumerable<T> ToObjects<T>(DataTable table) where T : new()
        {
            DataTableParserSettings dataTableParserSettingsLocal = dataTableParserSettings;

            return ToObjectsInternal<T>(table, dataTableParserSettingsLocal);
        }

        public virtual IEnumerable<T> ToObjects<T>(DataTable table, DataTableParserSettings dataTableParserSettingsLocal) where T : new()
        {
            return ToObjectsInternal<T>(table, dataTableParserSettingsLocal);
        }

        protected virtual IEnumerable<T> ToObjectsInternal<T>(DataTable table, DataTableParserSettings dataTableParserSettingsLocal) where T : new()
        {
            DataTableConverter dataTableConverter = GetConverter(dataTableParserSettingsLocal);

            return dataTableConverter.ConvertToType<T>(table);
        }

        protected virtual DataTableConverter GetConverter(DataTableParserSettings dataTableParserSettingsLocal)
        {
            return new DataTableConverter(dataTableParserSettings, mappingResolver, dataTableResolver, dataTypeConverter);
        }
    }
}
