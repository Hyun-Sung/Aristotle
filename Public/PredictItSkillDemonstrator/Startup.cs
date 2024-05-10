using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using PredictItSkillDemonstrator.Configurations;
using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using PredictItSkillDemonstrator.Controllers;
using PredictItSkillDemonstrator.BusinessLayer;
using PredictItSkillDemonstrator.HelperFunctions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Net.Security;

namespace PredictItSkillDemonstrator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //implement retry policy with exp backoff
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            //https://learn.microsoft.com/en-us/entra/identity-platform/scenario-protected-web-api-verification-scope-app-roles?tabs=aspnetcore
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddSingleton(Configuration.GetSection("ApiKeys").Get<ApiKeyConfiguration>());
            services.AddTransient<WeatherForecastController>();
            services.AddTransient<WeatherHelper>();
            services.AddTransient<OpenWeatherApiKeyHelperClass>();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PredictItSkillDemonstrator", Version = "v1" });
            });
            
            services.AddHttpClient("OpenWeatherAPI", c =>
            {
                //c.DefaultRequestHeaders.Clear();
                //c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                c.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");

            }).ConfigurePrimaryHttpMessageHandler((c) =>
            new HttpClientHandler()
            {
                //this allows chrome based browers to run the service despite certificate errors.
                //the idea came from https://stackoverflow.com/questions/2675133/c-sharp-ignore-certificate-errors, user: Ogglas
                //the difference is here I am adding it to the startup file while configuring the client for "OpenWeatherAPI"

                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                    {
                        return true;   //Is valid
                    }

                    // this hash string can be found by clicking on the certificate in chrome -> details -> thumbprint 
                    // then running cert.GetCertHashString().ToLower() on the value
                    if (cert.GetCertHashString() == "99E92D8447AEF30483B1D7527812C9B7B3A915A7")
                    {
                        return true;
                    }
                    return false;
                }
        }).AddPolicyHandler(GetRetryPolicy());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PredictItSkillDemonstrator v1"));

                // Custom authentication blueprint taken found from
                //https://stackoverflow.com/questions/53234135/programmatically-add-allowanonymous-attribute-to-all-my-controller-methods/53242694#53242694
                //user4864425
                app.Use(async (context, next) =>
                {
                    // Set claims for the test user.
                    var claims = new[] { new Claim("scp", "access_as_user") };
                    var id = new ClaimsIdentity(claims, "DebugAuthorizationMiddleware", "name", "role");
                    // Add the test user as Identity.
                    context.User.AddIdentity(id);
                    // User is now authenticated.
                    await next.Invoke();
                });
            }
            else
            {
                // use configured authentication
                app.UseAuthentication();
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
