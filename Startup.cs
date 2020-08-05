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
            routerBuilder.MapRoute("livros/paraler", LivrosParaLer);
            routerBuilder.MapRoute("livros/lendo", LivrosLendo);
            routerBuilder.MapRoute("livros/lidos", LivrosLidos);
            routerBuilder.MapRoute("cadastro/NovoLivro/{nome}/{autor}", CadastroNovoLivro);
            routerBuilder.MapRoute("Livros/Detalhes/{id:int}", ExibirDetalhes);
            routerBuilder.MapRoute("Cadastro/NovoLivro", ExibeFormulario);
            routerBuilder.MapRoute("Cadastro/Incluir", ProcessaFormulario);

            var rotas = routerBuilder.Build();
            app.UseRouter(rotas);

            //app.Run(Roteamento);
        }

        private string CarregaArquivoHTML(string nomeArquivo)
        {
            string nomeCompletoArquivo = $"HTML/{nomeArquivo}.html";
            using (var arquivo = File.OpenText(nomeCompletoArquivo))
            {
                return arquivo.ReadToEnd();
            }
        }

        private Task ProcessaFormulario(HttpContext context)
        {
            Livro livro = new Livro()
            {
                Titulo = context.Request.Form["titulo"].First(),
                Autor = context.Request.Form["autor"].First(),
            };

            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("Livro adicionado com sucesso!");
        }

        private Task ExibeFormulario(HttpContext context)
        {
            var html = CarregaArquivoHTML("formulario");
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
            var conteudoArquivo = CarregaArquivoHTML("para-ler");

            foreach (var item in _repo.ParaLer.Livros)
            {
                conteudoArquivo = conteudoArquivo.Replace("#NOVO-ITEM#", $"<li>{item.Titulo} - {item.Autor}</li>#NOVO-ITEM#");
            }

            conteudoArquivo = conteudoArquivo.Replace("#NOVO-ITEM#", "");
            return context.Response.WriteAsync(conteudoArquivo);
        }

        public Task LivrosLendo(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var conteudoArquivo = CarregaArquivoHTML("lendo");

            foreach (var item in _repo.Lendo.Livros)
            {
                conteudoArquivo = conteudoArquivo.Replace("#NOVO-ITEM#", $"<li>{item.Titulo} - {item.Autor}</li>#NOVO-ITEM#");
            }

            conteudoArquivo = conteudoArquivo.Replace("#NOVO-ITEM#", "");
            return context.Response.WriteAsync(conteudoArquivo);
        }

        public Task LivrosLidos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            var conteudoArquivo = CarregaArquivoHTML("lidos");

            foreach (var item in _repo.Lidos.Livros)
            {
                conteudoArquivo = conteudoArquivo.Replace("#NOVO-ITEM#", $"<li>{item.Titulo} - {item.Autor}</li>#NOVO-ITEM#");
            }

            conteudoArquivo = conteudoArquivo.Replace("#NOVO-ITEM#", "");
            return context.Response.WriteAsync(conteudoArquivo);
        }
    }
}