using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRepository.UI.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        public ActionResult ProjectDetail()
        {
            return View();
        }
        
    }
}