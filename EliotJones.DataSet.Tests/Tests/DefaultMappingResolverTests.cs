namespace EliotJones.DataSet.Tests.Tests
{
    using EliotJones.DataSet.Tests.Factories;
    using EliotJones.DataSet.Tests.POCOs;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultMappingResolverTests
    {
        DefaultMappingResolver defaultMappingResolver = new DefaultMappingResolver();

        [Fact]
        public void GetPropertyMappings_NullDataTable_ThrowsNullReferenceException()
        {
            DataTableParserSettings dtps = new DataTableParserSettings();

            Assert.Throws(typeof(NullReferenceException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(null, dtps));
        }

        [Fact]
        public void GetPropertyMappings_NullSettings_ThrowsNullReferenceException()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns();

            Assert.Throws(typeof(NullReferenceException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, null));
        }

        [Fact]
        public void GetPropertyMappings_NullSettingsAndDataTable_ThrowsNullReferenceException()
        {
            Assert.Throws(typeof(NullReferenceException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(null, null));
        }

        [Fact]
        public void GetPropertyMappings_DataTableWithoutColumns_ReturnsEmptyCollection()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns();

            DataTableParserSettings dtps = new DataTableParserSettings();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, dtps);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_DataTableWithUnnamedColumns_ReturnsEmptyCollection()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(null);

            DataTableParserSettings dtps = new DataTableParserSettings();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoMappings>(dt, dtps);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_MappedClassHasNoProperties_ReturnsEmptyCollection()
        {
            string anyName = "random name";

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(anyName);

            DataTableParserSettings dtps = new DataTableParserSettings();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoProperties>(dt, dtps);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_OnePropertyWithMatchingColumn_ReturnsCollectionWithOneResult()
        {
            SimpleOnePropertyNoIdNoMappings poco = new SimpleOnePropertyNoIdNoMappings();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            DataTableParserSettings dtps = new DataTableParserSettings();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoMappings>(dt, dtps);

            Assert.True(results.Count == 1);
        }

        [Fact]
        public void GetPropertyMappings_OnePropertyWithMatchingColumn_ReturnsCollectionWithCorrectResult()
        {
            SimpleOnePropertyNoIdNoMappings poco = new SimpleOnePropertyNoIdNoMappings();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            DataTableParserSettings dtps = new DataTableParserSettings();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoMappings>(dt, dtps);

            ExtendedPropertyInfo propInfo = results.First();

            Assert.True(propInfo.FieldName == "PropertyOne");

            Assert.True(propInfo.ColumnIndex == 0);
        }

        [Theory, InlineData("propertyOnE"), InlineData("PROPERTYONE"), InlineData("propertyone")]
        public void GetPropertyMappings_OnePropertyWithMatchingColumnIncorrectCase_ReturnsCollectionWithOneResult(string columnName)
        {
            SimpleOnePropertyNoIdNoMappings poco = new SimpleOnePropertyNoIdNoMappings();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columnName);

            DataTableParserSettings dtps = new DataTableParserSettings();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoMappings>(dt, dtps);

            Assert.True(results.Count == 1);
        }
    }
}
