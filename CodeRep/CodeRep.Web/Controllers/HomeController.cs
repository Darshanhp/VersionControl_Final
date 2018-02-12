using CodeRep.Entities;
using CodeRepository;
using CodeRepository.Web.Controllers;
using CodeRepository.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace CodeRep.Web.Controllers
{
    public class HomeController : BaseApiController
    {
        public static string email;
        public HomeController(IRepositoryInterface repo)
            : base(repo)
        {
        }
        //public ActionResult Index()
        //{
        //    ViewBag.Title = "Home Page";

        //    return View();
        //}
        [System.Web.Http.HttpGet]
        public CoAuthor Get(string name)
        {
            try
            {
                if(name.Contains("|o|"))
                {
                    string source = name;
                    string[] stringSeparators = new string[] { "|o|" };
                    string[] result;

                    // ...
                    result = source.Split(stringSeparators, StringSplitOptions.None);
                    ProjectModel pr = new ProjectModel();
                    pr.ProjectName = result[0];
                    pr.UserId = result[1];

                    CoAuthor author;

                    author = TheRepository.GetAuthor(pr.UserId, pr.ProjectName);
                    return author;
                }
                else
                {
                    CoAuthor author;
                    author = TheRepository.GetAuthor(name);
                    return author;
                }
                

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post([FromBody] ProjectModel projectModel)
        {

            try
            {
                ModelFactory mf = new ModelFactory();
                var entity = mf.ParseProject(projectModel);
                email = projectModel.UserId;
                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read from body");

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
        }

    }
}
