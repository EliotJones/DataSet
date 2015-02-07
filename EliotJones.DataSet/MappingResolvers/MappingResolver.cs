namespace EliotJones.DataSet.MappingResolvers
{
    using System.Collections.Generic;
    using System.Data;

    public abstract class MappingResolver
    {
        public abstract ICollection<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable, 
            DataTableParserSettings settings) where T : new();
    }
}
