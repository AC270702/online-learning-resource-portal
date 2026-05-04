using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLearningResourcePortal.Models;

namespace OnlineLearningResourcePortal.Data;

public static class SeedData
{
    public const string AdminRole = "Admin";
    public const string AdminEmail = "admin@localhost";
    public const string AdminPassword = "Admin123!";

    public static async Task InitializeAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();

        if (!await roleManager.RoleExistsAsync(AdminRole))
            await roleManager.CreateAsync(new IdentityRole(AdminRole));

        var adminUser = await userManager.FindByEmailAsync(AdminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, AdminPassword);
            await userManager.AddToRoleAsync(adminUser, AdminRole);
        }

        if (await context.Courses.AnyAsync())
            return;

        context.Courses.AddRange(
            new Course
            {
                Title = "Introduction to Web Applications",
                Description = "Overview of client/server architecture, HTTP, and building data-driven sites with ASP.NET.",
                ThumbnailUrl = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=800&q=80",
                ResourceUrl = "https://learn.microsoft.com/en-us/aspnet/core/",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "HTML & CSS fundamentals",
                Description = "Semantic HTML5, CSS layout, and responsive design basics for usable interfaces.",
                ThumbnailUrl = "https://images.unsplash.com/photo-1621839673705-6617adf9e890?w=800&q=80",
                ResourceUrl = "https://developer.mozilla.org/en-US/docs/Web/HTML",
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                Title = "Database connectivity with SQL",
                Description = "Relational model, CRUD operations, and connecting a web app to a database.",
                ThumbnailUrl = "https://images.unsplash.com/photo-1544383835-bda2bc66a55d?w=800&q=80",
                ResourceUrl = "https://learn.microsoft.com/en-us/sql/",
                CreatedAt = DateTime.UtcNow
            });

        await context.SaveChangesAsync();
    }
}
