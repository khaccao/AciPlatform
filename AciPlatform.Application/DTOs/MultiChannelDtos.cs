namespace AciPlatform.Application.DTOs.MultiChannel;

public class ConnectPageDto
{
    public string PageId { get; set; }
    public string Name { get; set; }
    public string AccessToken { get; set; }
    public string UserToken { get; set; }
}

public class CreatePostDto
{
    public int PageId { get; set; }
    public string Content { get; set; }
    public string? ImageUrls { get; set; }
    public DateTime? ScheduledTime { get; set; }
    public bool AutoGenerateContent { get; set; } = false;
    public string? AiPrompt { get; set; }
}

public class WorkflowDto
{
    public string Name { get; set; }
    public string WorkflowJson { get; set; }
    public string TriggerType { get; set; }
}
