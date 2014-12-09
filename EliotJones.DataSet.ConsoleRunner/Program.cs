namespace EliotJones.DataSet.ConsoleRunner
{
    using System;
    using System.Diagnostics;
    using System.Text;

    public class Program
    {
        static void Main(string[] args)
        {
            var pi = typeof(StringBuilder).GetProperties();

            foreach (var p in pi)
            {
                Console.WriteLine(p.Name);
            }
            if (Debugger.IsAttached) Debugger.Break();
        }
    }
}
