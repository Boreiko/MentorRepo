using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService
{
    public class Rule
    {
        public string FilePattern { get; set; }
        public string DestinationFolder { get; set; }
        public bool IsDateAppended { get; set; }

     }
}
