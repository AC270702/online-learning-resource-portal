using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLearningResourcePortal.Data;
using OnlineLearningResourcePortal.Models;

namespace OnlineLearningResourcePortal.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SeedData.AdminRole)]
public class CoursesController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var courses = await db.Courses
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        return View(courses);
    }

    public IActionResult Create()
    {
        return View(new Course());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid)
            return View(course);

        course.CreatedAt = DateTime.UtcNow;
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var course = await db.Courses.FindAsync(id);
        if (course == null)
            return NotFound();

        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (id != course.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(course);

        var existing = await db.Courses.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Title = course.Title;
        existing.Description = course.Description;
        existing.ThumbnailUrl = course.ThumbnailUrl;
        existing.ResourceUrl = course.ResourceUrl;
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var course = await db.Courses.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (course == null)
            return NotFound();

        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await db.Courses.FindAsync(id);
        if (course != null)
        {
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
