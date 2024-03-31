using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar.Data.Entities;
using RentACar.Data.Mapping;
using RentACar.Data.Middleware;
using RentACar.Data;
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
            services.AddAutoMapper(typeof(StartUp));

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
            // Configure AutoMapper mappings
            AutoMapperConfig.ConfigureMapping();

            if (env.IsDevelopment())
            {
                // Development environment settings
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // Production environment settings
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Custom middleware for database seeding, roles, and admin
            app.UseSeedMiddleware();

            // Redirect HTTP to HTTPS
            app.UseHttpsRedirection();
            // Serve static files
            app.UseStaticFiles();

            // Routing
            app.UseRouting();

            // Authentication
            app.UseAuthentication();
            // Authorization
            app.UseAuthorization();

            // Endpoint routing
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
