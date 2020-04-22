using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using FileWatcherConfigSection = FileService.FileWatcherConfigSection;


namespace FileService
{
    class Program
    {
        static List<string> directories;
        static List<Rule> rules;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            FileWatcherConfigSection config = (FileWatcherConfigSection)ConfigurationManager.GetSection("FileWatcherSection");

            if (config != null)
            {
                ReadConfiguration(config);
            }
            else
            {
                Console.Write("Cannot find");
                return;
            }

            string path = directories.First();
            string defaultPath = config.Rules.DefaultDirectory;
            
            FileWatcher fileWatcher = new FileWatcher(path, ".txt", defaultPath, rules);
            fileWatcher.Run();
            
        }
        private static void ReadConfiguration(FileWatcherConfigSection config)
        {
            directories = new List<string>(config.Directories.Count);
            rules = new List<Rule>();

            CultureInfo.DefaultThreadCurrentCulture = config.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = config.Culture;
            CultureInfo.CurrentUICulture = config.Culture;
            CultureInfo.CurrentCulture = config.Culture;

            foreach (Directory directory in config.Directories)
            {
                directories.Add(directory.Path);
            }
            foreach (RuleElement rule in config.Rules)
            {
                rules.Add(new Rule
                {
                    FilePattern = rule.FilePattern,
                    DestinationFolder = rule.DestinationFolder,
                    IsDateAppended = rule.IsDateAppended
                });
            }
          
        }
    }
}
