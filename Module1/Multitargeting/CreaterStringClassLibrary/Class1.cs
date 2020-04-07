using System;

namespace CreaterStringClassLibrary
{
    public class CreaterString
    {
        public static string CreaterHelloString(DateTime dateTime, string name) {

            return String.Format("{0} Hello, {1}", dateTime,name);
       
        }
    }
}
