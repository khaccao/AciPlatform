using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Hotel;

[Table("HotelBeds")]
public class HotelBed
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [MaxLength(20)] public string RoomNo { get; set; } = string.Empty;
    [MaxLength(20)] public string BedCode { get; set; } = string.Empty;
    [MaxLength(100)] public string? BedName { get; set; }
    [MaxLength(20)] public string BedType { get; set; } = "SINGLE"; // BOTTOM/TOP/SINGLE
    [MaxLength(20)] public string Status { get; set; } = "VACANT";
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
