using CodeRepository;
using CodeRepository.Entities;
using CodeRepository.Web.Models;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;
using static CodeRep.Web.Repository;

namespace CodeRep.Web
{
    /// <summary>
    /// Summary description for Repository
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Repository : System.Web.Services.WebService
    {
        //private IRepositoryInterface _repo;
        //private ModelFactory _modelFactory = new ModelFactory();
        //private RepositoryContext _ctx;

        //public Repository(IRepositoryInterface repo, RepositoryContext ctx)
        //{
        //    _repo = repo;
        //    _ctx = ctx;
        //}

        //protected IRepositoryInterface TheRepository
        //{
        //    get
        //    {
        //        return _repo;
        //    }
        //}
        [DataContract(IsReference = true)]
        public class ProjectValues
        {
            [DataMember]
            public int ProjectId { get; set; }
            [DataMember]
            public string Url { get; set; }
            [DataMember]
            public string ProjectName { get; set; }
            [DataMember]
            public string ProjectLanguage { get; set; }
            [DataMember]
            public string ProjectDescription { get; set; }
            [DataMember]
            public DateTime? CreationDate { get; set; }
            [DataMember]
            public DateTime? LastModifiedDate { get; set; }
            [DataMember]
            public string UserId { get; set; }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<Project> GetAllProjects()
        {
            //RepositoryContext _ctx = new RepositoryContext();
            //return _ctx.Project.ToList(); 
            using (var ctx = new RepositoryContext())
                return ctx.Project.ToList();
        }

        [WebMethod]
        public int converttodaysweb(int day, int month, int year)
        {
            DateTime dt = new DateTime(year, month, day);
            int datetodays = DateTime.Now.Subtract(dt).Days;
            return datetodays;
        }

        [WebMethod]
        public List<File> GetAllFiles()
        {
            //RepositoryContext _ctx = new RepositoryContext();
            //return _ctx.Project.ToList(); 
            using (var ctx = new RepositoryContext())
                return ctx.File.ToList();
        }
    }
}
