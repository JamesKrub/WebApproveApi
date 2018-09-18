using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApproveApi.Models.PersonnelModel;

namespace WebApproveApi.Controllers
{
    public class PersonnelController : ApiController
    {
        [HttpGet]
        [Route("personnel/chkLogin/{username?}/{password?}")]
        public string Get(string username,string password)
        {
            PersonnelService service = new PersonnelService();
            return service.chkLogin(username, password);
        }

        // GET: api/Personnel/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Personnel
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Personnel/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Personnel/5
        public void Delete(int id)
        {
        }
    }
}
