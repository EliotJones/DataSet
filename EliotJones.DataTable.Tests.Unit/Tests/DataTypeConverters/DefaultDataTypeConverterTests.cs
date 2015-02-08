namespace EliotJones.DataTable.Tests.Unit.Tests.DataTypeConverters
{
    using EliotJones.DataTable.DataTypeConverter;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultDataTypeConverterTests
    {
        private DataTableParserSettings defaultSettings = new DataTableParserSettings();

        public static IEnumerable<object[]> StringTestData
        {
            get
            {
                return new[]
            {
                new object[] { string.Empty },
                new object[] { null },
                new object[] { 'x' },
                new object[] { '\r' },
                new object[] { "string" },
                new object[] { new DateTime(2001, 1, 1) },
                new object[] { int.MinValue },
                new object[] { int.MaxValue },
                new object[] { 0.5454f },
                new object[] { DBNull.Value },
                new object[] { Guid.Empty }
            };
            }
        }

        public static IEnumerable<object[]> UnsupportedClassTestData
        {
            get
            {
                return new[]
            {
                new object[] { string.Empty },
                new object[] { 'x' },
                new object[] { '\r' },
                new object[] { "string" },
                new object[] { new DateTime(2001, 1, 1) },
                new object[] { int.MinValue },
                new object[] { int.MaxValue },
                new object[] { 0.5454f },
                new object[] { Guid.Empty }
            };
            }
        }

        [Theory]
        [PropertyData("StringTestData")]
        public void FieldToObject_TypeToString_ReturnsString(object input)
        {
            DefaultDataTypeConverter converter = new DefaultDataTypeConverter();

            object toConvert = input;

            object result = converter.FieldToObject(toConvert, typeof(string), defaultSettings, new DbNullConverter(defaultSettings));

            string expectedResult = (input == null || input == DBNull.Value) ? null : input.ToString();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [PropertyData("UnsupportedClassTestData")]
        public void FieldToObject_TypeToUnsupportedClass_ThrowsNotImplementedException(object input)
        {
            var converter = new DefaultDataTypeConverter();

            Assert.Throws<NotImplementedException>(() => converter.FieldToObject(input, typeof(StringBuilder), defaultSettings, new DbNullConverter(defaultSettings)));
        }
    }
}
