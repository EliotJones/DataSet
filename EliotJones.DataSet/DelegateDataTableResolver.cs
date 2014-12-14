namespace EliotJones.DataSet
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public class DelegateDataTableResolver : IDataTableResolver
    {
        private IEnumerable<DelegateColumnMapping<T>> GetDelegatesForType<T>(IEnumerable<ExtendedPropertyInfo> mappings)
        {
            List<DelegateColumnMapping<T>> delegates = new List<DelegateColumnMapping<T>>();

            // Get the corresponding setter delegate for each mapping.
            foreach (var mapping in mappings)
            {
                var action = SetterActionUtil.GetGenericPropertySetter<T>(mapping.PropertyInfo);

                delegates.Add(new DelegateColumnMapping<T>
                    {
                        SetterDelegate = action,
                        ExtendedPropertyInfo = mapping
                    });
            }

            return delegates;
        }

        public IList<T> ToObjects<T>(DataTable dataTable, IDataTypeConverter dataTypeConverter, IEnumerable<ExtendedPropertyInfo> mappings, DataTableParserSettings settings) where T : new()
        {
            if (dataTable == null || dataTypeConverter == null || mappings == null) throw new NullReferenceException();

            List<T> objectList = new List<T>(capacity: dataTable.Rows.Count);

            IEnumerable<DelegateColumnMapping<T>> delegates = GetDelegatesForType<T>(mappings);

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                T returnObject = new T();

                foreach (var setterDelegate in delegates)
                {
                    object value = dataTypeConverter.FieldToObject(dataTable.Rows[rowIndex][setterDelegate.ExtendedPropertyInfo.ColumnIndex], setterDelegate.ExtendedPropertyInfo.PropertyInfo.PropertyType, settings);

                    setterDelegate.SetterDelegate(returnObject, value);
                }

                objectList.Add(returnObject);
            }

            return objectList;
        }

        private class DelegateColumnMapping<T>
        {
            public Action<T, object> SetterDelegate { get; set; }

            public ExtendedPropertyInfo ExtendedPropertyInfo { get; set; }
        }
    }

    internal static class SetterActionUtil
    {
        /// <summary>
        /// Generates a generic delegate for setting a property from a PropertyInfo.
        /// </summary>
        /// <typeparam name="T">The type on which to set the property.</typeparam>
        /// <param name="propertyInfo">The property to set.</param>
        /// <returns>A delegate which can be invoked for object to set a property.</returns>
        public static Action<T, object> GetGenericPropertySetter<T>(PropertyInfo propertyInfo)
        {
            // Get the method info for the property setter.
            MethodInfo methodInfo = propertyInfo.GetSetMethod();

            // Get a generic version of the helper method.
            MethodInfo genericPropertySetterHelper =
                typeof(SetterActionUtil).GetMethod(name: "GenericPropertySetterHelper",
                bindingAttr: BindingFlags.Static | BindingFlags.NonPublic);

            // First type argument is the target object type, the second is the target property type.
            MethodInfo constructedHelper = genericPropertySetterHelper.MakeGenericMethod(typeof(T), methodInfo.GetParameters()[0].ParameterType);

            // Makes a call to the helper method. obj is null because it's a static method.
            object returnObject = constructedHelper.Invoke(obj: null, parameters: new object[] { methodInfo });

            // Casts the generic object to the required delegate type.
            return (Action<T, object>)returnObject;
        }

        private static Action<TTarget, object> GenericPropertySetterHelper<TTarget, TParam>(MethodInfo methodInfo)
        {
            // Creates an open delegate which is strongly typed.
            Action<TTarget, TParam> action =
                (Action<TTarget, TParam>)Delegate.CreateDelegate(type: typeof(Action<TTarget, TParam>), method: methodInfo);

            // Convert the typed delegate to a delegate accepting an object using lambda expression.
            Action<TTarget, object> returnAction = (TTarget target, object param) => action(target, (TParam)param);

            return returnAction;
        }
    }
}
