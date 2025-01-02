using System.Diagnostics;
using System.Text.Json;

public static class RecipeParser
{
    public static RecipePrototype[] ParseJSON(string path)
    {
        using (var sw = File.CreateText("recipes.json"))
        {
            using var fs = File.OpenRead(path);
            const string separator = ">>";
            using var sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                var s = sr.ReadLine();
                if (s is null) continue;
                var i = s.IndexOf(">>");
                if (i == -1) continue;
                var content = s.AsSpan().Slice(i + separator.Length);
                sw.WriteLine(content);
            }
        }

        using var recipesFile = File.OpenRead("recipes.json");

        return JsonSerializer.Deserialize<RecipePrototype[]>(recipesFile, new JsonSerializerOptions { AllowTrailingCommas = true }) ?? throw new UnreachableException();
    }

    public class RecipePrototype
    {
        public required string Name { get; init; }
        public required double Duration { get; init; }
        public required string Category { get; init; }
        public required Product[] Products { get; init; }
        public required Ingredient[] Ingredients { get; init; }

        public struct Product
        {
            public required string Name { get; init; }
            public required double Amount { get; init; }
            public required double Probability { get; init; }
        }

        public struct Ingredient
        {
            public required string Name { get; init; }
            public required double Amount { get; init; }
        }
    }

    public static StationKind DecodeRecipeCategory(string category)
    {
        return category switch
        {
            "crafting" or "crafting-with-fluid" or "advanced-crafting"
                => StationKind.AssemblingMachine,
            "pressing" or "crafting-with-fluid-or-metallurgy" or "metallurgy-or-assembling"
                => StationKind.AssemblingMachine | StationKind.Foundry,
            "metallurgy"
                => StationKind.Foundry,
            "electronics" or "electronics-or-assembling" or "electronics-with-fluid"
                => StationKind.AssemblingMachine | StationKind.ElectromagneticPlant,
            "smelting"
                => StationKind.Furnace,
            "cryogenics"
                => StationKind.CryogenicPlant,
            "organic-or-assembling"
                => StationKind.AssemblingMachine | StationKind.Biochamber,
            "cryogenics-or-assembling"
                => StationKind.AssemblingMachine | StationKind.CryogenicPlant,
            "electromagnetics"
                => StationKind.ElectromagneticPlant,
            "oil-processing"
                => StationKind.OilRefinery,
            "organic-or-chemistry"
                => StationKind.OilRefinery | StationKind.Biochamber,
            "chemistry"
                => StationKind.ChemicalPlant,
            "chemistry-or-cryogenics"
                => StationKind.ChemicalPlant | StationKind.CryogenicPlant,
            "rocket-building"
                => StationKind.RocketSilo,
            "centrifuging"
                => StationKind.Centrifuge,
            "recycling-or-hand-crafting"
                => StationKind.Recycler,
            "organic-or-hand-crafting" or "organic"
                => StationKind.Biochamber,
            "captive-spawner-process"
                => StationKind.CaptiveSpawner,
            "crushing"
                => StationKind.Crusher,
            "parameters" or "recycling"
                => StationKind.Invalid,
            _ => throw new UnreachableException()
        };
    }

    public static IEnumerable<Recipe> ParseRecipePrototypes(IEnumerable<RecipePrototype> prototypes)
    {
        return prototypes.Select(x => new Recipe(x.Name)
        {
            Duration = x.Duration,
            Station = DecodeRecipeCategory(x.Category),
            Ingredients = x.Ingredients.Select(i => new ItemSlot
            {
                Name = i.Name,
                Amount = i.Amount
            }).ToArray(),
            Products = x.Products.Select(i => new ItemSlot
            {
                Name = i.Name,
                Amount = i.Amount * i.Probability
            }).ToArray()
        });
    }

    public static IEnumerable<Recipe> GetAllRecipes()
    {
        return ParseRecipePrototypes(ParseJSON("factorio-current.log"));
    }
}





