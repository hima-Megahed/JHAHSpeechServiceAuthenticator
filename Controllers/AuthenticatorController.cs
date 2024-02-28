using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JHAHSpeechServiceAuthenticator.Controllers;

[ApiController]
[Route("speechservices/[controller]")]
public class AuthenticatorController : ControllerBase
{
    [HttpGet("token")]
    public async Task<IActionResult> GetToken()
    {
        var SPEECH_SERVICES_REGION = Environment.GetEnvironmentVariable("SPEECHSERVICEREGION");
        var SPEECH_SERVICE_KEY = Environment.GetEnvironmentVariable("SPEECHSERVICEKEY");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SPEECH_SERVICE_KEY);
        var response = await httpClient.PostAsync($"https://{SPEECH_SERVICES_REGION}.api.cognitive.microsoft.com/sts/v1.0/issueToken", new StringContent(string.Empty));
        if (!response.IsSuccessStatusCode) throw new Exception("Was not able to get authentication token!");

        return Ok(new {
            token = await response.Content.ReadAsStringAsync(),
            region = SPEECH_SERVICES_REGION
        });
    }
}
