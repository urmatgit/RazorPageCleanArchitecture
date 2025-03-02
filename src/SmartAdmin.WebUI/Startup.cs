using CleanArchitecture.Razor.Application;
using CleanArchitecture.Razor.Infrastructure;
using CleanArchitecture.Razor.Infrastructure.Constants.Application;
using CleanArchitecture.Razor.Infrastructure.Constants.Localization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using SmartAdmin.WebUI.Extensions;
using SmartAdmin.WebUI.Hubs;
using SmartAdmin.WebUI.Models;
using System.IO;
using System.Linq;

namespace SmartAdmin.WebUI
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
            services.Configure<SmartSettings>(Configuration.GetSection(SmartSettings.SectionName));

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            // Note: This line is for demonstration purposes only, I would not recommend using this as a shorthand approach for accessing settings
            // While having to type '.Value' everywhere is driving me nuts (>_<), using this method means reloaded appSettings.json from disk will not work
            services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        
            services.AddApplication()
                    .AddInfrastructure(Configuration)
                    .AddWorkflow(Configuration);
          
            services.AddDatabaseDeveloperPageExceptionFilter();
           
            services.AddControllers();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x=>x.Code).ToArray());
                options.FallBackToParentUICultures = true;
            });
            services
                .AddRazorPages()
                .AddFluentValidation(fv =>
                {
                    fv.DisableDataAnnotationsValidation = true;
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.ImplicitlyValidateRootCollectionElements = true;
                })
                .AddViewLocalization()
                .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.PropertyNamingPolicy = null;
                      
                    })
                .AddRazorRuntimeCompilation()
                .AddRazorPagesOptions(options =>
                {

                    options.Conventions.AddPageRoute("/AspNetCore/Welcome", "");
                });

            services.ConfigureApplicationCookie(options => {
                        options.LoginPath = "/Identity/Account/Login";
                        options.LogoutPath = "/Identity/Account/Logout";
                        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                  });

            services.AddScoped<RequestLocalizationCookiesMiddleware>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
                RequestPath = new PathString("/Files")
            });
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
                    diagnosticContext.Set("UserName", httpContext.User?.Identity?.Name??string.Empty);
                };
            });
            app.UseRequestLocalization();
            app.UseRequestLocalizationCookies();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWorkflow();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<SignalRHub>(ApplicationConstants.SignalR.HubUrl);
            });
        }
    }
}
