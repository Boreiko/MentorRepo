using System;
using CreaterStringClassLibrary;

namespace CoreOutputApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please. Enter your name!");
            string name = Console.ReadLine();
            Console.WriteLine(CreaterString.CreaterHelloString(DateTime.Now, name));
        }
    }
}
