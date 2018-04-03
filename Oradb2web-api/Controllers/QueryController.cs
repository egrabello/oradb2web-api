using DataAccess;
using Oradb2web_api.Models;
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
        public async Task<HttpResponseMessage> Post([FromBody]Query query)
        {
            try
            {
                Resultado objetoConsulta = new Resultado();
               
                if (String.IsNullOrEmpty(query.SQL))
                {
                    objetoConsulta.erro.codigo = "0";
                    objetoConsulta.erro.mensagem = "A query não pode estar vazia";
                    return Request.CreateResponse(HttpStatusCode.OK, objetoConsulta);
                }

                QueriesHandler qh = new QueriesHandler();
                var resultadoConsulta = await qh.Query(query.SQL);
                objetoConsulta.resultado = resultadoConsulta;
                return Request.CreateResponse(HttpStatusCode.OK, objetoConsulta);
                
            }
            catch (Exception ex)
            {
                Resultado objetoConsulta = new Resultado();
                objetoConsulta.erro.mensagem = ex.Message;
                var arrayErro = ex.Message.Split(':');
                if (arrayErro.Length > 1)
                {
                    objetoConsulta.erro.codigo = ex.Message.Split(':')[0];
                    objetoConsulta.erro.mensagem = ex.Message.Split(':')[1].Trim();
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, objetoConsulta);
               
            }

        }
    }
}
