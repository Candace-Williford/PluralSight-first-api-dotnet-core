using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace CityInfo.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(
                    options => {
                        options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                        options.InputFormatters.Add(new XmlSerializerInputFormatter());
                    }
                );
                // this code allows you to override the serialization options of .NET Core
                // .AddJsonOptions(o => {
                //     if (o.SerializerSettings.ContractResolver != null)
                //     {
                //         var castedResolver = o.SerializerSettings.ContractResolver
                //             as DefaultContractResolver;
                //         castedResolver.NamingStrategy = null; //this part specifically overrides the bahavior that properties on a JSON object are serialized to start with lowercase instead of uppercase letters
                //     }
                // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseExceptionHandler();

            app.UseStatusCodePages();
            app.UseMvc();

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }
    }
}
