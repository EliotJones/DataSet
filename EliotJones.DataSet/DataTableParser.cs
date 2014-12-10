﻿namespace EliotJones.DataSet
{
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Class responsible for converting <see cref="DataTable"/> to list of specified type with default or custom conversion settings./>
    /// </summary>
    public class DataTableParser
    {
        private DataTableParserSettings DefaultDataTableParserSettings = new DataTableParserSettings();
        private IMappingResolver DefaultMappingResolver = new DefaultMappingResolver();

        private DataTableParserSettings dataTableParserSettings;
        private IMappingResolver mappingResolver;

        public DataTableParser()
        {
            dataTableParserSettings = new DataTableParserSettings();
        }

        /// <summary>
        /// Gets or sets the settings for passing a DataTable.
        /// </summary>
        public virtual DataTableParserSettings DataTableParserSettings
        {
            get { return dataTableParserSettings ?? DefaultDataTableParserSettings; }
            set { dataTableParserSettings = value; }
        }

        public virtual IMappingResolver MappingResolver
        {
            get { return mappingResolver ?? DefaultMappingResolver; }
            set { mappingResolver = MappingResolver; }
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
            DataTableParserSettings dataTableParserSettingsLocal = dataTableParserSettings ?? DefaultDataTableParserSettings;

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
            return new DataTableConverter(dataTableParserSettings, mappingResolver);
        }
    }
}
