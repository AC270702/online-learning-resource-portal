namespace OnlineLearningResourcePortal.Models;

public class Favorite
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;
}
