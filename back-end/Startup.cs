using back_end.Controllers;
using back_end.Repositorios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
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
            // Existen tres tipos de vida o ciclos de vida que puede tener un servicio:
            //   -> (Add)Transient, es el tiempo m�s corte de vida que le podemos dar a un servicio, y significa que cada vez que pidamos, por ejemplo,
            //            una instancia del servicio de 'IRepositorio', vamos a tener una nueva estancia de este RepositorioEnMemoria>() (una instancia distinta).
            //            Siempre retorna una nueva instancia.  
            //   -< (Add)Scope, el tiempo de vida de la clase instanciada va a ser durante toda la petici�n HTTP. ES decir, que si  distintas clases solicitan
            //            el mismo servicio, y �sto lo hacen dentro del mismo contexto HTTP, se les vsa a servir la misma instancia
            //   -> (Add)Singleton, el cual sirve para indicar que el tiempo de vida de la instancia RepositorioEnMemoria>() del servicio, v a ser
            //            durante todo el t. de ejecuci�n de la aplicaci�n, lo que quiere decir que distintos clientes van a compartir la misma instancia de la
            //            clase RepositorioEnMemoria>(). Siempre devolver� el mismo valor (guid, etc...). Si *no* se quiere compartir instancias entre distintos 
            //            usuarios, *NO* se debe utilizar Singleton.
            //            Con Singleton definido como Service, si por ejemplo a�andimos (POST) al un nuevo registro al ojeto g�nero, al
            //            recuperar la lista con GET, veremos los g�neros que ten�amos m�s el o los que vayamos a�adiendo; estamos compartiendo
            //            la instancia del repositorio en memoria y por eso los cambios o inserviones que se hagan con POST se ver�n reflejados con GET.
            //            Este comportamiento ser�a el m�s parecido al de una base de datos en la vida real.
            



            // services.AddTransient<IRepositorio, RepositorioEnMemoria>();  // >> Transient. Inyecci�n de dependencias.
            services.AddSingleton<IRepositorio, RepositorioEnMemoria>();  // >> Singleton. Inyecci�n de dependencias.
            // services.AddScoped<IRepositorio, RepositorioEnMemoria>();  // >> Singleton. Inyecci�n de dependencias.
            services.AddScoped<WeatherForecastController>(); // --> Modo de inyectar una clase que no tiene Inteface

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "back_end", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "back_end v1"));
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
