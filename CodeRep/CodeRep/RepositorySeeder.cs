using CodeRep.Entities;
using CodeRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository
{
    class RepositorySeeder
    {
        RepositoryContext _ctx;
        public RepositorySeeder(RepositoryContext ctx)
        {
            _ctx = ctx;
        }

        public void Seed()
        {
            if (_ctx.Project.Count() > 0)
            {
                return;
            }

            try
            {
                /*foreach (var projectName in Projects)
                {
                    var project = new Project
                    {
                        ProjectName = projectName
                    };
                    _ctx.Project.Add(project);
                    _ctx.SaveChanges();
                }*/
                for (int i = 0; i < Projects.Length; i++)
                {
                    var projectdetails = SplitValue(Projects[i]);
                    DateTime convertedDate = Convert.ToDateTime(projectdetails[3]);
                    var project = new Project()
                    {
                        ProjectName = projectdetails[0],
                        ProjectLanguage = projectdetails[1],
                        ProjectDescription = projectdetails[2],
                        CreationDate = convertedDate,
                        LastModifiedDate = convertedDate,
                        UserId = RandomString(16)
                    };

                    _ctx.Project.Add(project);
                    /*int maxCoursesId = _ctx.Courses.Max(c => c.Id);

                    //To enroll in 4 courses randomly
                    for (int z = 0; z < 4; z++)
                    {

                        int randomCourseId = new Random().Next(1, maxCoursesId);

                        var enrollment = new Enrollment
                        {
                            Student = student,
                            Course = _ctx.Courses.Where(c => c.Id == randomCourseId).Single(),
                            EnrollmentDate = DateTime.UtcNow.AddDays(-new Random().Next(10, 30))
                        };
                        _ctx.Enrollments.Add(enrollment);
                    }*/
                    /* }*/
                }
                _ctx.SaveChanges();

                for (int i = 0; i < Files.Length; i++)
                {
                    //var project = _ctx.Project.Where(s => s.ProjectId == i + 27).Single();
                    var fileDetails = SplitValue(Files[i]);
                    DateTime convertedDate = Convert.ToDateTime(fileDetails[3]);
                    var co = new CoAuthor()
                    {
                        Project = fileDetails[0],
                        Author = fileDetails[1],
                        CoAuhtor = fileDetails[2]
                       
                    };

                    _ctx.CoAuthor.Add(co);
                }
                _ctx.SaveChanges();
                for (int i = 0; i < Files.Length; i++)
                {
                    //var project = _ctx.Project.Where(s => s.ProjectId == i + 27).Single();
                    var fileDetails = SplitValue(Files[i]);
                    DateTime convertedDate = Convert.ToDateTime(fileDetails[3]);
                    var file = new File()
                    {
                        FileName = fileDetails[0],
                        FileType = fileDetails[1],
                        FileDescription = fileDetails[2],
                        CheckinDate = convertedDate,
                        path = "."
                    };

                    _ctx.File.Add(file);
                }
                _ctx.SaveChanges();
                /*for (int i = 0; i < tutorNames.Length; i++)
                    {
                        var nameGenderMail = SplitValue(tutorNames[i]);
                        var tutor = new Tutor
                        {
                            Email = String.Format("{0}.{1}@{2}", nameGenderMail[0], nameGenderMail[1], nameGenderMail[3]),
                            UserName = String.Format("{0}{1}", nameGenderMail[0], nameGenderMail[1]),
                            Password = RandomString(8),
                            FirstName = nameGenderMail[0],
                            LastName = nameGenderMail[1],
                            Gender = ((Enums.Gender)Enum.Parse(typeof(Enums.Gender), nameGenderMail[2]))
                        };

                        _ctx.Tutors.Add(tutor);

                        var courseSubject = _ctx.Subjects.Where(s => s.Id == i + 1).Single();

                        foreach (var CourseDataItem in CoursesSeedData.Where(c => c.SubjectID == courseSubject.Id))
                        {
                            var course = new Course
                            {
                                Name = CourseDataItem.CourseName,
                                CourseSubject = courseSubject,
                                CourseTutor = tutor,
                                Duration = new Random().Next(3, 6),
                                Description = String.Format("The course will talk in depth about: {0}", CourseDataItem.CourseName)
                            };
                            _ctx.Courses.Add(course);
                        }
                    }

                    _ctx.SaveChanges();
                    */
                for (int i = 0; i < Users.Length; i++)
                {
                    var nameGenderMail = SplitValue(Users[i]);
                    var user = new User()
                    {
                        Email = String.Format("{0}.{1}@{2}", nameGenderMail[0], nameGenderMail[1], nameGenderMail[3]),
                        UserName = String.Format("{0}{1}", nameGenderMail[0], nameGenderMail[1]),
                        Password = RandomString(8),
                        FirstName = nameGenderMail[0],
                        LastName = nameGenderMail[1],
                        Gender = ((Enums.Gender)Enum.Parse(typeof(Enums.Gender), nameGenderMail[2])),
                        DateOfBirth = DateTime.UtcNow.AddDays(-new Random().Next(7000, 8000)),
                        RegistrationDate = DateTime.UtcNow.AddDays(-new Random().Next(365, 730)),
                        Description = nameGenderMail[4]
                    };

                    _ctx.User.Add(user);

                    /*int maxCoursesId = _ctx.Courses.Max(c => c.Id);

                    //To enroll in 4 courses randomly
                    for (int z = 0; z < 4; z++)
                    {

                        int randomCourseId = new Random().Next(1, maxCoursesId);

                        var enrollment = new Enrollment
                        {
                            Student = student,
                            Course = _ctx.Courses.Where(c => c.Id == randomCourseId).Single(),
                            EnrollmentDate = DateTime.UtcNow.AddDays(-new Random().Next(10, 30))
                        };
                        _ctx.Enrollments.Add(enrollment);
                    }*/
                }
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw ex;
            }

        }

        private static string[] SplitValue(string val)
        {
            return val.Split(',');
        }

        private string RandomString(int size)
        {
            Random _rng = new Random((int)DateTime.Now.Ticks);
            string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        static string[] Users =
        {
            "Taiseer,Joudeh,Male,hotmail.com,Test User",
            "Hasan,Ahmad,Male,mymail.com,Test User",
            "Moatasem,Ahmad,Male,outlook.com,Test User",
            "Salma,Tamer,Female,outlook.com,Test User",
            "Ahmad,Radi,Male,gmail.com,Test User",
            "Bill,Gates,Male,yahoo.com,Test User",
            "Shareef,Khaled,Male,gmail.com,Test User",
            "Aram,Naser,Male,gmail.com,Test User",
            "Layla,Ibrahim,Female,mymail.com,Test User",
            "Rema,Oday,Female,hotmail.com,Test User",
            "Fikri,Husein,Male,gmail.com,Test User",
            "Zakari,Husein,Male,outlook.com,Test User",
            "Taher,Waleed,Male,mymail.com,Test User",
            "Tamer,Wesam,Male,yahoo.com,Test User",
            "Khaled,Hasaan,Male,gmail.com,Test User",
            "Asaad,Ibrahim,Male,hotmail.com,Test User",
            "Tareq,Nassar,Male,outlook.com,Test User",
            "Diana,Lutfi,Female,outlook.com,Test User",
            "Tamara,Malek,Female,gmail.com,Test User",
            "Arwa,Kamal,Female,yahoo.com,Test User",
            "Jana,Ahmad,Female,yahoo.com,Test User",
            "Nisreen,Tamer,Female,gmail.com,Test User",
            "Noura,Ahmad,Female,outlook.com,Test User"
        };

        static string[] Files =
        {
            "Ahmad,Joudeh,Male,04/26/2017",
            "Taiseer,Ahmad,Male,04/26/2017",
            "Taymour,Wardi,Male,04/26/2017",
            "Kareem,Ismail,Male,04/26/2017",
            "Iyad,Radi,Male,04/26/2017",
            "Ramdan,Ahmad,Male,04/26/2017",
            "Shareef,Khaled,Male,04/26/2017",
            "Ibrahim,Naser,Male,04/26/2017",
            "Layla,Ibrahim,Female,04/26/2017",
            "Nisreen,Wesam,Female,04/26/2017"
        };

        static string[] Projects =
        {
            "History,C#,Good project,01/26/2017",
            "Science,C#,Good project,01/26/2017",
            "Geography,C#,Good project,01/26/2017",
            "English,C#,Good project,01/26/2017",
            "Commerce,C#,Good project,01/26/2017",
            "Computing,C#,Good project,01/26/2017",
            "Human Resource,C#,Good project,01/26/2017",
            "Mathematics,C#,Good project,01/26/2017",
            "Music,C#,Good project,01/26/2017",
            "Personal Development,C#,Good project,01/26/2017"
        };

        static IList<FileSeed> FileSeedData = new List<FileSeed>
        {
            new FileSeed {FileID =1, FileName="History Teaching Methods 1"},
            new FileSeed {FileID =1, FileName="History Teaching Methods 2"},
            new FileSeed {FileID =1, FileName="History Teaching Methods 3"},

            new FileSeed {FileID =2, FileName="Professional Experience 1 (Mathematics/Science)"},
            new FileSeed {FileID =2, FileName="Professional Experience 2 (Mathematics/Science)"},
            new FileSeed {FileID =2, FileName="Professional Experience 3 (Mathematics/Science)"},

             new FileSeed {FileID =3, FileName="Geography Teaching Methods 1"},
            new FileSeed {FileID =3, FileName="Geography Teaching Methods 2"},
            new FileSeed {FileID =3, FileName="Geography Teaching Methods 3"},

             new FileSeed {FileID =4, FileName="English Education 1"},
            new FileSeed {FileID =4, FileName="English Education 2"},
            new FileSeed {FileID =4, FileName="English Education 3"},
             new FileSeed {FileID =4, FileName="English Teaching Methods 1"},
            new FileSeed {FileID =4, FileName="English Teaching Methods 2"},

             new FileSeed {FileID =5, FileName="Commerce, Business Studies and Economics Teaching Methods 1"},
            new FileSeed {FileID =5, FileName="Commerce, Business Studies and Economics Teaching Methods 2"},
            new FileSeed {FileID =5, FileName="Commerce, Business Studies and Economics Teaching Methods 3"},

             new FileSeed {FileID =6, FileName="Computing Studies Teaching Methods 1"},
            new FileSeed {FileID =6, FileName="Computing Studies Teaching Methods 2"},
            new FileSeed {FileID =6, FileName="Computing Studies Teaching Methods 3"},

            new FileSeed {FileID =7, FileName="Human Resource Development in Organisations"},
            new FileSeed {FileID =7, FileName="Human Resources and Organisational Development"},

           new FileSeed {FileID =8, FileName="Mathematics Teaching and Learning 1"},
            new FileSeed {FileID =8, FileName="Mathematics Teaching and Learning 2"},
            new FileSeed {FileID =8, FileName="Mathematics Teaching Methods 1"},
            new FileSeed {FileID =8, FileName="Mathematics Teaching Methods 2"},

              new FileSeed {FileID =9, FileName="Music Study 1"},
            new FileSeed {FileID =9, FileName="Music Therapy 1"},
            new FileSeed {FileID =9, FileName="Music, Movement and Dance"},

             new FileSeed {FileID =10, FileName="Personal Development, Health and Physical Education 1"},
            new FileSeed {FileID =10, FileName="Personal Development, Health and Physical Education Teaching Methods 1"},
             new FileSeed {FileID =10, FileName="Personal Development, Health and Physical Education Teaching Methods 2"}
        };

        class FileSeed
        {
            public int FileID { get; set; }
            public string FileName { get; set; }
        }
    }
}
