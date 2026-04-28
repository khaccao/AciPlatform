using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces;

public interface IProjectService
{
    // Project CRUD
    Task<IEnumerable<ProjectDto>> GetAllProjects(string? companyCode);
    Task<ProjectDto?> GetProjectById(int id);
    Task<ProjectDto> CreateProject(CreateProjectRequest request, string? companyCode, int userId);
    Task UpdateProject(int id, CreateProjectRequest request);
    Task DeleteProject(int id);
    Task UpdateProjectStatus(int id, string status);
    
    // Task Management
    Task<IEnumerable<ProjectTaskDto>> GetProjectTasks(int projectId);
    Task<ProjectTaskDto> CreateTask(CreateTaskRequest request);
    Task UpdateTask(int taskId, CreateTaskRequest request);
    Task DeleteTask(int taskId);
    Task UpdateTaskProgress(int taskId, int progress, string status);
    
    // Members
    Task<IEnumerable<ProjectMemberDto>> GetProjectMembers(int projectId);
    Task AddMember(int projectId, int userId, string role);
    Task UpdateMemberStatus(int projectId, int userId, string status);
    Task RemoveMember(int projectId, int userId);

    Task<IEnumerable<ProjectTaskDto>> GetTasksByUserId(int userId);
}
