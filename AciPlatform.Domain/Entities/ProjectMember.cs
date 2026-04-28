using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("ProjectMembers")]
public class ProjectMember
{
    [Key]
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int UserId { get; set; }

    [MaxLength(50)]
    public string Role { get; set; } = "Researcher"; // Manager, Researcher, Secretary, Advisor

    [MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active (Đang hoạt động), OnLeave (Nghỉ phép), Suspended (Tạm dừng), Completed (Đã hoàn thành)

    public DateTime JoinDate { get; set; } = DateTime.Now;

    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }
}
