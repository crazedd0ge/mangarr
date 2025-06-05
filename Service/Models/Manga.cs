
using System.ComponentModel.DataAnnotations.Schema;


public class Manga
{

    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }

    public string? CoverImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
