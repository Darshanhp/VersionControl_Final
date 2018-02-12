using CodeRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeRepository.UI.Models
{
    public class DirModel
    {
        public string DirName { get; set; }
        public DateTime DirAccessed { get; set; }
    }

    public class FileModel
    {
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FileSizeText { get; set; }
        public string FileText { get; set; }
        public string FileType { get; set; }
        public DateTime FileAccessed { get; set; }
        public string FilePath { get; set; }
        public int Version { get; set; }
        public string CheckedUser { get; set; }

        //public Project Project { get; set; }

    }

    public class ExplorerModel
    {
        public List<DirModel> dirModelList;
        public List<FileModel> fileModelList;

        public ExplorerModel(List<DirModel> _dirModelList, List<FileModel> _fileModelList)
        {
            dirModelList = _dirModelList;
            fileModelList = _fileModelList;
        }
    }

}