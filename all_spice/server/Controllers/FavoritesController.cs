namespace all_spice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly FavoritesService _favoritesService;

    public FavoritesController(Auth0Provider auth0Provider, FavoritesService favoritesService)
    {
        _auth0Provider = auth0Provider;
        _favoritesService = favoritesService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<FavoriteRecipe>> CreateFavorite([FromBody] Favorite newFavorite)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            newFavorite.AccountId = userInfo.Id;
            FavoriteRecipe createdFavorite = _favoritesService.CreateFavorite(newFavorite);
            return Ok(createdFavorite);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpDelete("{favoriteId}")]
    public async Task<ActionResult<string>> DeleteFavorite(int favoriteId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            _favoritesService.DeleteFavorite(favoriteId, userInfo);
            return Ok("Favorite deleted");
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}