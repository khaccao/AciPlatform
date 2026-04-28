namespace AciPlatform.Application.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "Planned";
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Progress { get; set; } // Tính toán từ các task
}

public class ProjectTaskDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? AssignedToFullName { get; set; }
    public string Status { get; set; } = "Todo";
    public int Weight { get; set; }
    public int Progress { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
}

public class CreateProjectRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
}

public class CreateTaskRequest
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? AssignedToUserId { get; set; }
    public int Weight { get; set; } = 1;
    public DateTime? DueDate { get; set; }
}

public class ProjectMemberDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }
    public string? FullName { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
    public DateTime JoinDate { get; set; }
}
