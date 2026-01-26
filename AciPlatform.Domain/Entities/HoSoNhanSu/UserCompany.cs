using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.HoSoNhanSu;

public class UserCompany
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyCode { get; set; } = string.Empty;
}
