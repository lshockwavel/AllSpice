


namespace all_spice.Repositories;

public class RecipesRepository
{
    private readonly IDbConnection _db;

    public RecipesRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Recipe CreateRecipe(Recipe newRecipe)
    {
        string sql = @"
        INSERT INTO recipe
        (title, instructions, img, category, creator_id)
        VALUES
        (@Title, @Instructions, @Img, @Category, @CreatorId);
        
        SELECT recipe.*,
        accounts.*
        FROM recipe
        INNER JOIN accounts ON recipe.creator_id = accounts.id
        WHERE recipe.id = LAST_INSERT_ID();";
        Recipe createdRecipe = _db.Query(sql, (Recipe recipe, Account account) =>
        {
            recipe.Creator = account;
            return recipe;
        }, newRecipe).FirstOrDefault();

        return createdRecipe;
    }

    internal void DeleteRecipe(int recipeId)
    {
        string sql = @"DELETE FROM recipe
         WHERE id = @RecipeId LIMIT 1;";

        int rowsAffected = _db.Execute(sql, new { RecipeId = recipeId });

        if (rowsAffected != 1)
        {
            throw new Exception("Unable to delete recipe");
        }
    }

    internal void EditRecipe(Recipe original)
    {
        string sql = @"
        UPDATE recipe
        SET
            title = @Title,
            instructions = @Instructions,
            img = @Img,
            category = @Category
        WHERE id = @Id;";
        int rowsAffected = _db.Execute(sql, original);

        if (rowsAffected != 1)
        {
            throw new Exception("Unable to edit recipe");
        }
    }

    internal List<Recipe> GetAllRecipes()
    {
        string sql = @"
        SELECT 
        recipe.*,
        accounts.*
        FROM recipe
        INNER JOIN accounts ON recipe.creator_id = accounts.id;";
        List<Recipe> recipes = _db.Query<Recipe, Account, Recipe>(sql, (recipe, account) =>
        {
            recipe.Creator = account;
            return recipe;
        }).ToList();
        return recipes;
    }

    internal Recipe GetRecipeById(int recipeId)
    {
        string sql = @"
        SELECT 
        recipe.*,
        accounts.*
        FROM recipe
        INNER JOIN accounts ON recipe.creator_id = accounts.id
        WHERE recipe.id = @RecipeId;";
        Recipe recipe = _db.Query(sql, (Recipe recipe, Account account) =>
        {
            recipe.Creator = account;
            return recipe;
        }, new { RecipeId = recipeId }).FirstOrDefault();
        return recipe;
    }
}