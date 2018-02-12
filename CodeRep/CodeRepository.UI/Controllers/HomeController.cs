using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using CodeRepository.UI.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CodeRep.Entities;

namespace CodeRepository.UI.Controllers
{
    
    public class HomeController : Controller
    {
        public static string authoremail = "";
        public ActionResult Index()
        {
            return View();
        }

        //This method is used to add new user(coauthor) to a uploaded project
        [Authorize]
        public ActionResult AddUser(ProjectModel project)
        {
            ModelState.Clear();
            using (var client = new HttpClient())
            {
                //api call to get project to know the Author of Project
                CoAuthor myObject = new CoAuthor();
                client.BaseAddress = new Uri("http://localhost:33803");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string checkurl = "api/home/get/?name=" + project.ProjectName + "|o|" + AccountController.useremail;
                try
                {
                    var response = client.GetAsync(checkurl).Result;
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    //var myobject = response.Content.ReadAsAsync<ProjectModel>();
                    myObject = JsonConvert.DeserializeObject<CoAuthor>(responseString);
                    authoremail = myObject.Author;
                }
                catch
                {

                }

                //Api get call to get Author details to check who is the main author

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "api/projects/SearchProject/?name=" + project.ProjectName;
                try
                {
                    var response = client.GetAsync(url).Result;
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    var myobject = response.Content.ReadAsAsync<ProjectModel>();
                    IEnumerable<ProjectModel> myObj = JsonConvert.DeserializeObject<IEnumerable<ProjectModel>>(responseString);
                    bool exists = false;
                    foreach (var pr in myObj)
                    {
                        if(pr.UserId == project.UserId)
                        {
                            exists = true;
                        }
                    }
                    if(!exists)
                    {
                        ProjectModel pr = project;
                        string user = pr.UserId;
                        pr = myObj.First();
                        pr.UserId = user;
                        if(authoremail == "")
                        {
                            pr.ProjectDescription = AccountController.useremail;
                        }
                        else
                        {
                            pr.ProjectDescription = authoremail;
                        }                        

                        //api post call to create new CoAuhtor entry in teh CoAuhtor table
                        var result = client.PostAsJsonAsync("api/User", pr).Result;

                        user = project.UserId;
                        project = myObj.First();
                        project.UserId = user;

                        //api post call to create new entry in Project table with the CoAutor as user 

                        result = client.PostAsJsonAsync("api/home", project).Result;
                        if (result.IsSuccessStatusCode)
                        {
                            ModelState.Clear();
                            return View();
                        }
                        else
                        {
                            ModelState.Clear();
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    return View();
                }

            }
            return View();
        }

        // This method is used to search the Repository projects
        [Authorize]
        public ActionResult Search(ProjectModel project)
        {
            ViewBag.Message = "Your application description page.";
            ModelState.Clear();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:33803");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "api/projects/SearchProject/?name=" + project.ProjectName;
                try
                {
                    //api get call to get projects by project name

                    var response = client.GetAsync(url).Result;
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    var myobject = response.Content.ReadAsAsync<ProjectModel>();
                    List<ProjectModel> myObj = JsonConvert.DeserializeObject<List<ProjectModel>>(responseString);
                    ProjectModel pr = new ProjectModel();
                    myObj.Add(pr);
                    ModelState.Clear();
                    return View(myObj);
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    List<ProjectModel> obj = new List<ProjectModel>();
                    ProjectModel pr = new ProjectModel();
                    obj.Add(pr);
                    return View(obj);
                }

            }
            return View();
        }


        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //This method is used to  display User Projects on Home Page.

        [Authorize]
        public ActionResult UserProjects()
        {
            ViewBag.Message = "Your contact page.";
            ModelState.Clear();
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri("http://localhost:33803");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "api/user/get/?name=" + AccountController.useremail;
                try
                {
                    //api get call to get projects uploaded by user

                    var response = client.GetAsync(url).Result;
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    var myobject = response.Content.ReadAsAsync<ProjectModel>();
                    List<ProjectModel> myObj = JsonConvert.DeserializeObject<List<ProjectModel>>(responseString);
                    ProjectModel pr = new ProjectModel();
                    myObj.Add(pr);
                    ModelState.Clear();
                    return View(myObj);
                }
                catch(Exception ex)
                {
                    ModelState.Clear();
                    List<ProjectModel> obj = new List<ProjectModel>();
                    ProjectModel pr = new ProjectModel();
                    obj.Add(pr);
                    return View(obj);
                }
               
            }

            
        }

        // this method is used to download file

        [Authorize]
        public ActionResult DownloadProjects(string  projectname)
        {
            CoAuthor myObject = new CoAuthor();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:33803");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "api/home/get/?name=" + projectname + "|o|" + AccountController.useremail;
                try
                {
                    //api get call to get project by projectname and userid

                    var response = client.GetAsync(url).Result;
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    //var myobject = response.Content.ReadAsAsync<ProjectModel>();
                    myObject = JsonConvert.DeserializeObject<CoAuthor>(responseString);
                    authoremail = myObject.Author;
                }
                catch
                {

                }
            }
            string email = "";
            if(authoremail == "")
            {
                email = AccountController.useremail;
            }
            else
            {
                email = authoremail;
            }

            //Zip the project folder in the server and download

            projectname = projectname.Replace(".zip", "_folder");
            string path = Path.Combine(Server.MapPath("~/"+email ), projectname);
            path = path.Replace("\\CodeRepository.UI", "");
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(path);
                zip.Save(path + projectname + ".zip");
                return File(path+ projectname + ".zip",
                                           "application/zip", projectname + ".zip");
            }
        }

        //This method is used to upload projects to the server

        [Authorize]
        public ActionResult UploadProjects(ProjectModel model)
        {
            ModelState.Clear();

            
            string projectLanguage = model.ProjectLanguage;
            string projectDescription = model.ProjectDescription;
            
            

            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();
            var client = new HttpClient();
           
            HttpPostedFileBase myfile = Request.Files[0];
            if(myfile.FileName == "")
            {
                return RedirectToAction("UserProjects", "Home");
            }
            var fileName = "";
            var path = "";
            if (myfile.ContentLength > 0)
            {
                // extract only the fielname
                fileName = Path.GetFileName(myfile.FileName);
                path = Path.Combine(Server.MapPath("~/"), fileName);
                myfile.SaveAs(path);
            }

            var filestream2 = new FileStream(path, FileMode.Open);
            content.Add(new StreamContent(filestream2), "file", fileName);
            DateTime localDate = DateTime.Now;
            ProjectModel pr = new ProjectModel();
            pr.ProjectName = fileName;
            pr.ProjectLanguage = projectLanguage;
            pr.ProjectDescription = projectDescription;
            pr.CreationDate = localDate;
            pr.UserId = AccountController.useremail;
            client.BaseAddress = new Uri("http://localhost:33803/");

            //api post call  to create new entry in the project table.

            var response = client.PostAsJsonAsync("api/home", pr).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.Write("Success");
            }
            else
            {
                Console.Write("Error");
            }
            System.Threading.Thread.Sleep(1000);
            message.Method = HttpMethod.Post;
            message.Content = content;
            message.RequestUri = new Uri("http://localhost:33803/api/projects/");

            // below processes are to upload zip file to serer and extract it to user folder
            //api call to upload zip file to the server for extraction
            client.PostAsync(message.RequestUri,message.Content)
                .ContinueWith(task3 =>
            {
                
                if (task3.Result.IsSuccessStatusCode)
                {
                    Console.Write("\n  upload successful");
                    var message1 = new HttpRequestMessage();
                    message1.Method = HttpMethod.Options;
                    message1.Content = null;
                    message1.RequestUri = new Uri("http://localhost:33803/api/projects/");
                    System.Threading.Thread.Sleep(1000);

                    //api call to unzip file to the server
                    client.SendAsync(message1)
                        .ContinueWith(t3 =>
                        {
                            if (t3.IsFaulted || t3.IsCanceled)
                            {
                                string ex1 = t3.Exception.InnerException.GetType().Name;
                                string ex2 = t3.Exception.InnerException.Message;
                                Console.WriteLine(t3.Exception.Message);
                                throw new Exception();
                            }
                            //Console.Write("\n  status: {0}", response.ReasonPhrase);
                            else if (t3.Result.IsSuccessStatusCode)
                            {
                                Console.Write("\n  upload successful");
                            }
                        });
                }
            });
            ViewBag.Message = "Your Projects page.";

            ModelState.Clear();
            return RedirectToAction("UserProjects", "Home"); ;



            /*
      string path = @"C:\Users\Darshan\Desktop\test\";
      string name = "";
      string[] files = Directory.GetFiles(path);
      foreach (var file in files)
      {
          var filestream2 = new FileStream(file, FileMode.Open);
          var fileName = System.IO.Path.GetFileName(file);
          name = fileName;
          content.Add(new StreamContent(filestream2), "file", fileName);
          Console.Write("\n    {0}", System.IO.Path.GetFileName(fileName));
      }*/



            //var client = new HttpClient();
            /*byte[] file;
            string fileName = string.Empty;

            fileName = Request.Files[0].FileName;

            using (MemoryStream ms = new MemoryStream())
            {
                Request.Files[0].InputStream.CopyTo(ms);
                content.Add(new StreamContent(ms), "file", fileName);
                file = ms.ToArray();
            }*/
        }
    }
}