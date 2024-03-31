using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RentACar.Data;
using RentACar.Data.Entities;
using RentACar.Data.Mapping;

public class SeedMiddleware
{
    private readonly RequestDelegate next;

    public SeedMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, ApplicationDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Check if the request path starts with "/seed" (or any other specific path)
        if (httpContext.Request.Path.StartsWithSegments("/seed"))
        {
            // This request is within the scope of an HTTP request
            // You can safely access the ApplicationDbContext and other scoped services here

            // Database creation logic
            dbContext.Database.EnsureCreated();

            // Add Admin Role
            if (!await roleManager.RoleExistsAsync(GlobalConstants.AdminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdminRoleName));
            }

            // Add Admin User
            if (!userManager.Users.Any(u => u.UserName == "admin"))
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FirstName = "admin",
                    LastName = "admin",
                    UniqueCitizenNumber = "0000000000"
                };
                await userManager.CreateAsync(user, "admin");
                await userManager.AddToRoleAsync(user, GlobalConstants.AdminRoleName);
            }

            // Call the next middleware in the pipeline
            await next(httpContext);
        }
        else
        {
            // This request is not within the scope of "/seed"
            // Call the next middleware in the pipeline without performing seeding logic
            await next(httpContext);
        }
    }
}
