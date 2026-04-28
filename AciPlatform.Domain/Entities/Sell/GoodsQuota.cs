
using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.Sell;

public class GoodsQuota
{
    public int Id { get; set; }
    public int GoodsQuotaRecipeId { get; set; }
    [MaxLength(255)]
    public string GoodsQuotaCode { get; set; }
    [MaxLength(500)]
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int GoodsQuotaStepId { get; set; }
}


