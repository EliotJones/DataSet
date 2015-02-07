namespace EliotJones.DataTable
{
    using System;
    using System.Collections.Generic;

    public class DefaultDataTypeConverter : IDataTypeConverter
    {
        public object FieldToObject(object field, Type type, DataTableParserSettings settings)
        {
            if (settings.StrictTypeMappings)
            {
                return FieldToObjectStrict(field, type, settings);
            }
            else
            {
                return FieldToObjectRelaxed(field, type, settings);
            }
        }

        private object FieldToObjectStrict(object field, Type type, DataTableParserSettings settings)
        {
            Type t = field.GetType();

            if (t == type)
            {
                return field;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        private object FieldToObjectRelaxed(object field, Type type, DataTableParserSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
