using AutoMapper;
using back_end.Controllers;
using back_end.Filtros;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace back_end
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
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(provider =>
                new MapperConfiguration(config =>
               {
                   var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                   config.AddProfile(new AutoMapperProfiles(geometryFactory));
               }).CreateMapper()
            );

            // Además de registrar el servicio NetTopologySuite, hay que registrar un servicio llamado, que nos va a permitir
            // trabajar con distancias con C# utilizando el NetTopologySuite
            // srid: 4326 es el valor utilizado para hacer mediciones en el planeta Tierra.
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));


            // services.AddTransient<IAlmacenadorArchivos, AlmacenadorAzureStorage>(); //  Para Azure  
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>(); // Para guardar imagen localmente 

            services.AddHttpContextAccessor();


            services.AddDbContext<AplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"),
                sqlServer => sqlServer.UseNetTopologySuite() // -> para activar los queries espaciales con EF Core.
            ));




            services.AddCors(options =>   // AddCors se utiliza solo para navegadores web (no para Android o iOS)
            {
                var frontendURL = Configuration.GetValue<string>("frontend_url");
                options.AddDefaultPolicy(builder =>
                {
                    // Es *importante* no colocar '/' al final de la cadena URL, o de lo contrario no funcionará.
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
                        .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddControllers(options =>
            {
                // Filtro de excepción registrado a nivel global de nuestra aplicación.
                // No importa dónde ocurra el error, que éste será capturado por medio de este filtro personalizado.
                options.Filters.Add(typeof(FiltroDeExcepcion)); 
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "back_end", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Esta es nuestra tubería de procesos (HTTP pipeline) que contiene uno o más middlewares
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {  
                app.UseDeveloperExceptionPage(); 
                app.UseSwagger(); 
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "back_end v1")); 
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(); // Middleware que nos permite servir archivos estáticos

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>  
            {
                endpoints.MapControllers(); 
            });
        }
    }
}
