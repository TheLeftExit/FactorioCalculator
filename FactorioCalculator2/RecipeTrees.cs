public class RecipeTrees
{
    public static RecipeTree NauvisEarly { get; } = new()
    {
        Buildings = [
            new(StationKind.OffshorePump, "Offshore pump", 1, 1),
            new(StationKind.Miner, "Electric mining drill", 0.5, 1.1),
            new(StationKind.Furnace, "Electric furnace", 2, 1),
            new(StationKind.AssemblingMachine, "Assembling machine 2", 0.75, 1),
            new(StationKind.Pumpjack, "Pumpjack (%)", 1, 1.1),
            new(StationKind.OilRefinery, "Oil refinery", 1, 1),
            new(StationKind.ChemicalPlant, "Chemical plant", 1, 1)
        ],
        Nodes = [
            new("iron-plate"),
            new("copper-plate"),

            new("iron-gear-wheel"),
            new("automation-science-pack"),

            new("copper-cable"),
            new("electronic-circuit"),
            new("transport-belt"),
            new("inserter"),
            new("logistic-science-pack"),

            new("stone-brick"),
            new("stone-wall"),
            new("grenade"),
            new("firearm-magazine"),
            new("steel-plate"),
            new("piercing-rounds-magazine"),
            new("military-science-pack"),

            new("petroleum-gas", "light-oil-cracking"),
            new("light-oil", "heavy-oil-cracking"),
            new("heavy-oil", "advanced-oil-processing"),
            
            new("solid-fuel", "solid-fuel-from-light-oil"),
            new("rocket-fuel"),
            
            new("sulfur"),
            new("plastic-bar"),
            new("advanced-circuit"),
            new("pipe"),
            new("engine-unit"),
            new("chemical-science-pack"),

            new("iron-stick"),
            new("rail"),
            new("electric-furnace"),
            new("productivity-module"),
            new("production-science-pack"),

            new("lubricant"),
            new("electric-engine-unit"),
            new("sulfuric-acid"),
            new("battery"),
            new("flying-robot-frame"),
            new("low-density-structure"),
            new("processing-unit"),
            new("utility-science-pack")
        ],
        GenZero = [
            new("iron-ore") {
                Duration = 1,
                Ingredients = [],
                Products = [new("iron-ore", 1)],
                Station = StationKind.Miner
            },
            new("copper-ore") {
                Duration = 1,
                Ingredients = [],
                Products = [new("copper-ore", 1)],
                Station = StationKind.Miner
            },
            new("stone") {
                Duration = 1,
                Ingredients = [],
                Products = [new("stone", 1)],
                Station = StationKind.Miner
            },
            new("coal") {
                Duration = 1,
                Ingredients = [],
                Products = [new("coal", 1)],
                Station = StationKind.Miner
            },
            new("water") {
                Duration = 1,
                Ingredients = [],
                Products = [new("water", 1200)],
                Station = StationKind.OffshorePump
            },
            new("crude-oil") {
                Duration = 1,
                Ingredients = [],
                Products = [new("crude-oil", 0.1)],
                Station = StationKind.Pumpjack
            },
        ]
    };
}

