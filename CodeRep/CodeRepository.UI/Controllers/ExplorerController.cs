using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeRepository.UI.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CodeRep.Entities;
using CodeRepository.Entities;

namespace CodeRepository.UI.Controllers
{
    [Authorize]
    public class ExplorerController : Controller
    {
        public static string realfilename = "";
        public static string authoremail = "";
        public static string searchfilename = "";
        public static string searchauthoremail = "";
        //

         //This method is used to exxplore the project folder on search
        // GET: /Explorer/
        public ActionResult Search(string path)
        {
            string email = "";
            string mainemail = "";
            if (path != null)
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:33803");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    path = path.Replace("_folder", ".zip");
                    string url = "api/User/get/?name=" +"search|o|"+ path;
                    try
                    {
                        var response = client.GetAsync(url).Result;
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        IEnumerable<ProjectModel> myObject = JsonConvert.DeserializeObject<IEnumerable<ProjectModel>>(responseString);
                        ProjectModel pr = new ProjectModel();
                        pr = myObject.First();
                        email = pr.UserId;
                        CoAuthor Auth = new CoAuthor();
                        
                        string checkurl = "api/home/get/?name=" + path + "|o|" + email;
                        try
                        {
                            var Authresponse = client.GetAsync(checkurl).Result;
                            string AuthresponseString = Authresponse.Content.ReadAsStringAsync().Result;
                            //var myobject = response.Content.ReadAsAsync<ProjectModel>();
                            Auth = JsonConvert.DeserializeObject<CoAuthor>(AuthresponseString);
                            
                            if(Auth == null)
                            {
                                searchauthoremail = email;
                            }else
                            {
                                searchauthoremail = Auth.Author;
                            }
                        }
                        catch
                        {

                        }
                    }
                    catch
                    {

                    }
                }

            }
            String realfile = path;
            string realPath;
            if (path == null)
            {
                path = searchfilename;
            }
            path = path.Replace(".zip", "_folder");
            realPath = Server.MapPath("~/" + searchauthoremail + "/" + path);
            realPath = realPath.Replace("\\CodeRepository.UI", "");
            realPath = realPath.Replace("\\search", "");
            if (System.IO.File.Exists(realPath))
            {

                FileModel fileModel = new FileModel();

                String filename = Path.GetFileName(path);
                fileModel.FileName = filename;
                fileModel.FileType = Path.GetExtension(filename);
                fileModel.FilePath = searchfilename;
                string filedetail = fileModel.FileName + "|o|" + fileModel.FilePath + "|o|" + fileModel.FileSizeText + "|o|" + fileModel.Version;
                return RedirectToAction("FileDetail", "Files", new { filedetail = filedetail });
            }
            else if (System.IO.Directory.Exists(realPath))
            {

                Uri url = Request.Url;
                //Every path needs to end with slash
                if (url.ToString().Last() != '/')
                {
                    searchfilename = realfile;

                    Response.Redirect("/Explorer/search/" + path + "/");

                    //Server.Transfer("Index.cshtml");
                }

                List<DirModel> dirListModel = new List<DirModel>();

                IEnumerable<string> dirList = Directory.EnumerateDirectories(realPath);
                foreach (string dir in dirList)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);

                    DirModel dirModel = new DirModel();

                    dirModel.DirName = Path.GetFileName(dir);
                    dirModel.DirAccessed = d.LastAccessTime;

                    dirListModel.Add(dirModel);
                }


                List<FileModel> fileListModel = new List<FileModel>();

                IEnumerable<string> fileList = Directory.EnumerateFiles(realPath);
                foreach (string file in fileList)
                {
                    FileInfo f = new FileInfo(file);

                    FileModel fileModel = new FileModel();

                    if (f.Extension.ToLower() != "php" && f.Extension.ToLower() != "aspx"
                        && f.Extension.ToLower() != "asp")
                    {
                        fileModel.FileName = Path.GetFileName(file);
                        fileModel.FileType = Path.GetExtension(file);
                        string filedisplaypath = path;
                        filedisplaypath = filedisplaypath.Replace("search/", "");
                        filedisplaypath = Server.MapPath("~/" + searchauthoremail + "/" + filedisplaypath + "/" + fileModel.FileName);
                        filedisplaypath = filedisplaypath.Replace("\\CodeRepository.UI", "");
                        var fileContents = System.IO.File.ReadAllText(filedisplaypath);

                        fileModel.FileText = fileContents;

                        fileModel.FileAccessed = f.LastAccessTime;
                        fileModel.FileSizeText = (f.Length < 1024) ? f.Length.ToString() + " B" : f.Length / 1024 + " KB";
                        fileModel.FilePath = searchfilename;
                        fileListModel.Add(fileModel);
                    }
                }

                ExplorerModel explorerModel = new ExplorerModel(dirListModel, fileListModel);

                return View(explorerModel);
            }
            else
            {
                return Content(path + " is not a valid file or directory.");
            }
        }

        //This method is used to explore the project folder on clicking the projectname in UserProjects.
        //This iterates through the project folder in the server and displays it as it is
        public ActionResult Index(string path)
        {
            string email = "";
            if (path != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:33803");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    path = path.Replace("_folder", ".zip");
                    string url = "api/User/get/?name=" + "search|o|" + path;
                    try
                    {
                        // This is a get request to get the project Author

                        var response = client.GetAsync(url).Result;
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        IEnumerable<ProjectModel> myObject = JsonConvert.DeserializeObject<IEnumerable<ProjectModel>>(responseString);
                        ProjectModel pr = new ProjectModel();
                        pr = myObject.First();
                        email = pr.UserId;
                        CoAuthor Auth = new CoAuthor();

                        string checkurl = "api/home/get/?name=" + path + "|o|" + email;
                        try
                        {
                            var Authresponse = client.GetAsync(checkurl).Result;
                            string AuthresponseString = Authresponse.Content.ReadAsStringAsync().Result;
                            //var myobject = response.Content.ReadAsAsync<ProjectModel>();
                            Auth = JsonConvert.DeserializeObject<CoAuthor>(AuthresponseString);

                            if (Auth == null)
                            {
                                authoremail = email;
                            }
                            else
                            {
                                authoremail = Auth.Author;
                            }
                        }
                        catch
                        {

                        }
                    }
                    catch
                    {

                    }
                }

            }
            string realPath;
            if (path == null)
            {
                path = realfilename;
            }
            String realfile = path;

            email = authoremail;
            path = path.Replace(".zip", "_folder");
            realPath = Server.MapPath("~/" + email + "/" + path);
            realPath = realPath.Replace("\\CodeRepository.UI", "");
            realPath = realPath.Replace("\\index", "");

            //If path leads to a file open it in new window.
            if (System.IO.File.Exists(realPath))
            {
                FileModel fileModel = new FileModel();

                String filename = Path.GetFileName(path);
                fileModel.FileName = filename;
                fileModel.FileType = Path.GetExtension(filename);
                fileModel.FilePath = realfilename;
                string filedetail = fileModel.FileName + "|o|" + fileModel.FilePath + "|o|" + fileModel.FileSizeText + "|o|" + fileModel.Version;
                return RedirectToAction("FileDetail", "Files", new { filedetail = filedetail });
            }

            //If the path is to a directory iterate through it.
            //If there are unsaved files save them in database.

            else if (System.IO.Directory.Exists(realPath))
            {

                Uri url = Request.Url;
                //Every path needs to end with slash
                if (url.ToString().Last() != '/')
                {
                    realfilename = realfile;

                    Response.Redirect("/Explorer/index/" + path + "/");

                    //Server.Transfer("Index.cshtml");
                }

                List<DirModel> dirListModel = new List<DirModel>();

                IEnumerable<string> dirList = Directory.EnumerateDirectories(realPath);
                foreach (string dir in dirList)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);

                    DirModel dirModel = new DirModel();

                    dirModel.DirName = Path.GetFileName(dir);
                    dirModel.DirAccessed = d.LastAccessTime;

                    dirListModel.Add(dirModel);
                }


                List<FileModel> fileListModel = new List<FileModel>();

                IEnumerable<string> fileList = Directory.EnumerateFiles(realPath);
                foreach (string file in fileList)
                {
                    FileInfo f = new FileInfo(file);

                    FileModel fileModel = new FileModel();

                    if (f.Extension.ToLower() != "php" && f.Extension.ToLower() != "aspx"
                        && f.Extension.ToLower() != "asp")
                    {
                        fileModel.FileName = Path.GetFileName(file);
                        fileModel.FileType = Path.GetExtension(file);
                        string filedisplaypath = path;
                        filedisplaypath = filedisplaypath.Replace("index/", "");
                        filedisplaypath = Server.MapPath("~/" + email + "/" + filedisplaypath + "/" + fileModel.FileName);
                        filedisplaypath = filedisplaypath.Replace("\\CodeRepository.UI", "");
                        var fileContents = System.IO.File.ReadAllText(filedisplaypath);

                        fileModel.FileText = fileContents;

                        fileModel.FileAccessed = f.LastAccessTime;
                        fileModel.FileSizeText = (f.Length < 1024) ? f.Length.ToString() + " B" : f.Length / 1024 + " KB";
                        fileModel.FilePath = realfilename;

                        using (var getclient = new HttpClient())
                        {
                            getclient.BaseAddress = new Uri("http://localhost:33803");
                            getclient.DefaultRequestHeaders.Accept.Clear();
                            getclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            string geturl = "api/projects/filesproject/?projectId=" + realfilename;
                            try
                            {
                                //api call to get files in project
                                var getresponse = getclient.GetAsync(geturl).Result;
                                string responseString = getresponse.Content.ReadAsStringAsync().Result;
                                var myobject = getresponse.Content.ReadAsAsync<ProjectModel>();
                                IEnumerable<FileModel> myObj = JsonConvert.DeserializeObject<IEnumerable<FileModel>>(responseString);
                                bool exists = false;
                                foreach (FileModel p in myObj)
                                {
                                    if (p.FileName == fileModel.FileName)
                                    {
                                        exists = true;
                                    }

                                }
                                if (!exists)
                                {
                                    //if file does not exist make a post call to save the file

                                    var message = new HttpRequestMessage();
                                    var content = new MultipartFormDataContent();
                                    var client = new HttpClient();
                                    client.BaseAddress = new Uri("http://localhost:33803/");
                                    var response = client.PostAsJsonAsync("api/files", fileModel).Result;
                                    if (response.IsSuccessStatusCode)
                                    {
                                        Console.Write("Success");
                                    }
                                    else
                                    {
                                        Console.Write("Error");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return View();
                            }

                        }

                        fileListModel.Add(fileModel);
                    }
                }

                ExplorerModel explorerModel = new ExplorerModel(dirListModel, fileListModel);

                return View(explorerModel);
            }
            else
            {
                return Content(path + " is not a valid file or directory.");
            }
        }
    }
}