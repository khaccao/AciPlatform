using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("ProjectDocuments")]
public class ProjectDocument
{
    [Key]
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int? TaskId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    public string FilePath { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? FileType { get; set; }

    public int UploadedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }

    [ForeignKey("TaskId")]
    public virtual ProjectTask? Task { get; set; }
}
