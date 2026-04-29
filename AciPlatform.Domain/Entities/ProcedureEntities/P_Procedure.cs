using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.ProcedureEntities;

public class P_Procedure
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }

    public ICollection<ProcedureCondition> ProcedureConditions { get; set; }
}

