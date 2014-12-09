namespace EliotJones.DataSet
{
    using System;

    public class DefaultDataTypeConverter : IDataTypeConverter
    {
        public object FieldToObject<T>(object field)
        {
            Type t = field.GetType();

            if (t == typeof(T))
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
