using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("ProjectTasks")]
public class ProjectTask
{
    [Key]
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int? AssignedToUserId { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Todo"; // Todo, InProgress, Review, Done

    public int Weight { get; set; } = 1; // Trọng số để tính % tiến độ

    public int Progress { get; set; } = 0; // 0-100%

    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }

    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }
}
