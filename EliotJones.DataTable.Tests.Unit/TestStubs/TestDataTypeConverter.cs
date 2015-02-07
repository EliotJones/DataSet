namespace EliotJones.DataTable.Tests.Unit.TestStubs
{
    using System;

    internal class TestConverter : IDataTypeConverter
    {
        public object FieldToObject(object field, Type type, DataTableParserSettings settings)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
