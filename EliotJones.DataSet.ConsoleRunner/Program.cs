namespace EliotJones.DataSet.ConsoleRunner
{
    using System;
using System.Diagnostics;
    using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

    public class Program
    {
        static void Main(string[] args)
        {
            InitializeTonies it = new InitializeTonies();

            int count = 10000000;

            var sw = Stopwatch.StartNew();
            it.NormalSetter(count);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks + " Setter");

            sw.Restart();
            it.DelegateSetter(count);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks + " Delegate");

            sw.Restart();
            it.ReflectionSetter(count);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks + " Reflection");

            sw.Restart();
            it.DelegateExpressionSetter(count);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks + " Delegate with generic invoke");

            if (Debugger.IsAttached) Debugger.Break();
        }
    }

    public class Tony
    {
        public string Name { get; set; }
    }

    public class InitializeTonies
    {   
        public void NormalSetter(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();

                t.Name = "Tony" + i;
            }
        }

        public void ReflectionSetter(int n)
        {
            PropertyInfo propertyInfo = typeof(Tony).GetProperties()[0];

            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();
                propertyInfo.SetValue(t, "Tony" + i);
            }
        }

        public void DelegateSetter(int n)
        {
            MethodInfo methodInfo = typeof(Tony).GetProperties()[0].GetSetMethod();

            Action<Tony, string> setterDelegate = (Action<Tony, string>)Delegate.CreateDelegate(
                type: typeof(Action<Tony, string>),
                method: methodInfo);

            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();

                setterDelegate(t, "Tony" + i);
            }
        }

        public void DelegateExpressionSetter(int n)
        {
            MethodInfo methodInfo = typeof(Tony).GetProperties()[0].GetSetMethod();

            Action<Tony, object> setterDelegate = SetterDelegateUtil.GenerateSetterForMethod<Tony>(methodInfo);

            for (int i = 0; i < n; i++)
            {
                Tony t = new Tony();

                setterDelegate(t, "Tony" + i);
            }
        }
    }

    public class SetterDelegateUtil
    {
        public static Action<T, object> GenerateSetterForMethod<T>(MethodInfo methodInfo)
        {
            MethodInfo genericMethod = typeof(SetterDelegateUtil).GetMethod("GenerateSetterForMethodHelper");

            MethodInfo constructedHelper = genericMethod.MakeGenericMethod(
                typeof(T),
                methodInfo.GetParameters()[0].ParameterType);

            object returnAction = constructedHelper.Invoke(obj: null, parameters: new object[] {methodInfo});

            return (Action<T, object>)returnAction;
        }

        public static Action<TTarget, object> GenerateSetterForMethodHelper<TTarget, TParam>(MethodInfo methodInfo)
        {
            Action<TTarget, TParam> action = 
                (Action<TTarget, TParam>)Delegate.CreateDelegate(
                typeof(Action<TTarget, TParam>), methodInfo);

            Action<TTarget, object> returnAction = (TTarget target, object param) => action(target, (TParam)param);

            return returnAction;
        }
    }
}
