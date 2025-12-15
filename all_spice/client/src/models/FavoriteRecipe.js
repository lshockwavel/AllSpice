import { Favorite } from "./Favorite.js"
import { Recipe } from "./Recipe.js"


export class FavoriteRecipe extends Recipe {
    constructor(data) {
        super(data)
        this.favoriteId = data.favoriteId
    }
}