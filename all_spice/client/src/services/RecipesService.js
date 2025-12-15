import { AppState } from "@/AppState.js"
import { Recipe } from "@/models/Recipe.js";
import { api } from "./AxiosService.js";
import { Ingredient } from "@/models/Ingredient.js";


class RecipesService {

    async createRecipe(recipeData) {
        const response = await api.post('api/recipes', recipeData);
        console.log('createRecipe response:', response);
        const newRecipe = new Recipe(response.data);
        AppState.recipes.push(newRecipe);
        return newRecipe;
    }

    async getAllRecipes() {
        const response = await api.get('api/recipes');
        console.log('getAllRecipes response:', response);
        AppState.recipes = response.data.map(responseData => new Recipe(responseData));
    }

    async getRecipeById(recipeId) {
        AppState.activeRecipe = null;
        console.log('Fetching recipe with ID:', recipeId);

        const response = await api.get(`api/recipes/${recipeId}`);
        console.log('getRecipeById response:', response);

        AppState.activeRecipe = new Recipe(response.data);
    }

    async getIngredientsByRecipeId(recipeId) {
        const response = await api.get(`api/recipes/${recipeId}/ingredients`);
        console.log('getIngredientsByRecipeId response:', response);
        AppState.ingredients = response.data.map(ingredientResponse => new Ingredient(ingredientResponse));

        return response.data;
    }

    async updateRecipeById(recipeId, updatedData) {
        const response = await api.put(`api/recipes/${recipeId}`, updatedData);
        console.log('updateRecipeById response:', response);
        const updatedRecipe = new Recipe(response.data);
        const index = AppState.recipes.findIndex(recipe => recipe.id === recipeId);
        if (index !== -1) {
            AppState.recipes.splice(index, 1, updatedRecipe);
        }
        AppState.activeRecipe = updatedRecipe;
        return updatedRecipe;
    }

    async deleteRecipeById(recipeId) {
        const response = await api.delete(`api/recipes/${recipeId}`);
        console.log('deleteRecipeById response:', response);
        AppState.recipes = AppState.recipes.filter(recipe => recipe.id !== recipeId);
    }

    async searchRecipes(name) {
        const response = await api.get(`api/recipes?name=${name}`);
        console.log('searchRecipes response:', response);
        AppState.recipes = response.data.map(recipeData => new Recipe(recipeData));
    }

}

export const recipesService = new RecipesService()