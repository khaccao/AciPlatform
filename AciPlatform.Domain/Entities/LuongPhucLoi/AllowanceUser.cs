using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.LuongPhucLoi;

public class AllowanceUser
{
    [Key]
    public int Id { get; set; }

    public int AllowanceId { get; set; }

    public int UserId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? AmountOverride { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
