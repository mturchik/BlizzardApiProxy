namespace BlizzardApiProxy.Models;

public class Character
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public DisplaySlug? Character_Class { get; set; }
    public DisplaySlug? Active_Spec { get; set; }
    public long? Average_Item_Level { get; set; }
    public long? Equipped_Item_Level { get; set; }
}

public class CharacterEquipment
{
    public Character? Character { get; set; }
    public List<EquippedItem>? Equipped_Items { get; set; }
}

public class EquippedItem
{
    public IdNamePair? Item { get; set; }
    public List<Socket>? Sockets { get; set; }
    public TypeNamePair? Slot { get; set; }
    public string? Name { get; set; }
}

public class Socket
{
    public TypeNamePair? Socket_Type { get; set; }
    public IdNamePair? Item { get; set; }
    public string? Display_String { get; set; }
}
