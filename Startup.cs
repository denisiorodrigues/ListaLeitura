using Alura.ListaLeitura.App.Logica;
using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void ConfigureProduction(IApplicationBuilder app)
        {
            var routerBuilder = new RouteBuilder(app);
            routerBuilder.MapRoute("livros/paraler", LivrosLogica.LivrosParaLer);
            routerBuilder.MapRoute("livros/lendo", LivrosLogica.LivrosLendo);
            routerBuilder.MapRoute("livros/lidos", LivrosLogica.LivrosLidos);
            routerBuilder.MapRoute("cadastro/NovoLivro/{nome}/{autor}", CadastroLogica.CadastroNovoLivro);
            routerBuilder.MapRoute("Livros/Detalhes/{id:int}", CadastroLogica.ExibirDetalhes);
            routerBuilder.MapRoute("Cadastro/NovoLivro", CadastroLogica.ExibeFormulario);
            routerBuilder.MapRoute("Cadastro/Incluir", CadastroLogica.ProcessaFormulario);

            var rotas = routerBuilder.Build();
            app.UseRouter(rotas);

            //app.Run(Roteamento);
        }
    }
}