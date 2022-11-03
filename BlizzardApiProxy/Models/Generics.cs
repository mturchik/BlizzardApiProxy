namespace BlizzardApiProxy.Models;
public class DisplaySlug
{
    public string? Name { get; set; }
    public long? Id { get; set; }
    public string? Display_String { get; set; }
    public string? Slug { get; set; }
}

public class TypeNamePair
{
    public string? Type { get; set; }
    public string? Name { get; set; }
}

public class IdNamePair
{
    public long? Id { get; set; }
    public string? Name { get; set; }
}
