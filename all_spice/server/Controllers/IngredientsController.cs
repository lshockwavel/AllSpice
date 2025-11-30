namespace all_spice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly IngredientsService _ingredientsService;

    public IngredientsController(Auth0Provider auth0Provider, IngredientsService ingredientsService)
    {
        _auth0Provider = auth0Provider;
        _ingredientsService = ingredientsService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Ingredient>> CreateIngredient([FromBody] Ingredient newIngredient)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            Ingredient createdIngredient = _ingredientsService.CreateIngredient(newIngredient, userInfo);
            return Ok(createdIngredient);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpDelete("{ingredientId}")]
    public async Task<ActionResult<string>> DeleteIngredientsByRecipeId(int ingredientId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            _ingredientsService.DeleteIngredientByIngredientId(ingredientId, userInfo);
            return Ok("Ingredients deleted");
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}