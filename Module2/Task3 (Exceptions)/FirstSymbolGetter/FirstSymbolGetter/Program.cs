using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSymbolGetter
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputString = null;
            Console.WriteLine("Input text. To stop input '0' value");
            List<char> firstSymbols = new List<char>();

            while (true)
            {
                try
                {
                    inputString = Console.ReadLine();
                    if (inputString == "0")
                        break;
                    
                    if (inputString.Equals(String.Empty))                
                          throw new NullReferenceException("The text is empty. Please input text");
                   
                    firstSymbols.Add(inputString.Remove(1, inputString.Length - 1).ToCharArray()[0]);
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
               
            }         

            Console.WriteLine("End\n");
            Output(firstSymbols);
            Console.ReadLine();

        }
        static void Output(List<char> firstSymbols)
        {
            foreach (var item in firstSymbols)
            {
                Console.WriteLine(item);
            }
        }
    }
}
