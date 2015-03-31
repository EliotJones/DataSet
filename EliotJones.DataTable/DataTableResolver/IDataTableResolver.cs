namespace EliotJones.DataTable.DataTableResolver
{
    using System.Collections.Generic;
    using System.Data;
    using DataTypeConverter;

    public interface IDataTableResolver
    {
        IList<T> ToObjects<T>(DataTable dataTable, 
            IDataTypeConverter dataTypeConverter, 
            IEnumerable<ExtendedPropertyInfo> mappings, 
            DataTableParserSettings settings);
    }
}
