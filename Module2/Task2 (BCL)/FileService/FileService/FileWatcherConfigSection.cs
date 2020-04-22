using System.Configuration;
using System.Globalization;
using FilesDistributor.Configuration;

namespace FileService
{
    public class FileWatcherConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("culture", DefaultValue = "ru-RU")]
        public CultureInfo Culture => (CultureInfo)this["culture"];

        [ConfigurationCollection(typeof(Directory), AddItemName = "directory")]
        [ConfigurationProperty("directories", IsRequired = false)]
        public DirectoryCollection Directories => (DirectoryCollection)this["directories"];

        [ConfigurationCollection(typeof(RuleElement), AddItemName = "rule")]
        [ConfigurationProperty("rules", IsRequired = true)]
        public RuleCollection Rules => (RuleCollection)this["rules"];
    }
}
