



namespace all_spice.Repositories;

public class IngredientsRepository
{
    private readonly IDbConnection _db;

    public IngredientsRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Ingredient CreateIngredient(Ingredient newIngredient)
    {
        string sql = @"
        INSERT INTO ingredients
        (name, quantity, recipe_id)
        VALUES
        (@Name, @Quantity, @RecipeId);
        
        SELECT ingredients.*
        FROM ingredients
        WHERE ingredients.id = LAST_INSERT_ID();";
        Ingredient ingredient = _db.Query<Ingredient>(sql, newIngredient).FirstOrDefault();

        return ingredient;
    }

    internal List<Ingredient> GetIngredientsByRecipeId(int recipeId)
    {
        string sql = @"
        SELECT ingredients.*
        FROM ingredients
        WHERE recipe_id = @RecipeId;";
        List<Ingredient> ingredients = _db.Query<Ingredient>(sql, new { RecipeId = recipeId }).ToList();

        return ingredients;
    }

    internal Ingredient GetIngredientById(int ingredientId)
    {
        string sql = @"
        SELECT ingredients.*
        FROM ingredients
        WHERE id = @IngredientId;";
        Ingredient ingredient = _db.Query<Ingredient>(sql, new { IngredientId = ingredientId }).FirstOrDefault();

        return ingredient;
    }

    internal void DeleteIngredientByIngredientId(int ingredientId)
    {
        string sql = @"
        DELETE FROM ingredients
        WHERE id = @IngredientId LIMIT 1;";
        int rowsAffected = _db.Execute(sql, new { IngredientId = ingredientId });

        if (rowsAffected != 1)
        {
            throw new Exception(rowsAffected + " rows affected. Something went wrong");
        }
    }
}