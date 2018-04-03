using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oradb2web_api.Models
{
    public class Resultado
    {
        public List<Dictionary<string, string>> resultado { get; set; }
        public Erro erro;

        public Resultado()
        {
            resultado = new List<Dictionary<string, string>>();
            erro = new Erro();
        }
    }

    public class Erro
    {
        public string codigo { get; set; }
        public string mensagem { get; set; }
    }
}