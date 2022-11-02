using BlizzardApiProxy.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlizzardApiProxy.Services;
public class ProfileService
{
    private readonly TokenService _tokenService;
    private readonly HttpClient _httpClient;
    private const string _defaultQuery = "?namespace=profile-us&locale=en_US";

    public ProfileService(TokenService tokenService)
    {
        _tokenService = tokenService;

        _httpClient = HttpClientFactory.Create();
        _httpClient.BaseAddress = new Uri("https://us.api.blizzard.com/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetAuthToken().Access_Token);
    }

    public Task<Character?> GetCharacterSummary(string realm, string name)
    {
        return _httpClient.GetFromJsonAsync<Character>($"profile/wow/character/{realm}/{name}" + _defaultQuery);
    }

    public Task<CharacterEquipment?> GetCharacterEquipment(string realm, string name)
    {
        return _httpClient.GetFromJsonAsync<CharacterEquipment>($"profile/wow/character/{realm}/{name}/equipment" + _defaultQuery);
    }
}
