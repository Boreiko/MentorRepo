using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Task2Test
{
    class Program
    {
        static void Main(string[] args)
        {
            jsonReader jr = new jsonReader();


            string json = @"{
            'Email': 'james@example.com',
             'Active': true,
             'CreatedDate': '2013-01-20T00:00:00Z',
            }";

            string json2 = @"{
            'Email': 'james@example.com',
             'Active': true,
             'CreatedDate': '2013-01-20T00:00:00Z',
            }";
            List<string> mylist = new List<string>(new string[] { json, json2 });


            object responce = jr.ReadJson(mylist);
            Console.WriteLine(responce);
            Console.ReadLine();
        }
    }
}
