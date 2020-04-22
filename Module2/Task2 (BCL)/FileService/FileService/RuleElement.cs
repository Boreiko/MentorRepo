using System.Configuration;

namespace FileService
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("filePattern", IsRequired = true, IsKey = true)]
        public string FilePattern => (string)this["filePattern"];

        [ConfigurationProperty("destFolder", IsRequired = true)]
        public string DestinationFolder => (string)this["destFolder"];

        [ConfigurationProperty("isDateAppended", IsRequired = false, DefaultValue = false)]
        public bool IsDateAppended => (bool)this["isDateAppended"];
    }
}
