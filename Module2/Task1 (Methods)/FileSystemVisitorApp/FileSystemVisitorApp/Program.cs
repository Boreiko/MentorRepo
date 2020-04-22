using System;
using System.Collections.Generic;
using System.IO;


namespace FileSystemVisitorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string path = @"..\..\..\TestFiles";
            #region WithoutFilter
            FileSystemVisitor fsv = new FileSystemVisitor(path);   // without filter

            fsv.onStart += StartNotify;
            fsv.onFinish += FinishNotify;
            fsv.onFileFind += FileFindedNotify;
            fsv.onDirectoryFind += DirectoryFindedNotify;
            fsv.Start();
            OutputFiles(fsv);
            #endregion

            #region WithFilter
            //FileSystemVisitor fsv = new FileSystemVisitor(path, x => x.Name.Contains("C#"), false, false);     // using filter 
            //fsv.onStart += StartNotify;
            //fsv.onFinish += FinishNotify;
            //fsv.onFilteredFileFind += FilteredFileFindedNotify;
            //fsv.onFilteredDirectoryFind += FilteredDirectoryFindedNotify;
            //fsv.Start();
            //OutputFiles(fsv);
            #endregion

            Console.ReadLine();
        }
        static void OutputFiles(FileSystemVisitor fsv)
        {
            Console.WriteLine("\nFiles:");
            foreach (var item in fsv)
            {
                Console.WriteLine(item);
            }
        }

        static public void StartNotify() {
            Console.WriteLine("Getting files START!");
        }

        static public void FinishNotify() {
            Console.WriteLine("Getting files FINISH!");
        }
        
        static public void FileFindedNotify()
        {
            Console.WriteLine("File finded");
        }
        static public void DirectoryFindedNotify()
        {
            Console.WriteLine("Directory finded");
        }
        static public void FilteredFileFindedNotify()
        {
            Console.WriteLine("Filtered file finded");
        }

        static public void FilteredDirectoryFindedNotify()
        {
            Console.WriteLine("Filtered directory finded");
        }
    }
}