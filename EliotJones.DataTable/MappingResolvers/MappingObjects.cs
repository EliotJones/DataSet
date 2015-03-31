namespace EliotJones.DataTable.MappingResolvers
{
    using System.Data;
    using System.Reflection;

    internal class MappingObjects
    {
        private readonly PropertyInfo[] properties;
        public PropertyInfo[] Properties
        {
            get { return properties; }
        }

        private readonly DataTable dataTable;
        public DataTable DataTable
        {
            get { return dataTable; }
        }

        private readonly DataTableParserSettings settings;
        public DataTableParserSettings Settings
        {
            get { return settings; }
        }

        public MappingObjects(PropertyInfo[] properties,
            DataTable dataTable,
            DataTableParserSettings settings)
        {
            this.properties = properties;
            this.dataTable = dataTable;
            this.settings = settings;
        }
    }
}
