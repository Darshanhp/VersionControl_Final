using CodeRepository.Entities;
using CodeRepository.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.IO.Compression;
using Ionic.Zip;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using CodeRep.Web.Controllers;

namespace CodeRepository.Web.Controllers
{
    public class ProjectsController : BaseApiController
    {
        public static string filename;

        public ProjectsController(IRepositoryInterface repo)
            : base(repo)
        {
        }

        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path)
              : base(path)
            { }

            public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
            {
                var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
                filename = name.Replace("\"", string.Empty);
                return name.Replace("\"", string.Empty);
                //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
            }
        }
        [System.Web.Http.ActionName("All")]
        // GET: Project
        [System.Web.Http.HttpGet]
        public IEnumerable<ProjectModel> Get()
        {
            IQueryable<Project> query;

            query = TheRepository.GetAllProjects();

            var results = query
            .ToList()
            .Select(s => TheModelFactory.Create(s));

            return results;
        }

        [System.Web.Http.ActionName("SearchProject")]
        [System.Web.Http.HttpGet]
        public IEnumerable<ProjectModel> SearchProject(string name)
        {
            IQueryable<Entities.Project> query;

            query = TheRepository.SearchProject(name).OrderBy(s => s.ProjectId);

            var totalCount = query.Count();

            System.Web.HttpContext.Current.Response.Headers.Add("X-InlineCount", totalCount.ToString());

            var results = query.ToList().Select(s => TheModelFactory.Create(s));

            return results;

        }

        [System.Web.Http.ActionName("filesproject")]
        [System.Web.Http.HttpGet]
        public IEnumerable<FileModel> GetFilesInProject(string projectId)
        {
            IQueryable<Entities.File> query;

            query = TheRepository.GetFilesInProject(projectId).OrderBy(s => s.FileId);

            var totalCount = query.Count();

            System.Web.HttpContext.Current.Response.Headers.Add("X-InlineCount", totalCount.ToString());

            var results = query.ToList().Select(s => TheModelFactory.Create(s));

            return results;

        }

        public class FileDesc
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public long Size { get; set; }
            public FileDesc(string n, string p, long s)
            {
                Name = n;
                Path = p;
                Size = s;
            }
        }


        public HttpResponseMessage Post()
        {
            var folderName = "App_Data";
            var PATH = HttpContext.Current.Server.MapPath("~/" + folderName);
            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new CustomMultipartFormDataStreamProvider(PATH);
                
                ///////////////////
                
                Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
                MemoryStream tempStream = new MemoryStream();
                reqStream.CopyTo(tempStream);

                
                tempStream.Seek(0, SeekOrigin.End);
                StreamWriter writer = new StreamWriter(tempStream);
                writer.WriteLine();
                writer.Flush();
                tempStream.Position = 0;


                StreamContent streamContent = new StreamContent(tempStream);
                foreach (var header in Request.Content.Headers)
                {
                    streamContent.Headers.Add(header.Key, header.Value);
                }
                
                ////////////////////////////////
                var task = streamContent.ReadAsMultipartAsync(streamProvider).ContinueWith<IEnumerable<FileDesc>>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        string ex1 = t.Exception.InnerException.GetType().Name;
                        string ex2 = t.Exception.InnerException.Message;
                        Console.WriteLine(t.Exception.Message);
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }
                    IEnumerable<FileDesc> fileInfo = null;
                    return fileInfo;
                });
                //HttpContent requestContent = Request.Content;
                //string jsonContent = requestContent.ReadAsStringAsync().Result;
                //filename = JsonConvert.DeserializeObject<string>(jsonContent);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }


        }

        public HttpResponseMessage Options()
        {
            var zipFileToRead = HttpContext.Current.Server.MapPath("~/App_Data/" + filename);
            var extractToFolder = HttpContext.Current.Server.MapPath("~/" + HomeController.email+"/"+filename);
            extractToFolder = extractToFolder.Replace(".zip", "_folder");
            extractToFolder = extractToFolder.Replace("\\CodeRep.Web", "");
            //"c:\\Users\\Darshan\\documents\\visual studio 2015\\Projects\\CodeRep\\CodeRep.Web\\BookService-master_folder"
            if (Directory.Exists(extractToFolder))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(extractToFolder);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(extractToFolder));
            }
            //Directory.CreateDirectory(extractToFolder);

            using (var zip = Ionic.Zip.ZipFile.Read(zipFileToRead))
            {
                foreach (var entry in zip.Entries)
                    entry.Extract(extractToFolder, ExtractExistingFileAction.OverwriteSilently);
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
        /*
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("projectdetails")]
        public HttpResponseMessage PostDetails([FromBody] ProjectModel projectModel)
        {

            try
            {
                ModelFactory mf = new ModelFactory();
                var entity = mf.ParseProject(projectModel);
                entity.UserId = User.Identity.GetUserId();
                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read subject/tutor from body");

                if (TheRepository.InsertProject(entity) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }*/

        /*public HttpResponseMessage Post(int projectId, [FromUri]string fileId, [FromBody]File File)
        {
            try
            {

                if (!TheRepository.CourseExists(courseId)) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find Course");

                var student = TheRepository.GetStudent(userName);
                if (student == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find Student");

                var result = TheRepository.EnrollStudentInCourse(student.Id, courseId, enrollment);

                if (result == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else if (result == 2)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Student already enrolled in this course");
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }*/





        //public File GetFile(int id)
        //{
        //    IRepositoryInterface repository = new RepositoryInterface(new RepositoryContext());

        //    return repository.GetFile(id);
        //}
    }
}