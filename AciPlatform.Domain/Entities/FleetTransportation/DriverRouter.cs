using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("DriverRouters")]
public class DriverRouter
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public int PetrolConsumptionId { get; set; }

    public double? AdvancePaymentAmount { get; set; }

    public double? FuelAmount { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
