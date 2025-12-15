



namespace all_spice.Repositories;

public class FavoritesRepository
{
    private readonly IDbConnection _db;

    public FavoritesRepository(IDbConnection db)
    {
        _db = db;
    }

    internal FavoriteRecipe CreateFavorite(Favorite newFavorite)
    {
        string sql = @"
        INSERT INTO favorites
        (recipe_id, account_id)
        VALUES
        (@RecipeId, @AccountId);
        
        SELECT favorites.*, recipe.*, accounts.*
        FROM favorites
        INNER JOIN recipe ON favorites.recipe_id = recipe.id
        INNER JOIN accounts ON favorites.account_id = accounts.id
        WHERE favorites.id = LAST_INSERT_ID();";
        // FavoriteRecipe favorite = _db.Query(sql, (Favorite favorite, FavoriteRecipe favoriteRecipe, Account account) =>
        // {
        //     favoriteRecipe.recipe.Creator = account;   
        //     return favoriteRecipe;
        // }, newFavorite).FirstOrDefault();

        FavoriteRecipe newFavoriteRecipe = _db.Query(sql, (Favorite favorite, FavoriteRecipe favoriteRecipe, Account account) =>
        {
            favoriteRecipe.FavoriteId = favorite.Id;
            favoriteRecipe.CreatorId = favorite.AccountId;
            favoriteRecipe.Creator = account;
            return favoriteRecipe;
        }, newFavorite).FirstOrDefault();

        return newFavoriteRecipe;
    }

    internal void DeleteFavorite(int favoriteId)
    {
        string sql = @"
        DELETE FROM favorites
        WHERE id = @FavoriteId;";
        int rowsAffected = _db.Execute(sql, new { FavoriteId = favoriteId });

        if (rowsAffected != 1)
        {
            throw new Exception("Unable to delete favorite");
        }
    }

    internal FavoriteRecipe GetFavoriteByAccountId(int favoriteId)
    {
        string sql = @"
        SELECT favorites.*, recipe.*, accounts.*
        FROM favorites
        INNER JOIN recipe ON favorites.recipe_id = recipe.id
        INNER JOIN accounts ON favorites.account_id = accounts.id
        WHERE favorites.id = @FavoriteId LIMIT 1;";
        FavoriteRecipe favorite = _db.Query(sql, (Favorite favorite, FavoriteRecipe favoriteRecipe, Account account) =>
        {
            favoriteRecipe.FavoriteId = favorite.Id;
            favoriteRecipe.CreatorId = favorite.AccountId;
            favoriteRecipe.Creator = account;
            return favoriteRecipe;
        }, new { FavoriteId = favoriteId }).FirstOrDefault();

        return favorite;
    }

    internal List<FavoriteRecipe> GetFavoritesByAccountId(string accountId)
    {
        string sql = @"
        SELECT favorites.*, recipe.*, accounts.*
        FROM favorites
        INNER JOIN recipe ON favorites.recipe_id = recipe.id
        INNER JOIN accounts ON favorites.account_id = accounts.id
        WHERE favorites.account_id = @AccountId;";
        List<FavoriteRecipe> favorites = _db.Query(sql, (Favorite favorite, FavoriteRecipe favoriteRecipe, Account account) =>
        {
            favoriteRecipe.FavoriteId = favorite.Id;
            favoriteRecipe.CreatorId = favorite.AccountId;
            favoriteRecipe.Creator = account;
            return favoriteRecipe;
        }, new { AccountId = accountId }).ToList();

        return favorites;
    }
}