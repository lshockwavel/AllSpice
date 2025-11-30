


namespace all_spice.Services;

public class FavoritesService
{
    private readonly FavoritesRepository _repo;

    public FavoritesService(FavoritesRepository repo)
    {
        _repo = repo;
    }

    internal FavoriteRecipe CreateFavorite(Favorite newFavorite)
    {
        FavoriteRecipe favorite = _repo.CreateFavorite(newFavorite);
        return favorite;
    }

    internal void DeleteFavorite(int favoriteId, Account userInfo)
    {
        FavoriteRecipe favorite = _repo.GetFavoriteByAccountId(favoriteId);
        if (favorite == null)
        {
            throw new Exception("Favorite not found");
        }
        if (favorite.CreatorId != userInfo.Id)
        {
            throw new Exception("You are not the owner of this favorite. You Monster!");
        }
        
        _repo.DeleteFavorite(favoriteId);
    }

    internal List<FavoriteRecipe> GetFavorites(string accountId)
    {
        List<FavoriteRecipe> favorites = _repo.GetFavoritesByAccountId(accountId);
        return favorites;
    }
}