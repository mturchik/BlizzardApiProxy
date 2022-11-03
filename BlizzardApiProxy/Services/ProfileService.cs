using BlizzardApiProxy.Models;
using System.Net.Http.Headers;

namespace BlizzardApiProxy.Services;
public class ProfileService
{
    private readonly TokenService _tokenService;
    private readonly HttpClient _httpClient;
    private const string _defaultQuery = "?namespace=profile-us&locale=en_US";

    public ProfileService(TokenService tokenService, IHttpClientFactory httpClientFactory)
    {
        _tokenService = tokenService;

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://us.api.blizzard.com/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetAuthToken().Access_Token);
    }

    public Task<Character?> GetCharacterSummary(string realm, string character)
    {
        return _httpClient.GetFromJsonAsync<Character>($"profile/wow/character/{realm}/{character}" + _defaultQuery);
    }

    public Task<CharacterEquipment?> GetCharacterEquipment(string realm, string character)
    {
        return _httpClient.GetFromJsonAsync<CharacterEquipment>($"profile/wow/character/{realm}/{character}/equipment" + _defaultQuery);
    }
}
