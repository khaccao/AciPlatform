namespace AciPlatform.Domain.Entities.Sell;

public class GoodsQuotaRecipe
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int GoodsQuotaStepId { get; set; }
}


