namespace EliotJones.DataTable.Tests.Unit.Tests
{
    using EliotJones.DataTable.DataTypeConverter;
    using EliotJones.DataTable.Exceptions;
    using Factories;
    using Helpers;
    using POCOs;
    using TestStubs;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DataTableResolver;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultDataTableResolverTests
    {
        private IDataTypeConverter dataTypeConverter = new TestConverter();
        private DefaultDataTableResolver dataTableResolver = new DefaultDataTableResolver();
        private DataTableParserSettings dataTableParserSettings = new DataTableParserSettings();

        [Fact]
        public void ToObjects_NullDataTable_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(null, dataTypeConverter, CreateEmptyPropertyMappings(), dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullDataTypeConverter_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(new DataTable(), null, CreateEmptyPropertyMappings(), dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullMappings_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(new DataTable(), dataTypeConverter, null, dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullArguments_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(null, null, null, null));
        }

        [Fact]
        public void ToObjects_EmptyDataTable_ReturnsEmptyEnumerableOfCorrectType()
        {
            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(new DataTable(), dataTypeConverter, CreateEmptyPropertyMappings(), dataTableParserSettings);

            Assert.Equal(0, results.Count());
        }

        [Fact]
        public void ToObjects_DataTableWithOneRowCorrectTypes_ReturnsEnumerableWithCorrectResult()
        {
            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<SimpleNoIdNoAttributes>();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<SimpleNoIdNoAttributes>();

            foreach (var mapping in mappings)
            {
                mapping.ColumnIndex = dt.Columns.IndexOf(mapping.FieldName);
            }

            dt.Rows.Add(1, "string");

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(dt, dataTypeConverter, mappings, dataTableParserSettings);

            Assert.True(results.Count == 1);
        }

        [Theory]
        [InlineData(1, "string")]
        [InlineData(-3, "")]
        [InlineData(0, "\r\n\0")]
        [InlineData(int.MinValue, "string")]
        [InlineData(int.MaxValue, "string")]
        public void ToObjects_DataTableWithIncorrectColumnIndexButCorrectColumn_ReturnsCorrectResult(int propertyOne, string propertyTwo)
        {
            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<SimpleNoIdNoAttributes>();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<SimpleNoIdNoAttributes>();

            dt.Rows.Add(propertyOne, propertyTwo);

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(dt, dataTypeConverter, mappings, dataTableParserSettings);

            Assert.Equal(GetAssertObject<int>(propertyOne), results.First().PropertyOne);
            Assert.Equal(GetAssertObject<string>(propertyTwo), results.First().PropertyTwo);
        }

        [Fact]
        public void ToObjects_IncorrectMapping_ThrowsInvalidMappingException()
        {
            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<SimpleNoIdNoAttributes>();

            mappings.First().FieldName = string.Empty;

            DataTable dt = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<SimpleNoIdNoAttributes>();

            Assert.Throws<InvalidMappingException<SimpleNoIdNoAttributes>>(() => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(dt, dataTypeConverter, mappings, dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullMapping_ThrowsInvalidMappingException()
        {
            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<SimpleNoIdNoAttributes>();

            var mappingsList = mappings.ToList();

            mappingsList[0] = null;

            DataTable dt = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<SimpleNoIdNoAttributes>();

            Assert.Throws<InvalidMappingException<SimpleNoIdNoAttributes>>(() => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(dt, dataTypeConverter, mappingsList, dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_DataTableWithManyRowsMatchingColumns_ReturnsEnumerableWithCorrectResult()
        {
            int rows = 1000;

            List<SimpleNoIdNoAttributes> objects = new List<SimpleNoIdNoAttributes>(capacity: rows);

            for (int i = 0; i < rows; i++)
            {
                objects.Add(new SimpleNoIdNoAttributes
                    {
                        PropertyOne = i + 1,
                        PropertyTwo = "Property" + i
                    });
            }

            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<SimpleNoIdNoAttributes>();

            DataTable dt = DataTableFactory.GenerateDataTableFilledWithObjects<SimpleNoIdNoAttributes>(objects);

            foreach (var mapping in mappings)
            {
                mapping.ColumnIndex = dt.Columns.IndexOf(mapping.FieldName);
            }

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(dt, dataTypeConverter, mappings, dataTableParserSettings);

            Assert.True(results.Count == rows);
        }

        private IEnumerable<ExtendedPropertyInfo> CreateEmptyPropertyMappings()
        {
            return new List<ExtendedPropertyInfo>();
        }

        private object GetAssertObject<T>(object field)
        {
            return dataTypeConverter.FieldToObject(field, typeof(T), dataTableParserSettings, new DbNullConverter(dataTableParserSettings));
        }
    }
}
