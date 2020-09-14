using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module_8
{
    class Program
    {
        static string nameDB = "Books";
        static string nameTable = "BooksTable";
        static void Main(string[] args)
        {
            var mongoClient = new MongoClient();
            var db = mongoClient.GetDatabase(nameDB);
            DB_Accessor dB_Accessor = new DB_Accessor(nameDB);
            dB_Accessor.setTableName(nameTable);

            var books = GetTestBooks();
            dB_Accessor.AddRecords(books);
            
            showEnumerator(dB_Accessor.GetNameBook(3));           
            Console.WriteLine();

            Console.WriteLine("Min count - " + dB_Accessor.GetBookMinCount());
            Console.WriteLine("Max count - " + dB_Accessor.GetBookMaxCount());
            Console.WriteLine();
           
            Console.WriteLine("Books without author:");
            showEnumerator(dB_Accessor.GetBookWithoutAuthor());       
            Console.WriteLine();
           
            Console.WriteLine("Authors:");
            showEnumerator(dB_Accessor.GetAllAuthor());          
            Console.WriteLine();

            dB_Accessor.AddOneCountEachBook();                        
            Console.WriteLine("Books after count increasing");
            showEnumerator(dB_Accessor.GetBooks());

            Console.WriteLine();

            dB_Accessor.AddAdditionalGenge("fantasy", "favority");           
            Console.WriteLine("Books after adding jenre 'favority'");
            showEnumerator(dB_Accessor.GetBooks());
            Console.WriteLine();

            dB_Accessor.DeleteBookWhereCountLess(3);
            dB_Accessor.DeleteAll();
            Console.WriteLine("Count books after deleting");
            Console.WriteLine(dB_Accessor.GetBooks().Count());
        
            Console.ReadLine();
           
        }

        static void showEnumerator<T>(IEnumerable<T> enumer)
        {
            foreach (var item in enumer)
            {
                Console.WriteLine(item.ToString());
            }
        }
         static IEnumerable<Book> GetTestBooks()
        {
            return new List<Book>()
            {
               new Book()
               {
                    Author = "Tolkien",
                    Name = "Hobbit",
                    Count = 5,
                    Genre = new string[] { "fantasy" },
                    Year = 2014
               },
               new Book()
               {
                    Author = "Tolkien",
                    Name = "Lord of the rings",
                    Count = 3,
                    Genre = new string[] { "fantasy" },
                    Year = 2015
               },
               new Book()
               {
                    Name = "Kolobok",
                    Count = 10,
                    Genre = new string[] { "kids" },
                    Year = 2000
               },
               new Book()
               {
                    Name = "Repka",
                    Count = 11,
                    Genre = new string[] { "kids" },
                    Year = 2000
               },
               new Book()
               {
                    Author = "Mihalkov",
                    Name = "Dyadya Stiopa",
                    Count = 1,
                    Genre = new string[] { "fantasy" },
                    Year = 2001
               },
            };
        }
    }
}
