



namespace all_spice.Services;

public class RecipesService
{
    private readonly RecipesRepository _repo;
    
    public RecipesService(RecipesRepository repo)
    {
        _repo = repo;
    }

    internal Recipe CreateRecipe(Recipe newRecipe)
    {
        Recipe createdRecipe = _repo.CreateRecipe(newRecipe);
        return createdRecipe;
    }

    internal void DeleteRecipe(int recipeId, Account userInfo)
    {
        Recipe recipe = GetRecipeById(recipeId);
        if (recipe.CreatorId != userInfo.Id)
        {
            throw new Exception("You are not the creator of this recipe. You Monster!");
        }
        _repo.DeleteRecipe(recipeId);
    }

    internal Recipe EditRecipe(int recipeId, Recipe updatedRecipe, Account user)
    {
        Recipe original = GetRecipeById(recipeId);
        if (original.CreatorId != user.Id)
        {
            throw new Exception("You are not the creator of this recipe. You Monster!");
        }
        original.Title = updatedRecipe.Title ?? original.Title;
        original.Instructions = updatedRecipe.Instructions ?? original.Instructions;
        original.Img = updatedRecipe.Img ?? original.Img;
        original.Category = updatedRecipe.Category ?? original.Category;

        // Need to update the database side to reflect changes
        _repo.EditRecipe(original);

        return original;
    }

    internal List<Recipe> GetAllRecipes()
    {
        List<Recipe> recipes = _repo.GetAllRecipes();
        return recipes;
    }

    // internal Recipe GetRecipeById(int recipeId)
    // {
    //     Recipe recipe = _repo.GetRecipeById(recipeId);

    //     if (recipe == null)
    //     {
    //         throw new Exception("Invalid Recipe Id");
    //     }

    //     return recipe;
    // }

    internal Recipe GetRecipeById(int recipeId)
    {
        Recipe recipe = _repo.GetRecipeByIdWithIngredients(recipeId);

        if (recipe == null)
        {
            throw new Exception("Invalid Recipe Id");
        }

        return recipe;
    }

    internal List<Recipe> GetRecipes(string name, string category)
    {
        List<Recipe> recipes = _repo.GetRecipesByQuery(name, category);
        return recipes;
    }
}