namespace EliotJones.DataSet
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IDataTableResolver
    {
        IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings) where T : new();
    }
}
