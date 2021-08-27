using System;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var s = "+0";
            var s2 = "-0";

            Console.WriteLine(s.ToDouble());
            Console.WriteLine(s2.ToDouble());

            Console.ReadKey();
        }
    }
}