using DataAccess;
using Oradb2web_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Http;

namespace Oradb2web_api.Controllers
{
   
    public class QueryController : ApiController
    {
        // POST api/<controller>
        public async Task<HttpResponseMessage> Post([FromBody]QueryInfo query)
        {
            try
            {
                 
                Resultado objetoConsulta = new Resultado();
                objetoConsulta.ip = new System.Net.WebClient().DownloadString("https://api.ipify.org");
                //GetIPAddresss().ToString();

                if (String.IsNullOrEmpty(query.SQL))
                {
                    objetoConsulta.erro.codigo = "0";
                    objetoConsulta.erro.mensagem = "A query não pode estar vazia";
                    
                    return Request.CreateResponse(HttpStatusCode.OK, objetoConsulta);
                }

                QueriesHandler qh = new QueriesHandler();
                var resultadoConsulta = await qh.Query(query.SQL, query?.ConnectionName);
                objetoConsulta.resultado = resultadoConsulta;
                return Request.CreateResponse(HttpStatusCode.OK, objetoConsulta);
                
            }
            catch (Exception ex)
            {
                Resultado objetoConsulta = new Resultado();
                objetoConsulta.ip = new System.Net.WebClient().DownloadString("https://api.ipify.org");
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

        public static IPAddress[] GetIPAddresses()
        {
            IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
            return ipv4Addresses;
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            //return ipHostInfo.AddressList;
        }

        public static IPAddress GetIPAddresss()
        {
            IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
            return ipv4Addresses[0];
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            //return ipHostInfo.AddressList;
        }
        //public static IPAddress GetIPAddress(int num = 0)
        //{
        //    return GetIPAddresses()[num];
        //}

        public static List<string> GetStringIPAddresses()
        {
            List<string> ips = new List<string>();
            IPAddress[] adresses = GetIPAddresses();
            for (int i = 0; i < adresses.Length; i++)
            {
                ips.Add(adresses[i].ToString());
            }

            return ips;
        }
    }
}
