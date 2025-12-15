namespace all_spice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly RecipesService _recipeService;
    private readonly IngredientsService _ingredientsService;

    public RecipesController(Auth0Provider auth0Provider, RecipesService service, IngredientsService ingredientsService)
    {
        _auth0Provider = auth0Provider;
        _recipeService = service;
        _ingredientsService = ingredientsService;
    }

    // [HttpGet("test")]
    // public ActionResult<string> Test()
    // {
    //     return Ok("Recipe Controller is working");
    // }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe newRecipe)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            newRecipe.CreatorId = userInfo.Id;
            Recipe createdRecipe = _recipeService.CreateRecipe(newRecipe);
            return Ok(createdRecipe);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<Recipe>> GetAllRecipes([FromQuery] string name, [FromQuery] string category)
    {
        try
        {
            List<Recipe> recipes;
            //Maybe add the the get all here with the if else statement?
            if (name == null && category == null)
            {
                recipes = _recipeService.GetAllRecipes();
            }
            else
            {
                recipes = _recipeService.GetRecipes(name, category);
            }
            return Ok(recipes);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    // [HttpGet]
    // public ActionResult<List<Recipe>> GetAllRecipes()
    // {
    //     try
    //     {
    //         List<Recipe> recipes = _recipeService.GetAllRecipes();
    //         return Ok(recipes);
    //     }
    //     catch (Exception exception)
    //     {
    //         return BadRequest(exception.Message);
    //     }
    // }

    [HttpGet("{recipeId}")]
    public ActionResult<Recipe> GetRecipeById(int recipeId)
    {
        try
        {
            Recipe recipe = _recipeService.GetRecipeById(recipeId);
            return Ok(recipe);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpPut("{recipeId}")]
    public async Task<ActionResult<Recipe>> EditRecipe(int recipeId, [FromBody] Recipe updatedRecipe)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            updatedRecipe.Id = recipeId;
            Recipe recipe = _recipeService.EditRecipe(recipeId, updatedRecipe, userInfo);
            return Ok(recipe);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpDelete("{recipeId}")]
    public async Task<ActionResult<string>> DeleteRecipe(int recipeId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            _recipeService.DeleteRecipe(recipeId, userInfo);
            return Ok("Recipe deleted successfully");
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{recipeId}/ingredients")]
    public ActionResult<List<Ingredient>> GetIngredientsByRecipeId(int recipeId)
    {
        try
        {
            List<Ingredient> ingredients = _ingredientsService.GetIngredientsByRecipeId(recipeId);
            return Ok(ingredients);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}