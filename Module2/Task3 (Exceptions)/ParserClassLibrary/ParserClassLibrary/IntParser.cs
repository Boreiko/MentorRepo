using System;

namespace ParserClassLibrary
{
    public class IntParser
    {         
         public IntParser() { }
         public int Parse(string InputString)
        {
            try
            {
                if (InputString == null || InputString == "")
                {
                    throw new ArgumentNullException();
                }

                double result = 0;
                char[] arrayChar = InputString.ToCharArray(0, InputString.Length);

                for (int i = 0; i < arrayChar.Length; i++)
                {
                    if (arrayChar[i] < 48 || arrayChar[i] > 57)
                    {
                        throw new Exception("Incorrect number");
                    }
                }

                for (int i = 0; i < InputString.Length; i++)
                {
                    result += (arrayChar[i] - 48) * Math.Pow(10, InputString.Length - 1 - i);
                }

                if (result < int.MaxValue && result > int.MinValue)
                {
                    return (int)result;
                }
                throw new ArgumentOutOfRangeException();

                
            }

            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Range is not available for integer");
                return 0;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Argument can't be empty");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

    }
}
