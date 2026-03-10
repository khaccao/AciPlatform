using AciPlatform.Domain.Entities.MultiChannel;

namespace AciPlatform.Application.Interfaces.MultiChannel;

public interface IFacebookService
{
    // App Config
    Task<FacebookAppConfig> GetAppConfigAsync();
    Task UpdateAppConfigAsync(string appId, string appSecret);

    // Page Management
    Task<List<FacebookPage>> GetConnectedPagesAsync(int userId);
    Task ConnectPageAsync(int userId, string pageId, string name, string accessToken, string userToken);
    Task DisconnectPageAsync(int pageId);

    // Posting
    Task<SocialPost> CreatePostAsync(int userId, int pageId, string content, string? imageUrls, DateTime? scheduledTime);
    Task<bool> PublishPostNowAsync(int postId);
    
    // Automation / Scheduled
    Task ProcessScheduledPostsAsync();

    // Insights
    Task<string> GetPageInsightsAsync(int pageId, string metric);
}

public interface IAutomationService
{
    Task<AutomationWorkflow> CreateWorkflowAsync(int userId, string name, string workflowJson, string triggerType);
    Task<List<AutomationWorkflow>> GetWorkflowsAsync(int userId);
    Task ExecuteWorkflowAsync(int workflowId);
}

public interface IAIService
{
    Task<string> GenerateContentAsync(string prompt, string provider = "gemini");
}
