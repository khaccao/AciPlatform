using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.HopDong;

public class UserContractHistory
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ContractTypeId { get; set; }

    public DateTime? SignedDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(255)]
    public string? FileUrl { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
