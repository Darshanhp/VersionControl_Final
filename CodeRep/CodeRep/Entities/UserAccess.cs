using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository.Entities
{
    public class UserAccess
    {
        public UserAccess()
        {
            User = new User();
            Project = new Project();
        }
        public int AccessId { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
        public string Role { get; set; }
    }
}
