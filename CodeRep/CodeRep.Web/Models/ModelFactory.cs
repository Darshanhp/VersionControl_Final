using CodeRep.Entities;
using CodeRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace CodeRepository.Web.Models
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private IRepositoryInterface _repo;
        public ModelFactory(HttpRequestMessage request, IRepositoryInterface repo)
        {
            _UrlHelper = new UrlHelper(request);
            _repo = repo;
        }

        public ModelFactory()
        {
        }

        public FileModel Create(File file)
        {
            return new FileModel()
            {
                FileId = file.FileId,
                FileName = file.FileName,
                FileType = file.FileType,
                FileText = file.FileDescription,
                FileAccessed = file.CheckinDate,
                FilePath = file.path,
                Version = file.Version,
                CheckedUser = file.CheckedUser
            };
        }

        public ProjectModel Create(Project project)
        {
            return new ProjectModel()
            {
                Url = _UrlHelper.Link("Projects", new { projectId = project.ProjectId }),
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectLanguage = project.ProjectLanguage,
                ProjectDescription = project.ProjectDescription,
                CreationDate = project.CreationDate,
                UserId = project.UserId,
                LastModifiedDate= project.LastModifiedDate
            };
        }

        public UserModel Create(User user)
        {
            return new UserModel()
            {
                Url = _UrlHelper.Link("Users", new { userName = user.UserName }),
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                RegistrationDate = user.RegistrationDate,
                LastLoginDate = user.LastLoginDate,
                Description = user.Description
            };
        }

        public UserAccessModel Create(UserAccess useraccess)
        {
            return new UserAccessModel()
            {
                AccessId = useraccess.AccessId,
                Project = Create(useraccess.Project),
                User = Create(useraccess.User),
                Role = useraccess.Role
            };
        }



        public Project ParseProject(ProjectModel model)
        {
            try
            {
                var project = new Project()
                {
                    ProjectName = model.ProjectName,
                    ProjectLanguage = model.ProjectLanguage,
                    ProjectDescription = model.ProjectDescription,
                    CreationDate = model.CreationDate,
                    UserId = model.UserId
            };

                return project;
            }
            catch (Exception)
            {

                return null;
            }
        }


        public CoAuthor ParseCoAuthor(ProjectModel model)
        {
            try
            {
                var co = new CoAuthor()
                {
                    Project = model.ProjectName,
                    Author = model.ProjectDescription,
                    CoAuhtor = model.UserId
                };

                return co;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public File ParseFile(UI.Models.FileModel model)
        {
            try
            {
                var file = new File()
                {
                    FileName = model.FileName,
                    FileType = model.FileType,
                    FileDescription = model.FileText,
                    CheckinDate = model.FileAccessed,
                    path = model.FilePath,
                    Version = model.Version,
                    CheckedUser = model.CheckedUser
                };

                return file;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}