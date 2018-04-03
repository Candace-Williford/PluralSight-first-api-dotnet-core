using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CityInfo.API
{
    public class Startup
    {
        //this is how you would implement configuration in .NET Core 2. All code seen in the implementation for 1
        //is actually handled by the default implementation of an IConfiguration in 2
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //this is how you would implement configuration in .NET Core 1
        // public static IConfigurationRoot Configuration;

        // public Startup(IHostingEnvironment environment)
        // {
        //     var builder = new ConfigurationBuilder()
        //         .SetBasePath(environment.ContentRootPath)
        //         .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true);

        //     Configuration = builder.Build();
        // }

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
            services.AddTransient<IMailService, LocalMailService>();

            //transient is created each time a service is requested and is best for lightweight, stateless services
            //scoped are created once per request
            //singletons the first time they are requested

            var connectionString = "wouldn't you like to know";
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString)); //scoped
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog("nlog.config"); //Logger wasn't creating log file on Ubuntu until I added this. See issue https://github.com/NLog/NLog.Extensions.Logging/issues/109
            loggerFactory.AddConsole(); //logs to the console window
            loggerFactory.AddDebug();
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            loggerFactory.AddNLog(); //"shortcut" for the line above
            
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
