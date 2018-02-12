using CodeRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeRepository.Web.Models
{
    public class FileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileSizeText { get; set; }
        public string FileText { get; set; }
        public string FileType { get; set; }  
        public DateTime FileAccessed { get; set; }
        //public String Project { get; set; }
        public string FilePath { get; set; }
        //public Project Project { get; set; }
        public int Version { get; set; }
        public string CheckedUser { get; set; }

    }
}