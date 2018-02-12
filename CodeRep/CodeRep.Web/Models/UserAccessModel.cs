using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeRepository.Web.Models
{
    public class UserAccessModel
    {
        public int AccessId { get; set; }
        public ProjectModel Project { get; set; }
        public UserModel User { get; set; }
        public string Role { get; set; }
    }
}