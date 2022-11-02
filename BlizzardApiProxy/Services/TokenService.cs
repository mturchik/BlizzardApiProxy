using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BlizzardApiProxy.Services;
public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private TokenResponse? Token { get; set; }

    public TokenService(IConfiguration configuration)
    {
        _httpClient = HttpClientFactory.Create();
        _configuration = configuration;
    }

    public TokenResponse GetAuthToken()
    {
        if (Token is null || Token.ExpiresOn <= DateTime.UtcNow)
            Token = SetAuthToken().Result;
        return Token;
    }

    private async Task<TokenResponse> SetAuthToken()
    {
        var authRequest = CreateAuthRequest();
        var authResponse = _httpClient.Send(authRequest);
        authResponse.EnsureSuccessStatusCode();

        Token = await authResponse.Content.ReadAsAsync<TokenResponse>();
        if (Token is null) throw new ApplicationException("Failed to authenticate");

        // Set expires timestamp to ensure active token
        Token.ExpiresOn = DateTime.UtcNow.AddSeconds(Token.Expires_In).Subtract(TimeSpan.FromMinutes(5));

        return Token;
    }

    private HttpRequestMessage CreateAuthRequest()
    {
        var authRequest = new HttpRequestMessage(HttpMethod.Post, "https://us.battle.net/oauth/token");

        var clientId = _configuration.GetValue<string>("BattleNetClientId");
        var clientSecret = _configuration.GetValue<string>("BattleNetClientSecret");
        var byteArray = new UTF8Encoding().GetBytes($"{clientId}:{clientSecret}");
        authRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(":region", "us"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };
        authRequest.Content = new FormUrlEncodedContent(formData);

        return authRequest;
    }
}

public class TokenResponse
{
    public string? Access_Token { get; set; }
    public string? Token_Type { get; set; }
    public int Expires_In { get; set; }
    public string? Scope { get; set; }
    [JsonIgnore]
    public DateTime? ExpiresOn { get; set; }
}