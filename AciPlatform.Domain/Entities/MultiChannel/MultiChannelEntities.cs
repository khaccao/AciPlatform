using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.MultiChannel;

[Table("FacebookAppConfigs")]
public class FacebookAppConfig
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string AppId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string AppSecret { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("FacebookPages")]
public class FacebookPage
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string PageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string AccessToken { get; set; } = string.Empty; // Page Access Token

    public string UserAccessToken { get; set; } = string.Empty; // Token of the user who connected

    public DateTime? TokenExpiresAt { get; set; }

    public int ConnectedByUserId { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? PictureUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("SocialPosts")]
public class SocialPost
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public string? ImageUrls { get; set; } // Comma separated or JSON

    public DateTime? ScheduledTime { get; set; }

    public bool IsPosted { get; set; }

    public string? FacebookPostId { get; set; }

    public int? PageId { get; set; } // FK to FacebookPage

    [ForeignKey("PageId")]
    public virtual FacebookPage? Page { get; set; }

    public string? Status { get; set; } // "Draft", "Scheduled", "Posted", "Failed"

    public string? ErrorMessage { get; set; }

    public string? AiGeneratedConfig { get; set; } // JSON storing prompt used to generate

    public int CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("AutomationWorkflows")]
public class AutomationWorkflow
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string WorkflowJson { get; set; } = string.Empty; // JSON structure of nodes and edges

    public bool IsActive { get; set; }

    public string TriggerType { get; set; } = string.Empty; // "Time", "Event"

    public int CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("AutomationLogs")]
public class AutomationLog
{
    [Key]
    public int Id { get; set; }

    public int WorkflowId { get; set; }

    [ForeignKey("WorkflowId")]
    public virtual AutomationWorkflow Workflow { get; set; } = null!;

    public string Status { get; set; } = string.Empty; // "Success", "Failed"

    public string Message { get; set; } = string.Empty;

    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
}
