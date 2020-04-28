using System;
using System.IO;

namespace FileSystemVisitorApp
{
    public class ItemEventArgs : EventArgs
    {
        public FileSystemInfo Item { get; set; }

    }
}
