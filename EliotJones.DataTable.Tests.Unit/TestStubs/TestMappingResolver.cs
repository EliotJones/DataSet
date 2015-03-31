namespace EliotJones.DataTable.Tests.Unit.TestStubs
{
    using System.Collections.Generic;
    using System.Data;
    using MappingResolvers;
    using Types;

    internal class TestMappingResolver : MappingResolver
    {
        private ICollection<ExtendedPropertyInfo> mappings;

        public TestMappingResolver()
        {
        }

        public TestMappingResolver(ICollection<ExtendedPropertyInfo> mappings)
        {
            this.mappings = mappings;
        }

        public override ICollection<ExtendedPropertyInfo> GetPropertyMappings<T>(DataTable dataTable, DataTableParserSettings settings)
        {
            return new ExtendedPropertyInfo[] { };
        }
    }
}
