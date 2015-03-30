namespace EliotJones.DataTable.Tests.Unit.Tests
{
    using EliotJones.DataTable.DataTypeConverter;
    using EliotJones.DataTable.MappingResolvers;
    using Factories;
    using Helpers;
    using POCOs;
    using System;
    using System.Linq;
    using TestStubs;
    using Xunit;

    public class ProtectedClassesTests
    {
        private DataTableParserSettings defaultSettings = new DataTableParserSettings();
        private MappingResolver defaultMappingResolver = new TestMappingResolver();
        private IDataTableResolver defaultDataTableResolver = new TestDataTableResolver();
        private IDataTypeConverter defaultDataTypeConverter = new TestConverter();

        private IDataTableResolver dataTableResolver = new DefaultDataTableResolver();

        [Fact]
        public void ToObjects_WithPrivateConstructor_CanMapObjects()
        {
            Guid guid = new Guid("07494404-072A-4BE3-962E-AA3E839AD330");

            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<PrivateConstructorPublicProperty>();

            var dataTable = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<PrivateConstructorPublicProperty>();

            var dataRow = dataTable.NewRow();
            dataRow["Id"] = guid;
            dataTable.Rows.Add(dataRow);

            var results = dataTableResolver.ToObjects<PrivateConstructorPublicProperty>(dataTable, new DefaultDataTypeConverter(), mappings, defaultSettings);

            Assert.Equal(1, results.Count);
            Assert.Equal(guid, results.Single().Id);
        }
    }
}
