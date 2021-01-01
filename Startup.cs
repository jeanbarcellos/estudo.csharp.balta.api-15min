using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using testeef.Data;

namespace testeef
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo tempo de execução. Use este método para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registra o contexto fornecido como um serviço no IServiceCollection
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));

            // Adiciona um serviço com escopo do tipo especificado em DataContext com um tipo de implementação especificado em DataContext para o IServiceCollection especificado.
            services.AddScoped<DataContext, DataContext>();

            // Adiciona serviços para controladores ao IServiceCollection especificado.
            // Este método não registrará serviços usados para visualizações ou páginas.
            services.AddControllers();

            // Registro dos Services no Container
            services.AddScoped<SeedingService>();
        }

        // Este método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            SeedingService seedingService
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
