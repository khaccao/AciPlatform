namespace AciPlatform.Application.DTOs;

public class MenuPermissionDto
{
    public int? Id { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? NameEN { get; set; }
    public string? NameKO { get; set; }
    public int? Order { get; set; }
    public bool View { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Approve { get; set; }
    public bool IsParent { get; set; }
    public string? CodeParent { get; set; }
}
