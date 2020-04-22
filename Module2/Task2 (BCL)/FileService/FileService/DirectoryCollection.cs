using FileService;
using System.Configuration;

namespace FilesDistributor.Configuration
{
    public class DirectoryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Directory();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Directory)element).Path;
        }
    }
}
