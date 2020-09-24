using System;
using System.Collections.Generic;
using SiteDownloader;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"..\..\..\..\ContentFolder";                         
            var scd = new SiteContentDownloader(path, 2, true);
            scd.FileExstention = new List<string> { ".jpg", ".gif", "pdf" };

            scd.LoadSite("https://www.google.com/");
            scd.LoadSite("https://www.yandex.kz/");
            scd.LoadSite("https://www.tiktok.com/");
            Console.Read();
        }
    }
}
