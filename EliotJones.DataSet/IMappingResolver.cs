namespace EliotJones.DataSet
{
    using System.Collections.Generic;
    using System.Data;

    public interface IMappingResolver
    {
        ICollection<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable, DataTableParserSettings settings) where T : new();
    }
}
