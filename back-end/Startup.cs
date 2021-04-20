using back_end.Controllers;
using back_end.Filtros;
using back_end.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

            services.AddResponseCaching(); // Activar el cach� en nuestra aplicaci�n.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();


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
            services.AddTransient<MiFiltroDeAccion>(); // -->Inyecci�n de dependencias.


            services.AddControllers(options =>
            {
                // Filtro de excepci�n registrado a nivel global de nuestra aplicaci�n.
                // No importa d�nde ocurra el error, que �ste ser� capturado por medio de este filtro personalizado.
                options.Filters.Add(typeof(FiltroDeExcepcion)); 
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "back_end", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Esta es nuestra tuber�a de procesos (HTTP pipeline)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Cada uno los m�todos de app. es un middleware. El orden en que aparecen es  importante, puesto que un middleware env�a info al siguiente
            //  (es decir, son procesos encadenados) 
            // Los middleware que empiezan por 'Use...', no detienen el proceso


            // >>> INI. A modo de ejemplo, vamos a guardar en un Log todas las peticiones HTTP realizadas por los clientes: 
            //          Es decir, estamos aqu� utilizando nuestro propio middleware para mostrar en la consola todas las respuestas HTTP de nuestra aplicaci�n.
            app.Use(async (context, next) =>
            {
                using (var swapStream = new MemoryStream())
                {
                    var respuestaOriginal = context.Response.Body;
                    context.Response.Body = swapStream;

                    await next.Invoke();

                    swapStream.Seek(0, SeekOrigin.Begin);
                    string respuesta = new StreamReader(swapStream).ReadToEnd();
                    swapStream.Seek(0, SeekOrigin.Begin);

                    await swapStream.CopyToAsync(respuestaOriginal);
                    context.Response.Body = respuestaOriginal;

                    logger.LogInformation(respuesta);
                }
            });
            // <<< FIN. A modo de ejemplo, vamos a guardar en un Log todas las peticiones HTTP realizadas por los clientes: 

            app.Map("/mapa1", (app) => // Con esto estamos utizando branching; ejecutar el middleware si el usuario accede a una URL o endpoint espec�fico (p.e.)
            {
                // Si entra aqu�, en este endpoint: (https://localhost:44315/mapa1), se est� interceptando el pipilene (o tuber�a de procesos)
                app.Run(async context =>
                {
                    
                    await context.Response.WriteAsync("Estoy interceptando el pipeline");
                    // Una vez ejecutado este middleware, se termina el pipeline(con ello el programa) y los siguientes middleware no son ejecutados.
                });
            });


            if (env.IsDevelopment())
            {  // Si estamosen desarrollo, utilizamos estos tres middleware en nuestra tuber�a de procesos.
                app.UseDeveloperExceptionPage();  // -> Este es un middleware
                app.UseSwagger(); // -> Este es otro middleware
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "back_end v1")); // -> Este es otro middleware
            }

            app.UseHttpsRedirection(); // -> Este es otro middleware

            app.UseRouting();  // -> Este es otro middleware

            app.UseResponseCaching(); // -> Este es otro middleware

            app.UseAuthentication();

            app.UseAuthorization();  // -> Este es otro middleware; si np pasa este middleware, no se procesa el siguiente.

            app.UseEndpoints(endpoints =>  // -> Este es otro middleware
            {
                endpoints.MapControllers(); 
            });
        }
    }
}
