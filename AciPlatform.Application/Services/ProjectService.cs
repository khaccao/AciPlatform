using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IApplicationDbContext _context;

    public ProjectService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjects(string? companyCode)
    {
        var query = _context.Projects.AsQueryable();
        if (!string.IsNullOrEmpty(companyCode))
        {
            query = query.Where(p => p.CompanyCode == companyCode);
        }

        var projects = await query.ToListAsync();
        var projectDtos = new List<ProjectDto>();

        foreach (var p in projects)
        {
            var dto = MapToDto(p);
            dto.Progress = await CalculateProjectProgress(p.Id);
            projectDtos.Add(dto);
        }

        return projectDtos;
    }

    public async Task<ProjectDto?> GetProjectById(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return null;

        var dto = MapToDto(project);
        dto.Progress = await CalculateProjectProgress(project.Id);
        return dto;
    }

    public async Task<ProjectDto> CreateProject(CreateProjectRequest request, string? companyCode, int userId)
    {
        var project = new Project
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Budget = request.Budget,
            CompanyCode = companyCode,
            CreatedBy = userId,
            Status = "Planned",
            CreatedAt = DateTime.Now
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return MapToDto(project);
    }

    public async Task UpdateProject(int id, CreateProjectRequest request)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            project.Code = request.Code;
            project.Name = request.Name;
            project.Description = request.Description;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;
            project.Budget = request.Budget;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateProjectStatus(int id, string status)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            project.Status = status;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ProjectTaskDto>> GetProjectTasks(int projectId)
    {
        var tasks = await _context.ProjectTasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

        var assignedUserIds = tasks.Where(t => t.AssignedToUserId.HasValue).Select(t => t.AssignedToUserId!.Value).Distinct().ToList();
        var users = await _context.Users
            .Where(u => assignedUserIds.Contains(u.Id))
            .ToListAsync();

        return tasks.Select(t => new ProjectTaskDto
        {
            Id = t.Id,
            ProjectId = t.ProjectId,
            Name = t.Name,
            Description = t.Description,
            AssignedToUserId = t.AssignedToUserId,
            AssignedToFullName = t.AssignedToUserId.HasValue ? users.FirstOrDefault(u => u.Id == t.AssignedToUserId)?.FullName : null,
            Status = t.Status,
            Weight = t.Weight,
            Progress = t.Progress,
            StartDate = t.StartDate,
            DueDate = t.DueDate
        });
    }

    public async Task<ProjectTaskDto> CreateTask(CreateTaskRequest request)
    {
        var task = new ProjectTask
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description,
            AssignedToUserId = request.AssignedToUserId,
            Weight = request.Weight,
            DueDate = request.DueDate,
            Status = "Todo",
            Progress = 0,
            StartDate = DateTime.Now
        };

        _context.ProjectTasks.Add(task);
        await _context.SaveChangesAsync();

        return new ProjectTaskDto
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Name = task.Name,
            Description = task.Description,
            AssignedToUserId = task.AssignedToUserId,
            Status = task.Status,
            Weight = task.Weight,
            Progress = task.Progress,
            DueDate = task.DueDate
        };
    }

    public async Task UpdateTask(int taskId, CreateTaskRequest request)
    {
        var task = await _context.ProjectTasks.FindAsync(taskId);
        if (task != null)
        {
            task.Name = request.Name;
            task.Description = request.Description;
            task.AssignedToUserId = request.AssignedToUserId;
            task.Weight = request.Weight;
            task.DueDate = request.DueDate;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await _context.ProjectTasks.FindAsync(taskId);
        if (task != null)
        {
            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateTaskProgress(int taskId, int progress, string status)
    {
        var task = await _context.ProjectTasks.FindAsync(taskId);
        if (task != null)
        {
            task.Progress = progress;
            task.Status = status;
            if (status == "Done") task.Progress = 100;
            if (task.Progress == 100) task.CompletedDate = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ProjectMemberDto>> GetProjectMembers(int projectId)
    {
        var members = await _context.ProjectMembers
            .Where(m => m.ProjectId == projectId)
            .ToListAsync();

        var memberUserIds = members.Select(m => m.UserId).Distinct().ToList();
        var users = await _context.Users
            .Where(u => memberUserIds.Contains(u.Id))
            .ToListAsync();

        return members.Select(m => new ProjectMemberDto
        {
            Id = m.Id,
            ProjectId = m.ProjectId,
            UserId = m.UserId,
            FullName = users.FirstOrDefault(u => u.Id == m.UserId)?.FullName,
            Role = m.Role,
            Status = m.Status,
            JoinDate = m.JoinDate
        });
    }

    public async Task AddMember(int projectId, int userId, string role)
    {
        var exists = await _context.ProjectMembers.AnyAsync(m => m.ProjectId == projectId && m.UserId == userId);
        if (!exists)
        {
            _context.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = projectId,
                UserId = userId,
                Role = role,
                Status = "Active",
                JoinDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateMemberStatus(int projectId, int userId, string status)
    {
        var member = await _context.ProjectMembers.FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);
        if (member != null)
        {
            member.Status = status;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveMember(int projectId, int userId)
    {
        var member = await _context.ProjectMembers.FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);
        if (member != null)
        {
            _context.ProjectMembers.Remove(member);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ProjectTaskDto>> GetTasksByUserId(int userId)
    {
        var tasks = await _context.ProjectTasks
            .Where(t => t.AssignedToUserId == userId)
            .ToListAsync();

        return tasks.Select(t => new ProjectTaskDto
        {
            Id = t.Id,
            ProjectId = t.ProjectId,
            Name = t.Name,
            Description = t.Description,
            AssignedToUserId = t.AssignedToUserId,
            Status = t.Status,
            Weight = t.Weight,
            Progress = t.Progress,
            StartDate = t.StartDate,
            DueDate = t.DueDate
        });
    }

    private async Task<int> CalculateProjectProgress(int projectId)
    {
        var tasks = await _context.ProjectTasks.Where(t => t.ProjectId == projectId).ToListAsync();
        if (!tasks.Any()) return 0;

        decimal totalWeight = tasks.Sum(t => t.Weight);
        if (totalWeight == 0) return 0;

        decimal completedWeight = tasks.Sum(t => (t.Progress / 100m) * t.Weight);
        
        return (int)Math.Round((completedWeight / totalWeight) * 100);
    }

    private ProjectDto MapToDto(Project p)
    {
        return new ProjectDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Budget = p.Budget,
            CreatedAt = p.CreatedAt
        };
    }
}
