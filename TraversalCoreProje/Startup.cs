using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Container;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TraversalCoreProje.Models;

namespace TraversalCoreProje
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
            // olu�acak log lar� Output ekran�nda g�sterme ve bir klas�rde metin belgesinde g�nderme
            services.AddLogging(x =>
            {
                x.ClearProviders();
                x.SetMinimumLevel(LogLevel.Debug); // LogLevel. -> komutlar� var ( debug - trace - �nformation - error - warning )
                x.AddDebug(); // output �zerinde g�r�nt�ler
            });



            // context i eklemek ve Identitye AppUser ve AppRole s�n�flar�m�z� eklemek i�in yapt�k
            // ayr�ca Identity Validasyonlar�nda de�i�iklik yapt���m�z CustomIdentityValidator modelini ekledik
            // _logger.logInformation("buraya info gir"); , _logger.logError("buraya Error gir"); // bunu controllerda dependincy ILogger<HomeController> ile ekleyerek kullan
            // --------------
            services.AddDbContext<Context>();
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Context>()
                .AddErrorDescriber<CustomIdentityValidator>().AddEntityFrameworkStores<Context>();
            // dosyaya yazma k�sm� i�in paket y�kl�yoruz -Serilog.Extensions.Logging.File -
            // --------------


            // new ' leyerek manager kullanmay� b�rakmak i�in
            // --------------
            // business layer da olu�sturdu�umuz yere att�k kalabal�k olmas�n diye
            services.ContainerDependencies();
            // --------------

            services.AddControllersWithViews();

            // sayfalara girenin authorize olmas� gereksin diye [AllowAnonymous]  controller a ekleyerek o controller a authorize olmadan girilebilir yap�yoruz 
            // --------------
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddMvc();
            // --------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // LOGGER ���N DOSYAYA ILoggerFactory
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // dosyaya yazma kodlar�
            // --------------
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log1.txt");
            // --------------
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404", "?code={0}"); // error sayfas� eklemek
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Default}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
