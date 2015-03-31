namespace EliotJones.DataTable.MappingResolvers
{
    using System.Collections.Generic;
    using System.Data;
    using Types;

    public abstract class MappingResolver
    {
        protected const string Id = "id";

        public abstract ICollection<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable, 
            DataTableParserSettings settings);
    }
}
