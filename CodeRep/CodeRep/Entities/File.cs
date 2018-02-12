using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository.Entities
{
    public class File
    {
        
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileDescription { get; set; }
        public DateTime CheckinDate { get; set; }
        public string path { get; set; }
        public int Version { get; set; }
        public string CheckedUser { get; set; }
        

    }
}
