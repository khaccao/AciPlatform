using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.MultiChannel;
using AciPlatform.Domain.Entities.MultiChannel;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AciPlatform.Application.Services.MultiChannel;

public class FacebookService : IFacebookService
{
    private readonly IApplicationDbContext _context;
    // private readonly IHttpClientFactory _httpClientFactory; // For real API calls

    public FacebookService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FacebookAppConfig> GetAppConfigAsync()
    {
        return await _context.FacebookAppConfigs.FirstOrDefaultAsync() 
               ?? new FacebookAppConfig { AppId = "", AppSecret = "" };
    }

    public async Task UpdateAppConfigAsync(string appId, string appSecret)
    {
        var config = await _context.FacebookAppConfigs.FirstOrDefaultAsync();
        if (config == null)
        {
            config = new FacebookAppConfig { AppId = appId, AppSecret = appSecret };
            _context.FacebookAppConfigs.Add(config);
        }
        else
        {
            config.AppId = appId;
            config.AppSecret = appSecret;
            _context.FacebookAppConfigs.Update(config);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<List<FacebookPage>> GetConnectedPagesAsync(int userId)
    {
        // Assuming we want to show global pages or user specific? Requirement says "Lưu Page", "Danh sách page". 
        // We'll return all active pages for now or filter by user if specific permission logic exists.
        return await _context.FacebookPages.Where(p => p.IsActive).ToListAsync();
    }

    public async Task ConnectPageAsync(int userId, string pageId, string name, string accessToken, string userToken)
    {
        var existing = await _context.FacebookPages.FirstOrDefaultAsync(p => p.PageId == pageId);
        if (existing != null)
        {
            existing.AccessToken = accessToken;
            existing.UserAccessToken = userToken;
            existing.Name = name;
            existing.IsActive = true;
            existing.TokenExpiresAt = DateTime.UtcNow.AddDays(60); // approx
            _context.FacebookPages.Update(existing);
        }
        else
        {
            var page = new FacebookPage
            {
                PageId = pageId,
                Name = name,
                AccessToken = accessToken,
                UserAccessToken = userToken,
                ConnectedByUserId = userId,
                TokenExpiresAt = DateTime.UtcNow.AddDays(60)
            };
            _context.FacebookPages.Add(page);
        }
        await _context.SaveChangesAsync();
    }

    public async Task DisconnectPageAsync(int pageId)
    {
        var page = await _context.FacebookPages.FindAsync(pageId);
        if (page != null)
        {
            page.IsActive = false;
            _context.FacebookPages.Update(page);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<SocialPost> CreatePostAsync(int userId, int pageId, string content, string? imageUrls, DateTime? scheduledTime)
    {
        var post = new SocialPost
        {
            PageId = pageId,
            Content = content,
            ImageUrls = imageUrls,
            ScheduledTime = scheduledTime,
            CreatedByUserId = userId,
            Status = scheduledTime.HasValue ? "Scheduled" : "Draft",
            IsPosted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.SocialPosts.Add(post);
        await _context.SaveChangesAsync();
        
        // If no schedule, try to post immediately? 
        // The interface has PublishPostNowAsync separated or CreatePost (Immediate).
        // Requirement: "Đăng ngay", "Đăng bài hẹn giờ".
        // If scheduledTime is null, user might call PublishPostNowAsync manually next.
        
        return post;
    }

    public async Task<bool> PublishPostNowAsync(int postId)
    {
        var post = await _context.SocialPosts.Include(p => p.Page).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null || post.Page == null) return false;

        try
        {
            // Simulate FB API call
            // var client = _httpClientFactory.CreateClient();
            // var response = await client.PostAsync($"https://graph.facebook.com/{post.Page.PageId}/feed?message={post.Content}&access_token={post.Page.AccessToken}...");
            
            // Dummy success
            post.IsPosted = true;
            post.Status = "Posted";
            post.FacebookPostId = "dummy_fb_post_id_" + DateTime.Now.Ticks;
            post.ErrorMessage = null;
            
            _context.SocialPosts.Update(post);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            post.Status = "Failed";
            post.ErrorMessage = ex.Message;
            _context.SocialPosts.Update(post);
            await _context.SaveChangesAsync();
            return false;
        }
    }

    public async Task ProcessScheduledPostsAsync()
    {
        var pendingPosts = await _context.SocialPosts
            .Where(p => !p.IsPosted && p.Status == "Scheduled" && p.ScheduledTime <= DateTime.UtcNow)
            .ToListAsync();

        foreach (var post in pendingPosts)
        {
            await PublishPostNowAsync(post.Id);
        }
    }

    public async Task<string> GetPageInsightsAsync(int pageId, string metric)
    {
        // Placeholder for real Facebook Insights API
        // Would fetch reach, impressions, etc.
        var data = new 
        {
            metric = metric,
            pageId = pageId,
            values = new [] {
                new { date = DateTime.Today.AddDays(-1), value = 120 },
                new { date = DateTime.Today, value = 150 }
            }
        };
        await Task.CompletedTask; return System.Text.Json.JsonSerializer.Serialize(data);
    }
}

