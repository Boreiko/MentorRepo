using System;
using System.Collections.Generic;
using System.IO;


namespace FileSystemVisitorApp
{
    public class FileSystemVisitor
    {
        DirectoryInfo directory;
        public Func<FileSystemInfo, bool> filter;
        bool isNeedStop = false;
        public Action action;

        public event EventHandler<StartEventArgs> onStart;
        public event EventHandler<FinishEventArgs> onFinish;
        public event EventHandler<ItemEventArgs> onFileFinded;
        public event EventHandler<ItemEventArgs> onFilteredFileFinded;
        public event EventHandler<ItemEventArgs> onDirectoryFinded;
        public event EventHandler<ItemEventArgs> onFilteredDirectoryFind;

        public FileSystemVisitor(){
        }
        public FileSystemVisitor(string path, Func<FileSystemInfo, bool> filter = null)
        {
            directory = new DirectoryInfo(path);
            this.filter = filter;
        }
        public IEnumerable<FileSystemInfo> StartByPassing()
        {
            onStart.Invoke(this, new StartEventArgs());

            foreach (var fileSystemInfo in GetFileInfo(directory))
            {
                yield return fileSystemInfo;
            }
            onFinish.Invoke(this, new FinishEventArgs());
        }

        IEnumerable<FileSystemInfo> GetFileInfo(DirectoryInfo directory)
        {
            foreach (var fileSystemInfo in directory.EnumerateFileSystemInfos())
            {
                if (isNeedStop) break;

                if (fileSystemInfo is DirectoryInfo)
                {
                    action = ProcessItem((DirectoryInfo)fileSystemInfo, onDirectoryFinded, onFilteredDirectoryFind);

                    if (action == Action.Continue)
                    {
                        yield return fileSystemInfo;

                        foreach (var fsi in GetFileInfo((DirectoryInfo)fileSystemInfo))
                        {
                            yield return fsi;
                        }
                        continue;
                    }
                }

                if (fileSystemInfo is FileInfo)
                {
                    action = ProcessItem((FileInfo)fileSystemInfo, onFileFinded, onFilteredFileFinded);
                }

                if (action == Action.Stop)
                {
                    isNeedStop = true;
                    yield break;
                }
                if (action == Action.Skip)
                {
                    continue;
                }
                yield return fileSystemInfo;
            }
        }

        public Action ProcessItem(FileSystemInfo item, EventHandler<ItemEventArgs> itemFinded, EventHandler<ItemEventArgs> filteredItemFinded)            
        {
            ItemEventArgs args = new ItemEventArgs
            {
                Item = item,
            };

            action = Action.Continue;

            itemFinded.Invoke(this, args);
 
            if (filter == null)
            {
                return action;
            }

            if (filter(item))
            {
                filteredItemFinded.Invoke(this, args);
                return action;
            }

            if (action == Action.Continue)
                return Action.Continue;

            return Action.Skip;
        }
    }
}
