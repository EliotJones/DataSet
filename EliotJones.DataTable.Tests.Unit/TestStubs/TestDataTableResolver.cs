namespace EliotJones.DataTable.Tests.Unit.TestStubs
{
    using System.Collections.Generic;
    using System.Data;

    internal class TestDataTableResolver : IDataTableResolver
    {
        public IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings) where T : new()
        {
            return new List<T>();
        }
    }
}
