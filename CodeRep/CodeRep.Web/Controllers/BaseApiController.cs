using CodeRepository.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeRepository.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        private IRepositoryInterface _repo;
        private ModelFactory _modelFactory;

        public BaseApiController(IRepositoryInterface repo)
        {
            _repo = repo;
        }

        protected IRepositoryInterface TheRepository
        {
            get
            {
                return _repo;
            }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(Request,TheRepository);
                }
                return _modelFactory;
            }
        }
    }
}
