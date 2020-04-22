using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;

namespace FileSystemVisitorApp
{
    class FileSystemVisitor : IEnumerable
    {
        List<FileSystemInfo> finalList { get; set; }
        IEnumerable<FileInfo> files { get; set; }
        IEnumerable<DirectoryInfo> directories { get; set; }
        DirectoryInfo dirInfo { get; set; }

        public delegate void DisplayDelegate();
        
        public event DisplayDelegate onStart;
        public event DisplayDelegate onFinish;
        public event DisplayDelegate onFileFind;
        public event DisplayDelegate onDirectoryFind;
        public event DisplayDelegate onFilteredFileFind;
        public event DisplayDelegate onFilteredDirectoryFind;

        Func<FileSystemInfo, bool> filterDelegate;

        bool isNeedStopSearching;
        bool isNeedStopRecursion;
        bool isNeedExclude;
        public FileSystemVisitor(string path)
        {
            dirInfo = new DirectoryInfo(path);
            finalList = new List<FileSystemInfo>(); 
        }

        public FileSystemVisitor(string path, Func<FileSystemInfo, bool> filterDelegate, bool isNeedStopSearching = false, bool isNeedExclude= false) : this(path)
        {
            this.filterDelegate = filterDelegate;
            this.isNeedExclude = isNeedExclude;
            this.isNeedStopSearching = isNeedStopSearching;
        }

        public void Start()
        {
            onStart();
            GetFiles(dirInfo);           
            onFinish();
        }

        void GetFiles(DirectoryInfo dirInfo)
        {
            if (filterDelegate == null)    // if don't need filter get files
            {  
                finalList.Add(dirInfo);

                onDirectoryFind?.Invoke();

                files = dirInfo.GetFiles();

                if (files.Count() != 0)           // Add files to Final list
                {
                    foreach (var file in files)
                    {
                        onFileFind?.Invoke();
                        finalList.Add(file);
                    }
                }

                directories = dirInfo.GetDirectories();

                foreach (var dir in directories)
                {
                    dirInfo = dir;
                    GetFiles(dirInfo);
                }
            }
            else  // if Need Filter 
            {
                GetFilterFiles();
            }
        }
        void GetFilterFiles()
        {
            if (!isNeedExclude)    // if Finded Directory Filter DO NOT NEED exclude from final list
            {
                if (filterDelegate(dirInfo))    // if filter matched add to final list
                {
                    finalList.Add(dirInfo);
                    onFilteredDirectoryFind?.Invoke();

                    if (isNeedStopSearching) { isNeedStopRecursion = true; return; }  // STOP RECURSION 
                }
            }
            else                   // if Finded directory Filter NEED exclude from final list
            {
                if (!filterDelegate(dirInfo)) // if directory finded DO NOT add to Final List
                    finalList.Add(dirInfo);
                else
                {
                    onFilteredDirectoryFind?.Invoke();
                }

                if (isNeedStopSearching) { isNeedStopRecursion = true; return; }   // STOP RECURSION 
            }

            files = dirInfo.GetFiles();

            if (files.Count() != 0)
            {
                foreach (var file in files)
                {
                    if (!isNeedExclude)      // if Finded File Filter DO NOT NEED exclude from final list
                    {
                        if (filterDelegate(file))    
                        {
                            finalList.Add(file);
                            onFilteredFileFind?.Invoke();

                            if (isNeedStopSearching) { isNeedStopRecursion = true; ; return; }
                        }
                    }

                    else
                    {
                        if (!filterDelegate(file))
                            finalList.Add(file);
                        else
                        {
                            onFilteredFileFind?.Invoke();
                        }
                    }
                }
            }

            directories = dirInfo.GetDirectories();

            foreach (var dir in directories)
            {
                if (isNeedStopRecursion) return;
                dirInfo = dir;
                GetFiles(dirInfo);
            }
        }
         public IEnumerator GetEnumerator()
        {
            foreach (var item in finalList)
            {
                yield return item.FullName;
            }
        }
    }
}
