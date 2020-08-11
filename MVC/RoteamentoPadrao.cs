using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App.MVC
{
    public class RoteamentoPadrao
    {
        public static Task TratamentoPadrao(HttpContext context)
        {
            var classe = Convert.ToString(context.GetRouteValue("classe"));
            var nomeMetodo = Convert.ToString(context.GetRouteValue("metodo"));

            var nomeCompleto = $"Alura.ListaLeitura.Logica.{classe}Logica";

            var tipo = Type.GetType(nomeCompleto);
            var metodo = tipo.GetMethods().Where(x => x.Name == nomeMetodo).FirstOrDefault();
            var requestDelegate = (RequestDelegate)Delegate.CreateDelegate(typeof(RequestDelegate), metodo);

            return requestDelegate.Invoke(context);
        }
    }
}
