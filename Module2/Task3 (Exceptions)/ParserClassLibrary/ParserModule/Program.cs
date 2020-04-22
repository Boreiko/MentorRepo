using System;
using System.Collections.Generic;
using ParserClassLibrary;

namespace ParserModule
{
    class Program
    {
        static void Main(string[] args)
        {
            IntParser parser = new IntParser();

            string s = "104";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            s = "1";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            s = "0";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            s = "14";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            s = "1MSD0";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            s = "154645";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            s = "1546451127168460";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            s = "";
            Console.WriteLine(s + " - " + parser.Parse(s) + "\n");

            Console.ReadLine();
           
        }
    }
}
