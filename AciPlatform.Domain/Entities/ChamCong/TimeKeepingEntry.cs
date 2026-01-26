using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.ChamCong;

public class TimeKeepingEntry
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime WorkDate { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public double? WorkingHours { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
