namespace AciPlatform.Application.DTOs;

public class MenuPagingationRequestModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchText { get; set; }
    public bool? isParent { get; set; }
    public string? CodeParent { get; set; }
    public int? userRoleId { get; set; }
}

public class UserViewModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchText { get; set; }
    public DateTime? Birthday { get; set; }
    public int? Gender { get; set; }
    public int? PositionId { get; set; }
    public int? Warehouseid { get; set; }
    public int? DepartmentId { get; set; }
    public bool? RequestPassword { get; set; }
    public bool? Quit { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Targetid { get; set; }
    public int? Month { get; set; }
    public int? Degreeid { get; set; }
    public int? Certificateid { get; set; }
    public List<int>? Ids { get; set; }
}

public class UserFilterParams
{
    public string? Keyword { get; set; }
    public DateTime? BirthDay { get; set; }
    public int? Gender { get; set; }
    public int? PositionId { get; set; }
    public int? WarehouseId { get; set; }
    public int? DepartmentId { get; set; }
    public bool? RequestPassword { get; set; }
    public bool? Quit { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? TargetId { get; set; }
    public int? Month { get; set; }
    public int DegreeId { get; set; }
    public int CertificateId { get; set; }
    public List<int>? Ids { get; set; }
    public List<string> roles { get; set; } = new();
    public int UserId { get; set; }
}

public class PagingRequestModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchText { get; set; }
}

public class MenuViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? NameEN { get; set; }
    public string? NameKO { get; set; }
    public string? CodeParent { get; set; }
    public bool? IsParent { get; set; }
    public int? Order { get; set; }
    public string? Note { get; set; }
}

public class AuthenticateSocialModel
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Avarta { get; set; }
    public string? Provider { get; set; }
    public string? PhotoUrl { get; set; }
    public int Gender { get; set; }
    public string? ProviderId { get; set; }
}

public class WebCustomerV2Model
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Address { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }
}

public class WebCustomerUpdateModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }
}

public class BaseResponseModel
{
    public object? Data { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
}

public class BaseResponseCommonModel
{
    public object? Data { get; set; }
}

public class InvoiceAuthorizeRequest
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
}
