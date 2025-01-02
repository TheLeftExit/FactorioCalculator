public record Recipe(string Name)
{
    public required double Duration { get; init; }
    public required StationKind Station { get; init; }
    public required ItemSlot[] Ingredients { get; init; }
    public required ItemSlot[] Products { get; init; }
}

[Flags]
public enum StationKind
{
    Miner = 0x00001,
    Furnace = 0x00002,
    AssemblingMachine = 0x00004,
    ChemicalPlant = 0x00008,
    OilRefinery = 0x00010,
    Foundry = 0x00020,
    ElectromagneticPlant = 0x00040,
    Biochamber = 0x00080,
    CryogenicPlant = 0x00100,
    RocketSilo = 0x00200,
    CaptiveSpawner = 0x00400,
    Centrifuge = 0x00800,
    Crusher = 0x01000,
    Recycler = 0x02000,
    OffshorePump = 0x04000,
    Pumpjack = 0x08000,
    Invalid = 0x10000,
    AgriculturalTower = 0x20000,
    Hand = 0x40000
}

public readonly record struct ItemSlot(string Name, double Amount)
{
    public static ItemSlot operator -(ItemSlot itemSlot) => new(itemSlot.Name, -itemSlot.Amount);
}

public record struct Building(StationKind Kind, string Name, double Speed, double Productivity)
{
    public IEnumerable<ItemSlot> GetBalance(Recipe recipe, int count = 1)
    {
        foreach (var item in recipe.Products) yield return item with { Amount = count * item.Amount * Speed * Productivity / recipe.Duration };
        foreach (var item in recipe.Ingredients) yield return item with { Amount = count * -item.Amount * Speed / recipe.Duration };
    }
}

public class RecipeTree
{
    public required Building[] Buildings { get; init; }
    public required RecipeTreeNode[] Nodes { get; init; }
    public required Recipe[] GenZero { get; init; }
}

public record struct RecipeTreeNode(string ItemName, string RecipeName)
{
    public RecipeTreeNode(string itemName) : this(itemName, itemName) { }
}