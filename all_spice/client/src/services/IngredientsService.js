import { api } from "./AxiosService.js";



class IngredientsService {
    async createIngredient(ingredientData) {
        const response = await api.post('api/ingredients', ingredientData);
        console.log('createIngredient response:', response);
        return response.data;
    }

    async deleteIngredient(ingredientId) {
        const response = await api.delete(`api/ingredients/${ingredientId}`);
        console.log('deleteIngredient response:', response);
        
    }
}

export const ingredientsService = new IngredientsService()