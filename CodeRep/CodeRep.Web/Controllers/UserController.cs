using CodeRepository;
using CodeRepository.Entities;
using CodeRepository.Web.Controllers;
using CodeRepository.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeRep.Web.Controllers
{
    public class UserController : BaseApiController
    {
        public static string email;
        public UserController(IRepositoryInterface repo)
            : base(repo)
        {
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post([FromBody] ProjectModel projectModel)
        {

            try
            {
                ModelFactory mf = new ModelFactory();
                var entity = mf.ParseCoAuthor(projectModel);
                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read subject/tutor from body");

                if (TheRepository.InsertCoAuthor(entity) && TheRepository.SaveAll())
                {
                    var newentity = mf.ParseProject(projectModel);
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(newentity));
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



        [System.Web.Http.HttpGet]
        public IEnumerable<ProjectModel> Get(string name)
        {
            if(name.Contains("|o|"))
            {
                try
                {
                    string source = name;
                    string[] stringSeparators = new string[] { "|o|" };
                    string[] result;

                    // ...
                    result = source.Split(stringSeparators, StringSplitOptions.None);

                    IQueryable<Project> query;

                    query = TheRepository.GetProjectbyname(result[1]);
                    var results = query
                    .ToList()
                    .Select(s => TheModelFactory.Create(s));
                    if (results != null)
                    {
                        return results;
                    }
                    else
                    {
                        return results;
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                email = name;
                try
                {
                    IQueryable<Project> query;

                    query = TheRepository.GetProject(name);
                    var results = query
                    .ToList()
                    .Select(s => TheModelFactory.Create(s));
                    if (results != null)
                    {
                        return results;
                    }
                    else
                    {
                        return results;
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            
        }
    }
}
