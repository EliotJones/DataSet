namespace EliotJones.DataTable.Tests.Unit.TestStubs
{
    using EliotJones.DataTable.MappingResolvers;
    using System.Collections.Generic;
    using System.Data;

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
