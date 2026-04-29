using AciPlatform.Domain.Entities.BaseEntities;

namespace AciPlatform.Domain.Entities.Ledger;

public class Warehouse : BaseEntity
{
    public int Id { get; set; }
    public int? BranchId { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? ManagerName { get; set; }
    public bool IsSyncChartOfAccount { get; set; }
}
