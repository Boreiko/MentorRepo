using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Messages = FileService.Resources.Messages; 

namespace FileService
{
    class FileWatcher
    {
        FileSystemWatcher watcher { get; set; } 
        List<Rule> rules { get; }
        string filter { get; set; }
        string DefaultPath { get; set; }

        public FileWatcher(string path, string filter , string defaultPath) {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            DefaultPath = defaultPath;
        }

        public FileWatcher(string path, string filter, string defaultPath, List<Rule> rules) : this(path, filter, defaultPath)
        {          
            this.rules = rules;
        }

        void NotifyWatcherFilters()
        {
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

            watcher.Filter = filter;   // "*.txt"
        }

        void SubscribeHandlers()
        {
            watcher.Created += OnCreated;
            watcher.Renamed += OnRenamed;
        }
        public void Run()
        {       
            watcher.EnableRaisingEvents = true;
            NotifyWatcherFilters();
            SubscribeHandlers();

           
            Console.WriteLine(Messages.Quit);
            ConsoleKeyInfo input = Console.ReadKey();
            while (input.Key != ConsoleKey.Escape);
            
        }

        void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine(Messages.FileCreated, e.FullPath, File.GetCreationTime(e.FullPath));
        }
          void OnRenamed(object source, RenamedEventArgs renamedEventArgs)
          {
            Console.WriteLine(Messages.FileRenamed, renamedEventArgs.OldFullPath, renamedEventArgs.FullPath);
            CheckRule(renamedEventArgs); // check file name in dictionary rules 
        }
        void CheckRule(RenamedEventArgs renamedEventArgs)
        {
            string destinationPath = null; 

            foreach (var rule in rules) // check rules matches
            {
                var match = Regex.Match(renamedEventArgs.Name, rule.FilePattern);
                if (match.Success) // if file name was found in dictionary move to dest path
                {
                    try
                    {
                        destinationPath = CreatePath(rule,rule.DestinationFolder, renamedEventArgs.Name); // get dest file path

                        if (File.Exists(destinationPath)) // if file already exist in dest path
                            throw new IOException();

                        File.Move(renamedEventArgs.FullPath, destinationPath);
                        Console.WriteLine(Messages.MovedRuleMatch, destinationPath);
                        return;
                    }
                    catch (IOException)
                    {
                        Console.WriteLine(Messages.FileExist);
                        return;
                    }
                }
            }
    
                destinationPath = CreatePath(null, DefaultPath, renamedEventArgs.Name);
                try
                {
                    if (File.Exists(destinationPath)) // if file already exist in dest path
                        throw new IOException();
                    File.Move(renamedEventArgs.FullPath, destinationPath); // move to default folder
                }
                catch (IOException)
                {
                    Console.WriteLine(Messages.FileExist);
                    return;
                }
                
                Console.WriteLine(Messages.MovedRuleNotMatch, destinationPath);
            
        }

        string CreatePath(Rule rule, string dest , string fileName)
        {   
            string extension = fileName.Substring(fileName.LastIndexOf("."));
            string fileNameWithoutExstension = fileName.Remove(fileName.LastIndexOf("."));
            string resultPath = dest+ "\\";
            resultPath += fileNameWithoutExstension;

            if (rule!=null && rule.IsDateAppended) {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
                dateTimeFormat.DateSeparator = ".";
                
                resultPath += DateTime.Now.ToLocalTime().ToString(dateTimeFormat.ShortDatePattern); // add date to file name through '.' separator            
            }           
           
            resultPath += extension;
            return resultPath;
        }
    }
}
