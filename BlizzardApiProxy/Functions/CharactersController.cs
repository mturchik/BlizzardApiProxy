using BlizzardApiProxy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace BlizzardApiProxy.Functions;

public class HttpFunctions
{
    private readonly ILogger<HttpFunctions> _logger;
    private readonly ProfileService _profileService;

    public HttpFunctions(ILogger<HttpFunctions> log, ProfileService profileService)
    {
        _logger = log;
        _profileService = profileService;
    }

    [FunctionName("GetCharacterSummary")]
    [OpenApiOperation(operationId: "getCharacterProfile", Summary = "Gets the character summary", Description = "This gets the character summary.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "realm", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The realm name", Description = "Realm Slug", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "character", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The character name", Description = "Full Character Name", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "CSV Formatted Data", Description = "Profile formatted as CSV")]
    public async Task<IActionResult> GetCharacterSummary([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{realm}/{character}/profile")] HttpRequest req, string realm, string character)
    {
        var summary = await _profileService.GetCharacterSummary(realm, character) ?? new();
        return new ObjectResult($"{summary.Name},{summary.Character_Class?.Name},{summary.Active_Spec?.Name},{summary.Average_Item_Level},{summary.Equipped_Item_Level}");
    }

    [FunctionName("GetCharacterEquipment")]
    [OpenApiOperation(operationId: "getCharacterEquipment", Summary = "Gets the character equipment", Description = "This gets the character equipment.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "realm", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The realm name", Description = "Realm Slug", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "character", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The character name", Description = "Full Character Name", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
    public async Task<IActionResult> GetCharacterEquipment([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{realm}/{character}/equipment")] HttpRequest req, string realm, string character)
    {
        var equipment = await _profileService.GetCharacterEquipment(realm, character) ?? new();
        var str = equipment.Character?.Name;
        equipment.Equipped_Items?.ForEach(i =>
        {
            str += "," + i.Name?.Replace(",", "");
            if (i.Sockets?.Count > 0) str += "[+]";
        });
        return new ObjectResult(str);
    }
}

