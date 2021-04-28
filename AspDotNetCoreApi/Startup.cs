using AspDotNetCoreApi.Extensions.Configuration;
using AspDotNetCoreApi.Helpers;
using AspDotNetCoreApi.Helpers.Implementations;
using AspDotNetCoreApi.Orchestrator;
using AspDotNetCoreApi.Orchestrator.Implementations;
using AspDotNetCoreApi.Repositories;
using AspDotNetCoreApi.Repositories.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AspDotNetCoreApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors(Configuration)
                .ConfigureSwagger()
                .ConfigureVersioning()
                .ConfigureSetting(Configuration)
                .ConfigureAuthentication(Configuration);

            // Service injections
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDapperHelper, DapperHelper>();
            services.AddHttpClient();

            services.AddScoped<IAdventureWorksRepository, AdventureWorksRepository>();
            services.AddScoped<IAdventureWorksOrchestrator, AdventureWorksOrchestrator>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.NET Core API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspDotNetCoreApi v1"));
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
