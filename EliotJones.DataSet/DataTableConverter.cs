namespace EliotJones.DataSet
{
    using System.Collections.Generic;
    using System.Data;

    internal class DataTableConverter
    {
        private DataTableParserSettings dataTableParserSettings;

        public DataTableConverter(DataTableParserSettings dataTableParserSettings)
        {
            this.dataTableParserSettings = dataTableParserSettings;
        }

        public IEnumerable<T> ConvertToType<T>(DataTable dataTable)
        {
            return new List<T>();
        }
    }
}
