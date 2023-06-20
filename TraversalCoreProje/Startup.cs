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
            // oluþacak log larý Output ekranýnda gösterme ve bir klasörde metin belgesinde gönderme
            services.AddLogging(x =>
            {
                x.ClearProviders();
                x.SetMinimumLevel(LogLevel.Debug); // LogLevel. -> komutlarý var ( debug - trace - ýnformation - error - warning )
                x.AddDebug(); // output üzerinde görüntüler
            });



            // context i eklemek ve Identitye AppUser ve AppRole sýnýflarýmýzý eklemek için yaptýk
            // ayrýca Identity Validasyonlarýnda deðiþiklik yaptýðýmýz CustomIdentityValidator modelini ekledik
            // _logger.logInformation("buraya info gir"); , _logger.logError("buraya Error gir"); // bunu controllerda dependincy ILogger<HomeController> ile ekleyerek kullan
            // --------------
            services.AddDbContext<Context>();
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Context>()
                .AddErrorDescriber<CustomIdentityValidator>().AddEntityFrameworkStores<Context>();
            // dosyaya yazma kýsmý için paket yüklüyoruz -Serilog.Extensions.Logging.File -
            // --------------


            // new ' leyerek manager kullanmayý býrakmak için
            // --------------
            // business layer da oluþsturduðumuz yere attýk kalabalýk olmasýn diye
            services.ContainerDependencies();
            // --------------

            services.AddControllersWithViews();

            // sayfalara girenin authorize olmasý gereksin diye [AllowAnonymous]  controller a ekleyerek o controller a authorize olmadan girilebilir yapýyoruz 
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
        // LOGGER ÝÇÝN DOSYAYA ILoggerFactory
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // dosyaya yazma kodlarý
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
            app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404", "?code={0}"); // error sayfasý eklemek
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
