using CodeRep.Entities;
using CodeRepository.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CodeRepository.UI.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        
        //This method is used to display the file contents in new window.
        // GET: Files
        [ValidateInput(false)]
        public ActionResult FileDetail(string filedetail)

        {
            FileModel file = new FileModel();
            if(filedetail != null)
            {
                string source = filedetail;
                string[] stringSeparators = new string[] { "|o|" };
                string[] result;

                // ...
                result = source.Split(stringSeparators, StringSplitOptions.None);
                file.FileName = result[0];
                file.FilePath = result[1];
                file.FileSizeText = result[2];
                file.Version = Convert.ToInt32(result[3]);
            }
            // if check in call was made
            ModelState.Clear();
            if (file.FileSizeText== "CheckIn"|| file.FileSizeText == "")
            {
                using (var getclient = new HttpClient())
                {
                    // API CALL TO GET FILES IN PROJECT
                    getclient.BaseAddress = new Uri("http://localhost:33803");
                    getclient.DefaultRequestHeaders.Accept.Clear();
                    getclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string geturl = "api/files/filesbyname/?filename=" + file.FileName + "&project=" + file.FilePath;
                    try
                    {
                        var getresponse = getclient.GetAsync(geturl).Result;
                        var responseString = getresponse.Content.ReadAsStringAsync().Result;
                        IEnumerable<FileModel> myObj = JsonConvert.DeserializeObject<IEnumerable<FileModel>>(responseString);
                        int Ch1Max = int.MinValue;
                        int Ch1Min = int.MaxValue;

                        //check forthe latest version to display.

                        foreach (FileModel s in myObj)
                        {
                            if (s.Version > Ch1Max) Ch1Max = s.Version;
                            if (s.Version < Ch1Min) Ch1Min = s.Version;
                        }
                        foreach (FileModel s in myObj)
                        {
                            if (s.Version == Ch1Max)
                            {
                                ModelState.Clear();
                                file = s;
                                return View(s);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        return View(file);
                    }

                }
            }
            // Show previous version call
            else if(file.FileSizeText == "Show Previous Version")
            {
                using (var getclient = new HttpClient())
                {

                    getclient.BaseAddress = new Uri("http://localhost:33803");
                    getclient.DefaultRequestHeaders.Accept.Clear();
                    getclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string geturl = "api/files/filesbyname/?filename=" + file.FileName + "&project=" + file.FilePath;
                    try
                    {
                        var getresponse = getclient.GetAsync(geturl).Result;
                        var responseString = getresponse.Content.ReadAsStringAsync().Result;
                        IEnumerable<FileModel> myObj = JsonConvert.DeserializeObject<IEnumerable<FileModel>>(responseString);
                        
                        foreach (FileModel s in myObj)
                        {
                            if (s.Version == (file.Version-1))
                            {
                                ModelState.Clear(); 
                                return View(s);
                            }

                        }
                        return View(myObj.First());
                    }
                    catch (Exception ex)
                    {
                        return View(file);
                    }

                }

            }
            // show next version call
            else if (file.FileSizeText == "Show Next Version")
            {
                using (var getclient = new HttpClient())
                {

                    getclient.BaseAddress = new Uri("http://localhost:33803");
                    getclient.DefaultRequestHeaders.Accept.Clear();
                    getclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string geturl = "api/files/filesbyname/?filename=" + file.FileName + "&project=" + file.FilePath;
                    try
                    {
                        var getresponse = getclient.GetAsync(geturl).Result;
                        var responseString = getresponse.Content.ReadAsStringAsync().Result;

                        IEnumerable<FileModel> myObj = JsonConvert.DeserializeObject<IEnumerable<FileModel>>(responseString);

                        foreach (FileModel s in myObj)
                        {
                            file = s;
                            if (s.Version == (file.Version + 1))
                            {
                                 
                                ModelState.Clear();
                                return View(s);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        return View(file);
                    }

                }

            }

            return View(file);
        }

        //This method is used to check in the file

        [ValidateInput(false)]
        public ActionResult CheckInFile(FileModel file,string submit)
        {
            if(submit == "CheckIn")
            {
                ModelState.Clear();
                string mainemail = "";
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:33803");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string path = file.FilePath;
                    path = path.Replace("_folder", ".zip");
                    string url = "api/User/get/?name=" + "search|o|" + path;
                    try
                    {
                        // api call to get projects with the given name
                        var response = client.GetAsync(url).Result;
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        IEnumerable<ProjectModel> myObject = JsonConvert.DeserializeObject<IEnumerable<ProjectModel>>(responseString);
                        foreach(var item in myObject)
                        {
                            //Check if the current user is the Author or CoAuthor of above given project
                            if (item.UserId == AccountController.useremail)
                            {
                                mainemail = item.UserId;
                            }
                        }
                        
                    }
                    catch
                    {
                        
                    }
                }
                //if user is not author or coauthor no checkin is made
                if(mainemail=="")
                {
                    ViewBag.Message = "Not Authorized!";
                    string falsecheckin = file.FileName + "|o|" + file.FilePath + "|o|" + file.FileSizeText + "|o|" + file.Version;
                    return RedirectToAction("FileDetail", "Files", new { filedetail = falsecheckin });

                }
                // if user is author or coauthor checkin is made
                else
                {
                    using (var getclient = new HttpClient())
                    {
                        //api call to upload latest version of file to the DB

                        getclient.BaseAddress = new Uri("http://localhost:33803");
                        getclient.DefaultRequestHeaders.Accept.Clear();
                        getclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string geturl = "api/files/fileupdate";
                        file.CheckedUser = AccountController.useremail;
                        var getresponse = getclient.PutAsJsonAsync(geturl, file).Result;
                        string result = getresponse.Content.ReadAsStringAsync().Result;

                        // Creating a new folder with the new version name in hte server 

                        string folder = file.FilePath;
                        file.FileType = Path.GetExtension(file.FileName);
                        folder = folder.Replace(".zip", "_folder");
                        String path = Server.MapPath("~/" + AccountController.useremail + "/" + folder + "/Version_" + (file.Version + 1) + "_Files");
                        path = path.Replace("\\CodeRepository.UI", "");
                        Directory.CreateDirectory(path);

                        using (FileStream fs = System.IO.File.Create(path + "\\" + file.FileName))
                        {
                            // Add some text to file
                            Byte[] title = new UTF8Encoding(true).GetBytes(file.FileText);
                            fs.Write(title, 0, title.Length);
                        }

                        FilesController f = new FilesController();
                        //f.FileDetail(file, submit);
                        ModelState.Clear();
                        file.FileSizeText = submit;
                        file.FileText = "";
                    }
                }

                
            }
            else if (submit == "Show Previous Version")
            {
                file.FileSizeText = submit;
            }
            else if (submit == "Show Next Version")
            {
                file.FileSizeText = submit;
            }

            //Call file detail to display the latest version of file

            string filedetail = file.FileName + "|o|" + file.FilePath + "|o|" + file.FileSizeText+ "|o|"+file.Version;
            return RedirectToAction("FileDetail", "Files", new { filedetail = filedetail }); 
        }

    }
}