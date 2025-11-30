



namespace all_spice.Services;

public class IngredientsService
{
    private readonly IngredientsRepository _repo;
    private readonly RecipesService _recipesService;
    
    public IngredientsService(IngredientsRepository repo, RecipesService recipesService)
    {
        _repo = repo;
        _recipesService = recipesService;
    }

    internal Ingredient CreateIngredient(Ingredient newIngredient, Account userInfo)
    {
        //Need to find out if the account is the owner of the recipe to which the ingredient is being added
        Recipe recipe = _recipesService.GetRecipeById(newIngredient.RecipeId);
        if (recipe.CreatorId != userInfo.Id)
        {
            throw new Exception("You are not the creator of this recipe. You Monster!");
        }

        Ingredient ingredient = _repo.CreateIngredient(newIngredient);
        return ingredient;
    }

    internal void DeleteIngredientByIngredientId(int ingredientId, Account userInfo)
    {
        Ingredient ingredient = _repo.GetIngredientById(ingredientId);

        if (ingredient == null)
        {
            throw new Exception("Invalid Ingredient Id");
        }

        Recipe recipe = _recipesService.GetRecipeById(ingredient.RecipeId);
        if (recipe.CreatorId != userInfo.Id)
        {
            throw new Exception("You are not the creator of this recipe. You Monster!");
        }
        _repo.DeleteIngredientByIngredientId(ingredientId);
    }

    internal List<Ingredient> GetIngredientsByRecipeId(int recipeId)
    {
        List<Ingredient> ingredients = _repo.GetIngredientsByRecipeId(recipeId);
        return ingredients;
    }
}