using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository.Entities
{
    public class User
    {
        public User()
        {
            UserAccess = new List<UserAccess>();
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Enums.Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Description { get; set; }
        //public image LastName { get; set; }




        public ICollection<UserAccess> UserAccess;
    }
}
