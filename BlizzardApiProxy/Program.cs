using BlizzardApiProxy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<ProfileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/{realm}/{character}/summary", async (ProfileService service, string realm, string character) =>
{
    var summary = await service.GetCharacterSummary(realm, character) ?? new();
    return $"{summary.Name},{summary.Character_Class?.Name},{summary.Active_Spec?.Name},{summary.Average_Item_Level},{summary.Equipped_Item_Level}";
})
.WithName("GetCharacterSummary");

app.MapGet("/{realm}/{character}/equipment", async (ProfileService service, string realm, string character) =>
{
    var equipment = await service.GetCharacterEquipment(realm, character) ?? new();
    var str = equipment.Character?.Name;
    equipment.Equipped_Items?.ForEach(i =>
    {
        str += "," + i.Name?.Replace(",", "");
        if (i.Sockets?.Count > 0) str += "[+]";
    });
    return str;
})
.WithName("GetCharacterEquipment");

app.Run();
