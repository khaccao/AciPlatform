namespace AciPlatform.Domain.Entities.BaseEntities;
public class BaseEntity : BaseEntityCommon
{
    public DateTime? DeleteAt { get; set; }
    public bool IsDelete { get; set; }
}

