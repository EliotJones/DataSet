namespace EliotJones.DataTable.Tests.Unit.TestStubs
{
    using System.Collections.Generic;
    using System.Data;
    using DataTableResolver;
    using DataTypeConverter;
    using Types;

    internal class TestDataTableResolver : IDataTableResolver
    {
        public IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings)
        {
            return new List<T>();
        }
    }
}
