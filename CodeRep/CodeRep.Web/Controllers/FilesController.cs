using CodeRep.Web.Controllers;
using CodeRepository;
using CodeRepository.Entities;
using CodeRepository.UI.Models;
using CodeRepository.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using System.Web.Http.Filters;

namespace CodeRepository.Web.Controllers
{
    public class FilesController : BaseApiController
    {
        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path)
              : base(path)
            { }

            public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
            {
                var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
                return name.Replace("\"", string.Empty);
                //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
            }
        }
        public FilesController(IRepositoryInterface repo)
            : base(repo)
        {
        }
        public IEnumerable<Models.FileModel> Get()
        {
            IQueryable<Entities.File> query;

            query = TheRepository.GetAllFiles();

            var results = query
            .ToList()
            .Select(s => TheModelFactory.Create(s));

            return results;
        }

        public HttpResponseMessage GetFile(int id)
        {
            try
            {
                var file = TheRepository.GetFile(id);
                if (file != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(file));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [System.Web.Http.ActionName("filesbyname")]
        public IEnumerable<Models.FileModel> GetFile(string filename, string project)
        {
            IQueryable<Entities.File> query;

            query = TheRepository.GetFilesByFileName(filename,project).OrderBy(s => s.FileId);

            var totalCount = query.Count();

            var results = query.ToList().Select(s => TheModelFactory.Create(s));

            return results;
        }




        //private static readonly string ServerUploadFolder = "C:\\Temp"; //Path.GetTempPath();

        //[HttpPost]
        //[ValidateMimeMultipartContentFilter]
        //public async Task<FileModel> UploadSingleFile()
        //{
        //    var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
        //    await Request.Content.ReadAsMultipartAsync(streamProvider);

        //    return new FileModel
        //    {
        //        //FileName = streamProvider.FileData.Select(entry => entry.LocalFileName),
        //        //Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName),
        //        //FileType = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType),
        //        FileDescription = streamProvider.FormData["description"],
        //        CheckinDate = DateTime.UtcNow,
        //        //UpdatedTimestamp = DateTime.UtcNow,
        //        //DownloadLink = "TODO, will implement when file is persisited"
        //    };
        //}

        /*public class ValidateMimeMultipartContentFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(HttpActionContext actionContext)
            {
                if (!actionContext.Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
            }

            public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
            {

            }

        }*/

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

       /* public Task<IEnumerable<FileDesc>> Post()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/logFile.txt");
            StreamWriter logFile = new StreamWriter(path);
            logFile.WriteLine("entered post() command");
            logFile.WriteLine(DateTime.Now.ToString());
            var folderName = "App_Data";
            var PATH = HttpContext.Current.Server.MapPath("~/" + folderName);
            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            if (Request.Content.IsMimeMultipartContent())
            {
                logFile.WriteLine("is MultipartContent");
                logFile.Close();
                var streamProvider = new CustomMultipartFormDataStreamProvider(PATH);
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IEnumerable<FileDesc>>(t =>
                {
                    logFile = new StreamWriter(path, true);
                    logFile.WriteLine("reading stream");
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }
                    logFile.WriteLine("about to write files");
                    logFile.Flush();
                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var info = new FileInfo(i.LocalFileName);
                        logFile.WriteLine("filename: " + i.LocalFileName);
                        logFile.Flush();
                        return new FileDesc(info.Name, rootUrl + "/" + folderName + "/" + info.Name, info.Length / 1024);
                        logFile.Close();
                    });
                    return fileInfo;
                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        }*/

        public HttpResponseMessage Post([FromBody] UI.Models.FileModel fileModel)
        {
            try
            {
                fileModel.Version = 0;
                ModelFactory mf = new ModelFactory();
                var entity = mf.ParseFile(fileModel);

                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read subject/tutor from body");

                if (TheRepository.Insert(entity) && TheRepository.SaveAll())
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
        }

        [System.Web.Http.ActionName("fileupdate")]
        [HttpPatch]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] UI.Models.FileModel fileModel)
        {
            try
            {
                fileModel.FileType = Path.GetExtension(fileModel.FileName);
                DateTime checkindate = DateTime.Now;
                fileModel.FileAccessed = checkindate;
                fileModel.Version = fileModel.Version + 1;
                ModelFactory mf = new ModelFactory();
                var entity = mf.ParseFile(fileModel);

                var originalProject = TheRepository.GetProject(fileModel.FilePath,  UserController.email);

                originalProject.LastModifiedDate = DateTime.Now;
                TheRepository.UpdateProject(originalProject);
                TheRepository.SaveAll();

                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read subject/tutor from body");

                if (TheRepository.Insert(entity) && TheRepository.SaveAll())
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
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var file = TheRepository.GetFile(id);

                if (file == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                //if (file.Project.Count > 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Can not delete course, students has enrollments in course.");
                //}

                if (TheRepository.DeleteFile(id) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /*
        [System.Web.Http.ActionName("fileupdate")]
        [HttpPatch]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] UI.Models.FileModel fileModel)
        {
            try
            {
                fileModel.FileType = Path.GetExtension(fileModel.FileName);
                DateTime checkindate = DateTime.Now;
                fileModel.FileAccessed = checkindate;
                fileModel.Version = fileModel.Version + 1;
                var updatedFile = TheModelFactory.ParseFile(fileModel);

                if (updatedFile == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read File");

                var originalFile = TheRepository.GetFile(fileModel.FileName, fileModel.FilePath);

                if (originalFile == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "File is not found");
                }
                else
                {
                    updatedFile.FileId = originalFile.FileId;
                }

                if (TheRepository.Update(originalFile, updatedFile) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(updatedFile));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        */
    }
}
