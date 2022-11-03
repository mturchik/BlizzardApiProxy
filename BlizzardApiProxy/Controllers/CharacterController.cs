using BlizzardApiProxy.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;
[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ProfileService _profileService;

    public CharacterController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("/{realm}/{character}/summary", Name = "GetCharacterSummary")]
    public async Task<string> GetCharacterSummary(string realm, string character)
    {
        var summary = await _profileService.GetCharacterSummary(realm, character) ?? new();
        return $"{summary.Name},{summary.Character_Class?.Name},{summary.Active_Spec?.Name},{summary.Average_Item_Level},{summary.Equipped_Item_Level}";
    }

    [HttpGet("/{realm}/{character}/equipment", Name = "GetCharacterEquipment")]
    public async Task<string> GetCharacterEquipment(string realm, string character)
    {
        var equipment = await _profileService.GetCharacterEquipment(realm, character) ?? new();
        var str = equipment.Character?.Name ?? "";
        equipment.Equipped_Items?.ForEach(i =>
        {
            str += "," + i.Name?.Replace(",", "");
            if (i.Sockets?.Count > 0) str += "[+]";
        });
        return str;
    }
}
