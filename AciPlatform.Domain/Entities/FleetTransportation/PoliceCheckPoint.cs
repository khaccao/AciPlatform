using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("PoliceCheckPoints")]
public class PoliceCheckPoint
{
    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string? Code { get; set; }

    [MaxLength(255)]
    public string? Name { get; set; }

    public double Amount { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
