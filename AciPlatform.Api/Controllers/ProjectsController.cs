using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AciPlatform.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var companyCode = User.FindFirstValue("CompanyCode");
        var projects = await _projectService.GetAllProjects(companyCode);
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetProjectById(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request)
    {
        var companyCode = User.FindFirstValue("CompanyCode");
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var project = await _projectService.CreateProject(request, companyCode, userId);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateProjectRequest request)
    {
        await _projectService.UpdateProject(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _projectService.DeleteProject(id);
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        await _projectService.UpdateProjectStatus(id, status);
        return NoContent();
    }

    [HttpGet("{id}/tasks")]
    public async Task<IActionResult> GetTasks(int id)
    {
        var tasks = await _projectService.GetProjectTasks(id);
        return Ok(tasks);
    }

    [HttpPost("tasks")]
    public async Task<IActionResult> CreateTask(CreateTaskRequest request)
    {
        var task = await _projectService.CreateTask(request);
        return Ok(task);
    }

    [HttpPut("tasks/{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, CreateTaskRequest request)
    {
        await _projectService.UpdateTask(taskId, request);
        return NoContent();
    }

    [HttpDelete("tasks/{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        await _projectService.DeleteTask(taskId);
        return NoContent();
    }

    [HttpPatch("tasks/{taskId}/progress")]
    public async Task<IActionResult> UpdateTaskProgress(int taskId, [FromQuery] int progress, [FromQuery] string status)
    {
        await _projectService.UpdateTaskProgress(taskId, progress, status);
        return NoContent();
    }

    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetMembers(int id)
    {
        var members = await _projectService.GetProjectMembers(id);
        return Ok(members);
    }

    [HttpPost("{id}/members")]
    public async Task<IActionResult> AddMember(int id, [FromQuery] int userId, [FromQuery] string role)
    {
        await _projectService.AddMember(id, userId, role);
        return Ok();
    }

    [HttpGet("my-tasks")]
    public async Task<IActionResult> GetMyTasks()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdStr, out int userId))
        {
            var tasks = await _projectService.GetTasksByUserId(userId);
            return Ok(tasks);
        }
        return Unauthorized();
    }

    [HttpDelete("{id}/members/{userId}")]
    public async Task<IActionResult> RemoveMember(int id, int userId)
    {
        await _projectService.RemoveMember(id, userId);
        return NoContent();
    }

    [HttpPatch("{id}/members/{userId}/status")]
    public async Task<IActionResult> UpdateMemberStatus(int id, int userId, [FromBody] string status)
    {
        await _projectService.UpdateMemberStatus(id, userId, status);
        return NoContent();
    }
}
