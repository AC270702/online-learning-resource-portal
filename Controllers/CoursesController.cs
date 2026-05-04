using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLearningResourcePortal.Data;

namespace OnlineLearningResourcePortal.Controllers;

public class CoursesController(ApplicationDbContext db) : Controller
{
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var courses = await db.Courses
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        return View(courses);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var course = await db.Courses.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (course == null)
            return NotFound();

        return View(course);
    }
}
