namespace EliotJones.DataTable.MappingResolvers
{
    using System.Collections.Generic;
    using System.Data;
    using Types;

    public interface IMappingResolver
    {
        ICollection<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable, 
            DataTableParserSettings settings);
    }
}
