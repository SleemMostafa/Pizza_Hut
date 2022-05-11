using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification;
using Pizza_Hut.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Pizza_Hut
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
            //id customer
            services.AddHttpContextAccessor();
            //
            services.AddDbContext<Context>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Cs"));
            });
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
            services.AddIdentity<Customer, IdentityRole>(
            
            ).AddEntityFrameworkStores<Context>();

            //register Custom Services "Inject Constructor"
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepositor, ProductRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IProductSizeCartRepository, ProductSizeCartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductSizeOrderRepository, ProductSizeOrderRepository>();

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ProductHub>("ProductHub"); 
                endpoints.MapHub<CategoryHub>("CategoryHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
