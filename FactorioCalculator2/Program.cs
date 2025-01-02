var recipesGlobal = RecipeParser.GetAllRecipes();

var tree = RecipeTrees.NauvisEarly;

var recipeLookup1 = tree.GenZero.Select(x => (x.Name, Recipe: x));
var recipeLookup2 = tree.Nodes.Select(x => recipesGlobal.Single(r => r.Name == x.RecipeName)).Select(x => (x.Name, Recipe: x));
var recipeLookup = recipeLookup1.Concat(recipeLookup2).ToDictionary(x => x.Name, x => x.Recipe);

var recipeNodes = tree.GenZero.Select(x => new RecipeTreeNode(x.Name)).Concat(tree.Nodes).Reverse().ToList();

var startingBalance = new ItemSlot[]
{
    new("automation-science-pack", -0.4),
    new("logistic-science-pack", -0.4),
    new("military-science-pack", -0.4),
    new("chemical-science-pack", -0.4),
    new("production-science-pack", -0.4),
    new("utility-science-pack", -0.4),

    new("processing-unit", -0.2),
    new("rocket-fuel", -0.2),
    new("low-density-structure", -0.2)
};

var factoryNodes = new List<FactoryNode>();

ItemSlot[] GetBalance()
{
    return factoryNodes
        .Select(x => x.Building.GetBalance(recipeLookup[x.RecipeName], x.Count))
        .SelectMany(x => x)
        .Concat(startingBalance)
        .GroupBy(x => x.Name)
        .Select(x => new ItemSlot(x.Key, x.Sum(slot => slot.Amount)))
        .OrderBy(x => recipeNodes.FindIndex(node => node.ItemName == x.Name))
        .ToArray();
}

while (true)
{
    var balance = GetBalance();

    var itemName = balance.FirstOrDefault(x => x.Amount < 0).Name;
    if (itemName is null) break;
    var recipeName = recipeNodes.Single(x => x.ItemName == itemName).RecipeName;

    var factoryNodeIndex = factoryNodes.FindIndex(x => x.RecipeName == recipeName);
    if(factoryNodeIndex != -1)
    {
        factoryNodes[factoryNodeIndex] = factoryNodes[factoryNodeIndex] with { Count = factoryNodes[factoryNodeIndex].Count + 1 };
    }
    else
    {
        var recipe = recipeLookup[recipeName];
        var building = tree.Buildings.First(x => recipe.Station.HasFlag(x.Kind));
        var factoryNode = new FactoryNode(recipeName, building, 1);
        factoryNodes.Add(factoryNode);
    }
}

var results = factoryNodes.Select(x =>
{
    var recipe = recipeLookup[x.RecipeName];
    var balance = x.Building.GetBalance(recipe, x.Count).ToArray();
    var outputCount = recipe.Products.Length == 1 ? balance.IntersectBy(recipe.Products.Select(x => x.Name), x => x.Name).Single().Amount : double.NaN;
    return (x.RecipeName, x.Count, Output: outputCount, Balance: balance);
}).ToArray();

var finalBalance = GetBalance();
;

public record struct FactoryNode(string RecipeName, Building Building, int Count);