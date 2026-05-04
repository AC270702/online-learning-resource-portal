using System.ComponentModel.DataAnnotations;

namespace OnlineLearningResourcePortal.Models;

public class Course
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Thumbnail image URL")]
    [Url(ErrorMessage = "Enter a valid URL.")]
    public string? ThumbnailUrl { get; set; }

    [Display(Name = "Resource link (video, PDF, etc.)")]
    [Url(ErrorMessage = "Enter a valid URL.")]
    public string? ResourceUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
