using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLearningResourcePortal.Data;
using OnlineLearningResourcePortal.Models;

namespace OnlineLearningResourcePortal.Controllers;

[Authorize]
public class FavoritesController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Challenge();

        var favorites = await db.Favorites
            .AsNoTracking()
            .Include(f => f.Course)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.Id)
            .ToListAsync();

        return View(favorites);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Challenge();

        var courseExists = await db.Courses.AsNoTracking().AnyAsync(c => c.Id == id);
        if (!courseExists)
            return NotFound();

        var existing = await db.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.CourseId == id);
        if (existing != null)
        {
            db.Favorites.Remove(existing);
            TempData["Flash"] = "Removed from favorites.";
        }
        else
        {
            db.Favorites.Add(new Favorite { UserId = userId, CourseId = id });
            TempData["Flash"] = "Saved to favorites.";
        }

        await db.SaveChangesAsync();
        return RedirectToAction(nameof(CoursesController.Details), "Courses", new { id });
    }
}
