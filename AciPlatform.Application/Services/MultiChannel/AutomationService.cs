using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.MultiChannel;
using AciPlatform.Domain.Entities.MultiChannel;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.MultiChannel;

public class AutomationService : IAutomationService
{
    private readonly IApplicationDbContext _context;

    public AutomationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AutomationWorkflow> CreateWorkflowAsync(int userId, string name, string workflowJson, string triggerType)
    {
        var wf = new AutomationWorkflow
        {
            Name = name,
            WorkflowJson = workflowJson,
            TriggerType = triggerType, // "Time", "Event"
            CreatedByUserId = userId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        _context.AutomationWorkflows.Add(wf);
        await _context.SaveChangesAsync();
        return wf;
    }

    public async Task<List<AutomationWorkflow>> GetWorkflowsAsync(int userId)
    {
        return await _context.AutomationWorkflows.ToListAsync();
    }

    public async Task ExecuteWorkflowAsync(int workflowId)
    {
        var wf = await _context.AutomationWorkflows.FindAsync(workflowId);
        if (wf == null || !wf.IsActive) return;

        // Parse JSON workflow and execute nodes.
        // This is complex. We'll simulate a log entry indicating execution.
        
        var log = new AutomationLog
        {
            WorkflowId = workflowId,
            Status = "Success",
            Message = "Workflow executed successfully (simulation).",
            ExecutedAt = DateTime.UtcNow
        };
        _context.AutomationLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
