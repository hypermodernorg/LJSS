using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LJSS.Models;

using LJSS.Data;

namespace LJSS
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
            services.AddControllersWithViews();

            // identity
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Authenticate/Login");
            // end identity

            services.AddDbContext<VocabularyContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));
            services.AddDbContext<KanaContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));
            services.AddDbContext<TranslateContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Ensure the database and tables are created. Each table corresponds to a different
            // context, however, i suspect the correct method is each database should correspond to a context.
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contextWord = serviceScope.ServiceProvider.GetRequiredService<KanaContext>();
                contextWord.Database.Migrate();
                var contextKana = serviceScope.ServiceProvider.GetRequiredService<VocabularyContext>();
                contextKana.Database.Migrate();
 
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // identity
            app.UseAuthentication();
            app.UseAuthorization();
            // end identity
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
