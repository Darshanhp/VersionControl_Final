using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeRepository.Web.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string Url { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLanguage { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string UserId { get; set; }

    }
}