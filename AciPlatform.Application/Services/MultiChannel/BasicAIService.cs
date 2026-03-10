using AciPlatform.Application.Interfaces.MultiChannel;

namespace AciPlatform.Application.Services.MultiChannel;

public class BasicAIService : IAIService
{
    // Simple mock or basic HTTP call to OpenAI/Gemini
    public async Task<string> GenerateContentAsync(string prompt, string provider = "gemini")
    {
        // In a real app, we would read API Key from Config and call the endpoint.
        await Task.Delay(500); // simulate latency
        return $"[AI Generated Content for: {prompt}]\n\nExciting new update! Check this out. #Trending #{provider}";
    }
}
