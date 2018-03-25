using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LearningAspCoreWeb.Controllers;
using LearningAspCoreWeb.Middleware;
using LearningAspCoreWeb.Services;

namespace LearningAspCoreWeb
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json");
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            Configuration = builder.Build();
        }

        /// <summary>
        /// Configures the services.
        /// This method gets called by the runtime.
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHomeService, HomeService>();
            services.AddTransient<HomeController>();
            services.AddDistributedMemoryCache();
            services.AddSession(options => options.IdleTimeout = System.TimeSpan.FromMinutes(10));
        }

        /// <summary>
        /// Configures the specified application.
        /// This method gets called by the runtime.
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            app.UseSession();
            app.UseStaticFiles();
            app.UseHeaderMiddleware();
            //app.UseHeadingOneMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/session", sessionApp =>
            {
                sessionApp.Run(async context =>
                {
                    await Session.SessionAsync(context);
                });
            });

            app.Map("/home2", homeApp =>
            {
                homeApp.Run(async context =>
                {
                    HomeController homeController = app.ApplicationServices.GetService<HomeController>();
                    int statusCode = await homeController.Index(context);
                    context.Response.StatusCode = statusCode;
                });
            });

            PathString remaining;
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/configuration", out remaining),
                configApp =>
                {
                    configApp.Run(async context =>
                    {
                        if (remaining.StartsWithSegments("/appsettings"))
                        {
                            await Config.AppSettings(context, Configuration);
                        }
                        else if (remaining.StartsWithSegments("/database"))
                        {
                            await Config.ReadDatabaseConnection(context, Configuration);
                        }
                        else if (remaining.StartsWithSegments("/secret"))
                        {
                            await Config.ReadSecret(context, Configuration);
                        }

                        HomeController homeController = app.ApplicationServices.GetService<HomeController>();
                        int statusCode = await homeController.Index(context);
                        context.Response.StatusCode = statusCode;
                        // return;
                    });
                });

            app.Run(async context =>
            {
                if (context.Request.Path.Value.ToLower() == "/home")
                {
                    HomeController homeController = app.ApplicationServices.GetService<HomeController>();
                    int statusCode = await homeController.Index(context);
                    context.Response.StatusCode = statusCode;
                    return;
                }

                string result = string.Empty;
                switch (context.Request.Path.Value.ToLower())
                {
                    case "/head":
                        result = RequestAndResponse.GetHeaderInformation(context.Request);
                        break;
                    case "/add":
                        result = RequestAndResponse.QueryString(context.Request);
                        break;
                    case "/content":
                        result = RequestAndResponse.Content(context.Request);
                        break;
                    case "/encode":
                        result = RequestAndResponse.ContentEncoded(context.Request);
                        break;
                    case "/form":
                        result = RequestAndResponse.GetForm(context.Request);
                        break;
                    case "/write":
                        result = RequestAndResponse.WriteCookie(context.Response);
                        break;
                    case "/read":
                        result = RequestAndResponse.ReadCookie(context.Request);
                        break;
                    case "/json":
                        result = RequestAndResponse.GetJson(context.Response);
                        break;
                    default:
                        result = RequestAndResponse.GetRequestInformation(context.Request);
                        break;
                }
                await context.Response.WriteAsync(result);
            });
        }
    }
}
