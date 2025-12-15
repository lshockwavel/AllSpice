import { AppState } from "@/AppState.js";
import { Favorite } from "@/models/Favorite.js";
import { FavoriteRecipe } from "@/models/FavoriteRecipe.js";
import { api } from "./AxiosService.js";



class FavoritesService {
    // async getFavoritesByAccount() {
    //     const response = await api.get(`api/accounts/favorites`);
    //     console.log('getFavoritesByAccountId response:', response);
    //     AppState.favoriteRecipes = response.data.map(favData => new FavoriteRecipe(favData));
    // }

    async addFavorite(recipeId) {
        const response = await api.post('api/favorites', { recipeId });
        console.log('addFavorite response:', response);
        const newFavorite = new FavoriteRecipe(response.data);
        AppState.favoriteRecipes.push(newFavorite);
    }

    async removeFavorite(favoriteId) {
        const response = await api.delete(`api/favorites/${favoriteId}`);
        console.log('removeFavorite response:', response);
        AppState.favoriteRecipes = AppState.favoriteRecipes.filter(fav => fav.id !== favoriteId);
    }

}

export const favoritesService = new FavoritesService();