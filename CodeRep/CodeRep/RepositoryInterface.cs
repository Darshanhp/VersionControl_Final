using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeRepository.Entities;
using System.Text.RegularExpressions;
using System.IO.Compression;
using CodeRep.Entities;

namespace CodeRepository
{
    public class RepositoryInterface : IRepositoryInterface
    {
        private RepositoryContext _ctx;
        public RepositoryInterface(RepositoryContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<File> GetAllFiles()
        {
            return _ctx.File
                    .AsQueryable();
        }

        public IQueryable<Project> GetAllProjects()
        {
            return _ctx.Project
                    .AsQueryable();
        }

        public IQueryable<User> GetAllUsers()
        {
            return _ctx.User
                    .AsQueryable();
        }

        public IQueryable<File> GetFileByProject(string ProjectId)
        {
            return _ctx.File
                    .Include("Project")
                    .Where(c => c.path == ProjectId)
                    .AsQueryable();
        }

        public IQueryable<File> GetFilesByFileName(string filename,string project)
        {
            return _ctx.File
                    .Where(c => c.FileName == filename)
                    .Where(c => c.path == project)
                    .AsQueryable();
        }
        public File GetFile(string FileName,string Path)
        {

            return _ctx.File
                   .Where(c => c.FileName == FileName)
                   .Where(c => c.path == Path)
                   .SingleOrDefault();
        }

        public File GetFile(int id)
        {

            return _ctx.File
                   .Where(c => c.FileId == id)
                   .SingleOrDefault();
        }
        public File GetFile(string filename)
        {

            return _ctx.File
                   .Where(c => c.FileName == filename)
                   .SingleOrDefault();
        }
        public IQueryable<Project> GetProject(string email)
        {

            return _ctx.Project
                   .Where(c => c.UserId == email).AsQueryable();
        }
        public CoAuthor GetAuthor(string email,string project)
        {

            return _ctx.CoAuthor
                   .Where(c => c.CoAuhtor == email)
                   .Where(c => c.Project == project)
                   .SingleOrDefault();
        }
        public CoAuthor GetAuthor(string project)
        {

            return _ctx.CoAuthor
                   .Where(c => c.Project == project)
                   .SingleOrDefault();
        }
        public Project GetProject(string projectName, string user)
        {
            return _ctx.Project
                   .Where(c => c.ProjectName == projectName)
                   .Where(c => c.UserId == user)
                   .SingleOrDefault();
        }
        public IQueryable<Project> GetProjectbyname(string projectName)
        {
            return _ctx.Project
                   .Where(c => c.ProjectName == projectName).AsQueryable();
        }
        public bool Insert(File file)
        {
            try
            {
                _ctx.File.Add(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertProject(Project project)
        {
            try
            {
                _ctx.Project.Add(project);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public bool Update(File origianlFile, File updatedFile)
        {
            _ctx.Entry(origianlFile).CurrentValues.SetValues(updatedFile);
            //To update child entites in Course entity

            return true;
        }

        public IQueryable<File> GetFilesInProject(string ProjectId)
        {
            return _ctx.File
                    .Where(c => c.path == ProjectId)
                    .AsQueryable();

        }

        public IQueryable<Project> SearchProject(string projectname)
        {
            return _ctx.Project
                    .Where(c => c.ProjectName.Contains(projectname))
                    .AsQueryable();

        }

        public bool DeleteFile(int id)
        {
            try
            {
                var entity = _ctx.File.Find(id);
                if (entity != null)
                {
                    _ctx.File.Remove(entity);
                    return true;
                }
            }
            catch
            {
                //ToDo: Logging
            }

            return false;
        }

        public bool InsertCoAuthor(CoAuthor co)
        {

            _ctx.CoAuthor.Add(co);
            return true;
        }

        public void Compress(System.IO.DirectoryInfo directoryPath)
        {
            foreach (System.IO.DirectoryInfo directory in directoryPath.GetDirectories())
            {
                var path = directoryPath.FullName;
                var newArchiveName = Regex.Replace(directory.Name, "[0-9]{8}", "20130913");
                newArchiveName = Regex.Replace(newArchiveName, "[_]+", "_");
                string startPath = path + directory.Name;
                string zipPath = path + "" + newArchiveName + ".zip";

                ZipFile.CreateFromDirectory(startPath, zipPath);
            }

        }
        public void Decompress(string zipPath, string extractPath)
        {
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }

        public IEnumerable<T> Add<T>( IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        public bool UpdateProject(Project project)
        {
            var entity = _ctx.Project.Find(project.ProjectId);
            _ctx.Entry(entity).CurrentValues.SetValues(project);
            return true;
        }
    }
}
