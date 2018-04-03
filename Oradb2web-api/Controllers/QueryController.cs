using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Oradb2web_api.Controllers
{
   
    public class QueryController : ApiController
    {
        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]String query)
        {
            try
            {
                    return Request.CreateResponse(HttpStatusCode.OK, "Deu certo");
                
            }
            catch (Exception ex)
            {

                HttpError err = new HttpError(ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }

        }
    }
}
