namespace EliotJones.DataSet
{
    using System;

    public class DefaultDataTypeConverter : IDataTypeConverter
    {
        public object FieldToObject(object field, Type type)
        {
            Type t = field.GetType();

            if (t == type)
            {
                return field;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
