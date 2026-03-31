namespace AciPlatform.Application.DTOs;

// Department
public class DepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? ParentId { get; set; }
    public int? Order { get; set; }
    public string? CompanyCode { get; set; }
}

public class DepartmentResponse : DepartmentRequest
{
    public int Id { get; set; }
}

// Position Detail
public class PositionDetailRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? DepartmentId { get; set; }
    public string? Note { get; set; }
    public int? Order { get; set; }
    public string? CompanyCode { get; set; }
}

public class PositionDetailResponse : PositionDetailRequest
{
    public int Id { get; set; }
}

// Degree
public class DegreeRequest
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? School { get; set; }
    public string? Description { get; set; }
    public int? GraduationYear { get; set; }
}

public class DegreeResponse : DegreeRequest
{
    public int Id { get; set; }
}

// Certificate
public class CertificateRequest
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Note { get; set; }
}

public class CertificateResponse : CertificateRequest
{
    public int Id { get; set; }
}

// Major
public class MajorRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class MajorResponse : MajorRequest
{
    public int Id { get; set; }
}

// Relative
public class RelativeRequest
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
}

public class RelativeResponse : RelativeRequest
{
    public int Id { get; set; }
}

// History Achievement
public class HistoryAchievementRequest
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? AchievedDate { get; set; }
    public string? Note { get; set; }
}

public class HistoryAchievementResponse : HistoryAchievementRequest
{
    public int Id { get; set; }
}

// Decision Type
public class DecisionTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Note { get; set; }
}

public class DecisionTypeResponse : DecisionTypeRequest
{
    public int Id { get; set; }
}

// Decide
public class DecideRequest
{
    public int UserId { get; set; }
    public int DecisionTypeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public string? FileUrl { get; set; }
    public string? Note { get; set; }
}

public class DecideResponse : DecideRequest
{
    public int Id { get; set; }
}

// Allowance
public class AllowanceRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

public class AllowanceResponse : AllowanceRequest
{
    public int Id { get; set; }
}

// Allowance User
public class AllowanceUserRequest
{
    public int AllowanceId { get; set; }
    public int UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? AmountOverride { get; set; }
    public string? Note { get; set; }
}

public class AllowanceUserResponse : AllowanceUserRequest
{
    public int Id { get; set; }
}

// Salary Type
public class SalaryTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public decimal BaseAmount { get; set; }
    public string? Formula { get; set; }
    public string? Note { get; set; }
}

public class SalaryTypeResponse : SalaryTypeRequest
{
    public int Id { get; set; }
}

// Contract Type
public class ContractTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int? DurationMonths { get; set; }
}

public class ContractTypeResponse : ContractTypeRequest
{
    public int Id { get; set; }
}

// Contract File
public class ContractFileRequest
{
    public string Name { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public int? ContractTypeId { get; set; }
    public string? Note { get; set; }
}

public class ContractFileResponse : ContractFileRequest
{
    public int Id { get; set; }
}

// User Contract History
public class UserContractHistoryRequest
{
    public int UserId { get; set; }
    public int ContractTypeId { get; set; }
    public DateTime? SignedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? FileUrl { get; set; }
    public string? Note { get; set; }
}

public class UserContractHistoryResponse : UserContractHistoryRequest
{
    public int Id { get; set; }
}

// Time keeping
public class TimeKeepingEntryRequest
{
    public int UserId { get; set; }
    public DateTime WorkDate { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public double? WorkingHours { get; set; }
    public string? Note { get; set; }
}

public class TimeKeepingEntryResponse : TimeKeepingEntryRequest
{
    public int Id { get; set; }
}

public class FaceAttendanceRequest
{
    public int UserId { get; set; }
    public string CapturedImage { get; set; } = string.Empty; // Base64 string from client
    public string? Note { get; set; }
}
