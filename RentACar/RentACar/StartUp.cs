using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Data.Entities;
using RentACar.Data.Mapping;
using RentACar.Data.Middleware;
using RentACar.Data.Services;

namespace RentACar
{
    public class StartUp
    {
        // Constructor
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /* This method gets called by the runtime. 
           Use this method to add services to the container. */
        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add AutoMapper
            services.AddAutoMapper(typeof(StartUp)); // Assuming AutoMapper configurations are in the same assembly
            services.AddAutoMapper(typeof(MappingProfile)); // Register your mapping profile

            // Add Identity with custom user and role types
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddDefaultUI()
            .AddRoles<IdentityRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add UserManager<User>
            services.AddScoped<UserManager<User>>();

            // Add transient services
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IRequestsService, RequestsService>();

            // Add controllers and Razor Pages
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        /* This method gets called by the runtime. 
           Use this method to configure the HTTP request pipeline. */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Custom middleware for database seeding, roles, and admin
            app.UseSeedMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}