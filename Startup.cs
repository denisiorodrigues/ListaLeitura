using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            routerBuilder.MapRoute("livros/paraler", LivrosParaLer);
            routerBuilder.MapRoute("livros/lendo", LivrosLendo);
            routerBuilder.MapRoute("livros/lidos", LivrosLidos);
            routerBuilder.MapRoute("cadastro/NovoLivro/{nome}/{autor}", CadastroNovoLivro);
            routerBuilder.MapRoute("Livros/Detalhes/{id:int}", ExibirDetalhes);
            routerBuilder.MapRoute("Cadastro/NovoLivro", ExibeFormulario);

            var rotas = routerBuilder.Build();
            app.UseRouter(rotas);

            //app.Run(Roteamento);
        }

        private Task ExibeFormulario(HttpContext context)
        {
            var html = @"<form>
                            <input />
                            <input />
                            <button>Gravar</button>
                        </form>";
            return context.Response.WriteAsync(html);
        }

        private Task ExibirDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            var repo = new LivroRepositorioCSV();
            Livro livro = repo.Todos.First(x => x.Id == id);

            return context.Response.WriteAsync(livro.Detalhes());
        }

        private Task CadastroNovoLivro(HttpContext context)
        {
            Livro livro = new Livro()
            {
                Titulo = context.GetRouteValue("nome").ToString(),
                Autor = context.GetRouteValue("autor").ToString(),
            };

            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("Livro adicionado com sucesso!");
        }

        public Task Roteamento(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var caminhosAtendidos = new Dictionary<string, RequestDelegate>()
            {
                { "/Livros/ParaLer",LivrosParaLer },
                { "/Livros/Lendo", LivrosLendo },
                { "/Livros/Lidos", LivrosLidos }
            };

            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {
                var metodo = caminhosAtendidos[context.Request.Path];
                return metodo.Invoke(context);
            }

            context.Response.StatusCode = 404;
            return context.Response.WriteAsync("Caminho inexistente.");
        }

        public Task LivrosParaLer(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }

        public Task LivrosLendo(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }

        public Task LivrosLidos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lidos.ToString());
        }
    }
}