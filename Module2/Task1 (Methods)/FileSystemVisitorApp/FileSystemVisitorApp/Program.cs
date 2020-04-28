using System;

namespace FileSystemVisitorApp
{
    class Programm
    {
        static void Main(string[] args)
        {
            string path = @"..\..\..\TestFiles";
            
            FileSystemVisitor fsv = new FileSystemVisitor(path, (x) => x.Name.Contains("C#"));

            InitHandlers(fsv);

            Output(fsv);
        }
        static void InitHandlers(FileSystemVisitor fsv)
        {
            fsv.onStart += (obj, args) =>
            {
                Console.WriteLine("Getting files START!");
            };

            fsv.onFinish += (obj, args) =>
            {
                Console.WriteLine("Getting files FINISH!");
            };

            fsv.onFileFinded += (obj, args) =>
            {
                Console.WriteLine("Founded file: " + args.Item.Name);
                if (args.Item.Name.Contains("C#"))
                 fsv.action = Action.Stop;
            };

            fsv.onDirectoryFinded += (obj, args) =>
            {
                Console.WriteLine("Founded directory: " + args.Item.Name);
               // if (args.Item.Name.Contains("Net"))
               //     fsv.action = Action.Skip;
            };

            fsv.onFilteredFileFinded += (obj, args) =>
            {
                Console.WriteLine("Founded file after FILTER: " + args.Item.Name);
            };

            fsv.onFilteredDirectoryFind += (obj, args) =>
            {
                Console.WriteLine("Founded directory after FILTER: " + args.Item.Name);
            };
        }
        static void Output(FileSystemVisitor fsv)
        {
            foreach (var fileSysInfo in fsv.StartByPassing())
            {
                Console.WriteLine(fileSysInfo + "\n");
            }
            Console.ReadKey();
        }
    }
}



