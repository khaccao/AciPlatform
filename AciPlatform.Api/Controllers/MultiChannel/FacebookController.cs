using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AciPlatform.Application.Interfaces.MultiChannel;
using AciPlatform.Application.DTOs.MultiChannel;
using System.Text.Json;
using System.Net.Http;
using AciPlatform.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Api.Controllers.MultiChannel;

[Route("api/v1/multichannel")]
[ApiController]
public class FacebookController : ControllerBase
{
    private readonly IFacebookService _facebookService;
    private readonly IAIService _aiService;
    private readonly IAutomationService _automationService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IApplicationDbContext _dbContext;

    public FacebookController(
        IFacebookService facebookService, 
        IAIService aiService, 
        IAutomationService automationService,
        IHttpClientFactory httpClientFactory,
        IApplicationDbContext dbContext)
    {
        _facebookService = facebookService;
        _aiService = aiService;
        _automationService = automationService;
        _httpClientFactory = httpClientFactory;
        _dbContext = dbContext;
    }

    [HttpGet("app-config")]
    public async Task<IActionResult> GetAppConfig()
    {
        var config = await _facebookService.GetAppConfigAsync();
        return Ok(config);
    }

    [HttpPost("app-config")]
    public async Task<IActionResult> UpdateAppConfig([FromBody] JsonElement model)
    {
        string appId = model.GetProperty("appId").GetString() ?? "";
        string appSecret = model.GetProperty("appSecret").GetString() ?? "";
        await _facebookService.UpdateAppConfigAsync(appId, appSecret);
        return Ok(new { message = "Config updated" });
    }

    /// <summary>
    /// Generates Facebook OAuth URL for login
    /// </summary>
    [HttpGet("oauth-url")]
    public async Task<IActionResult> GetOAuthUrl([FromQuery] string redirectUri)
    {
        var config = await _facebookService.GetAppConfigAsync();
        if (string.IsNullOrEmpty(config.AppId))
        {
            return BadRequest(new { error = "Facebook App ID not configured" });
        }

        var scopes = "pages_show_list,pages_read_engagement,pages_manage_posts,pages_read_user_content,public_profile,email";
        var oauthUrl = $"https://www.facebook.com/v18.0/dialog/oauth?client_id={config.AppId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={scopes}&response_type=code";
        
        return Ok(new { url = oauthUrl });
    }

    /// <summary>
    /// Exchange authorization code for access token and fetch user pages
    /// </summary>
    [HttpPost("oauth-callback")]
    public async Task<IActionResult> HandleOAuthCallback([FromBody] JsonElement model)
    {
        var code = model.GetProperty("code").GetString();
        var redirectUri = model.GetProperty("redirectUri").GetString();

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(redirectUri))
        {
            return BadRequest(new { error = "Missing code or redirectUri" });
        }

        var config = await _facebookService.GetAppConfigAsync();
        if (string.IsNullOrEmpty(config.AppId) || string.IsNullOrEmpty(config.AppSecret))
        {
            return BadRequest(new { error = "Facebook App not configured" });
        }

        var httpClient = _httpClientFactory.CreateClient();

        // Step 1: Exchange code for user access token
        var tokenUrl = $"https://graph.facebook.com/v18.0/oauth/access_token?client_id={config.AppId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&client_secret={config.AppSecret}&code={code}";
        
        var tokenResponse = await httpClient.GetAsync(tokenUrl);
        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        
        if (!tokenResponse.IsSuccessStatusCode)
        {
            return BadRequest(new { error = "Failed to exchange code for token", details = tokenContent });
        }

        var tokenJson = JsonSerializer.Deserialize<JsonElement>(tokenContent);
        var userAccessToken = tokenJson.GetProperty("access_token").GetString();

        // Step 2: Get long-lived token
        var longLivedUrl = $"https://graph.facebook.com/v18.0/oauth/access_token?grant_type=fb_exchange_token&client_id={config.AppId}&client_secret={config.AppSecret}&fb_exchange_token={userAccessToken}";
        var longLivedResponse = await httpClient.GetAsync(longLivedUrl);
        var longLivedContent = await longLivedResponse.Content.ReadAsStringAsync();
        
        if (longLivedResponse.IsSuccessStatusCode)
        {
            var longLivedJson = JsonSerializer.Deserialize<JsonElement>(longLivedContent);
            userAccessToken = longLivedJson.GetProperty("access_token").GetString();
        }

        // Step 3: Get user's pages
        var pagesUrl = $"https://graph.facebook.com/v18.0/me/accounts?access_token={userAccessToken}";
        var pagesResponse = await httpClient.GetAsync(pagesUrl);
        var pagesContent = await pagesResponse.Content.ReadAsStringAsync();

        if (!pagesResponse.IsSuccessStatusCode)
        {
            return BadRequest(new { error = "Failed to fetch pages", details = pagesContent });
        }

        var pagesJson = JsonSerializer.Deserialize<JsonElement>(pagesContent);
        var pages = new List<object>();

        if (pagesJson.TryGetProperty("data", out var pagesData))
        {
            foreach (var page in pagesData.EnumerateArray())
            {
                var pageId = page.GetProperty("id").GetString();
                var pageName = page.GetProperty("name").GetString();
                var pageAccessToken = page.GetProperty("access_token").GetString();

                pages.Add(new
                {
                    pageId,
                    name = pageName,
                    accessToken = pageAccessToken
                });

                // Auto-connect page
                await _facebookService.ConnectPageAsync(
                    userId: 1, // TODO: Get from auth
                    pageId: pageId ?? "",
                    name: pageName ?? "",
                    accessToken: pageAccessToken ?? "",
                    userToken: userAccessToken ?? ""
                );
            }
        }

        return Ok(new { 
            message = "Facebook connected successfully",
            userAccessToken,
            pagesConnected = pages.Count,
            pages
        });
    }

    [HttpGet("pages")]
    public async Task<IActionResult> GetPages()
    {
        int userId = 1;
        var pages = await _facebookService.GetConnectedPagesAsync(userId);
        return Ok(pages);
    }

    [HttpPost("connect-page")]
    public async Task<IActionResult> ConnectPage([FromBody] ConnectPageDto dto)
    {
        int userId = 1; 
        await _facebookService.ConnectPageAsync(userId, dto.PageId, dto.Name, dto.AccessToken, dto.UserToken);
        return Ok(new { message = "Page connected successfully" });
    }

    [HttpPost("disconnect-page/{pageId}")]
    public async Task<IActionResult> DisconnectPage(int pageId)
    {
        await _facebookService.DisconnectPageAsync(pageId);
        return Ok(new { message = "Page disconnected" });
    }

    [HttpPost("post")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
    {
        int userId = 1;
        
        string content = dto.Content;
        if (dto.AutoGenerateContent && !string.IsNullOrEmpty(dto.AiPrompt))
        {
            content = await _aiService.GenerateContentAsync(dto.AiPrompt);
        }

        var post = await _facebookService.CreatePostAsync(userId, dto.PageId, content, dto.ImageUrls, dto.ScheduledTime);

        if (post.Status == "Draft" && !dto.ScheduledTime.HasValue) 
        {
             bool result = await _facebookService.PublishPostNowAsync(post.Id);
             if (!result) return StatusCode(500, "Failed to publish post");
        }

        return Ok(post);
    }

    /// <summary>
    /// Post to Facebook Page directly using Graph API
    /// </summary>
    [HttpPost("publish")]
    public async Task<IActionResult> PublishToFacebook([FromBody] JsonElement model)
    {
        var pageId = model.GetProperty("pageId").GetInt32();
        var message = model.GetProperty("message").GetString();

        // Get page from DB
        var page = await _dbContext.FacebookPages.FindAsync(pageId);
        if (page == null)
        {
            return NotFound(new { error = "Page not found" });
        }

        var httpClient = _httpClientFactory.CreateClient();
        var postUrl = $"https://graph.facebook.com/v18.0/{page.PageId}/feed";
        
        var postData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("message", message ?? ""),
            new KeyValuePair<string, string>("access_token", page.AccessToken)
        });

        var response = await httpClient.PostAsync(postUrl, postData);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest(new { error = "Failed to post", details = responseContent });
        }

        return Ok(new { success = true, response = responseContent });
    }

    [HttpGet("insights/{pageId}")]
    public async Task<IActionResult> GetInsights(int pageId, [FromQuery] string metric = "impressions")
    {
        var data = await _facebookService.GetPageInsightsAsync(pageId, metric);
        return Ok(data);
    }

    [HttpPost("automation")]
    public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowDto dto)
    {
        int userId = 1;
        var wf = await _automationService.CreateWorkflowAsync(userId, dto.Name, dto.WorkflowJson, dto.TriggerType);
        return Ok(wf);
    }

    [HttpGet("automation")]
    public async Task<IActionResult> GetWorkflows()
    {
        int userId = 1;
        var wfs = await _automationService.GetWorkflowsAsync(userId);
        return Ok(wfs);
    }
}
