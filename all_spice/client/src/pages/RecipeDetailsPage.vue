<script setup>
import { recipesService } from '@/services/RecipesService.js';
import { AppState } from '../AppState.js';
import { computed, ref, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { Pop } from '@/utils/Pop.js';
import { accountService } from '@/services/AccountService.js';
import RecipeModal from '@/components/RecipeModal.vue';
import { favoritesService } from '@/services/FavoritesService.js';
import { ingredientsService } from '@/services/IngredientsService.js';

const activeRecipe = computed(() => AppState.activeRecipe);
const favoriteRecipes = computed(() => AppState.favoriteRecipes);
const ingredients = computed(() => AppState.ingredients);
const account = computed(() => AppState.account);

const isOwner = computed(() => {
    return activeRecipe.value?.creatorId === account.value?.id;
});

const newIngredient = ref({
    name: '',
    quantity: ''
});

const route = useRoute();
const router = useRouter(); // a Tool to change router?

onMounted(() => {
    getRecipeById();
    getIngredientsByRecipeId();
    getFavoritesByAccountId();
    // Logic to fetch or set the active recipe can be added here
});

watch(route, () => {
    getRecipeById();
    getIngredientsByRecipeId();
}, { immediate: true });

// watch(favoriteRecipes, () => {
//     getRecipeById();
// }, { immediate: true });

async function getRecipeById() {
    try {
        const recipeId = route.params.recipeId;
        await recipesService.getRecipeById(recipeId);
    } catch (error) {
        Pop.toast("There was an error loading the recipe", "error");
        console.error("There was an error loading the recipe", error);
    }
}

async function getIngredientsByRecipeId() {
    try {
        const recipeId = route.params.recipeId;
        await recipesService.getIngredientsByRecipeId(recipeId);
    } catch (error) {
        Pop.toast("There was an error loading the ingredients", "error");
        console.error("There was an error loading the ingredients", error);
    }
}

async function getFavoritesByAccountId() {
    try {
        await accountService.getFavoritesByAccount();
    } catch (error) {
        Pop.toast("There was an error loading the ingredients", "error");
        console.error("There was an error loading the ingredients", error);
    }
}

function isFavorited() {
    return  favoriteRecipes.value.some(fav => fav.id === activeRecipe.value?.id);
}

async function deleteRecipe() {
    try {
        const recipeId = route.params.recipeId;
        await recipesService.deleteRecipeById(recipeId);
        Pop.success('Recipe deleted!');
        router.push({ name: 'Home' });
    } catch (error) {
        Pop.error('Failed to delete recipe');
        console.error(error);
    }
}

async function updateRecipe(updatedData) {
    try {
        const recipeId = route.params.recipeId;
        await recipesService.updateRecipeById(recipeId, updatedData);
        Pop.success('Recipe updated!');
        await getRecipeById(); // Refresh the active recipe data
    } catch (error) {
        Pop.error('Failed to update recipe');
        console.error(error);
    }
}


async function toggleFavorite() {
    try {
        const recipeId = route.params.recipeId;
        if (isFavorited()) {
            const favorite = favoriteRecipes.value.find(fav => fav.id === activeRecipe.value?.id); //Getting the favorite entry for the ID
            console.log('Removing favorite with ID:', favorite.favoriteId);
            await favoritesService.removeFavorite(favorite.favoriteId);
            Pop.success('Removed from favorites');
        } else {
            await favoritesService.addFavorite(recipeId);
            Pop.success('Added to favorites');
        }
        await getFavoritesByAccountId(); // Refresh favorites
        await getRecipeById(); // Refresh recipe to update favoriteCount
    } catch (error) {
        Pop.error('Failed to update favorites');
        console.error(error);
    }
}

async function addIngredient() {
    try {
        const recipeId = route.params.recipeId;
        const ingredientData = {
            name: newIngredient.value.name,
            quantity: newIngredient.value.quantity,
            recipeId: recipeId
        };
        await ingredientsService.createIngredient(ingredientData);
        Pop.success('Ingredient added!');
        newIngredient.value = { name: '', quantity: '' }; // Reset form
        await getIngredientsByRecipeId(); // Refresh ingredients list
    } catch (error) {
        Pop.error('Failed to add ingredient');
        console.error(error);
    }
}

async function deleteIngredient(ingredientId) {
    try {
        const confirmed = await Pop.confirm('Delete this ingredient?');
        if (!confirmed) return;
        
        await ingredientsService.deleteIngredient(ingredientId);
        Pop.success('Ingredient deleted!');
        await getIngredientsByRecipeId(); // Refresh ingredients list
    } catch (error) {
        Pop.error('Failed to delete ingredient');
        console.error(error);
    }
}

const categoryColors = {
    breakfast: '#FFA726', // Orange
    lunch: '#42A5F5',     // Blue
    dinner: '#EF5350',    // Red
    dessert: '#AB47BC',   // Purple
    snack: '#66BB6A'      // Green
};

const categoryColor = computed(() => {
    return categoryColors[activeRecipe.value?.category] || '#757575'; // Default gray
});


</script>


<template>
    <div v-if="activeRecipe" class="container-fluid mt-4">
        <section class="row">
            <div class="col-md-6 position-relative">
                <img :src="activeRecipe.img" :alt="activeRecipe.title" class="show-food" />
                <div class="favorite-badge">
                    <i class="mdi" :class="isFavorited() ? 'mdi-heart' : 'mdi-heart-outline'" @click="toggleFavorite" style="cursor: pointer;"></i>
                    <span>{{ activeRecipe.favoriteCount }}</span>
                </div>
            </div>
            <div class="col-md-6">
                <section class="row">
                    <div class="col-md-12 d-flex justify-content-between align-items-center">
                        <h1 class="text-green">{{ activeRecipe.title }}</h1>
                        
                        <!-- Three dots dropdown menu (only if owner) -->
                        <div v-if="isOwner" class="dropdown">
                            <button class="btn btn-link text-dark" type="button" data-bs-toggle="dropdown"
                            aria-expanded="false">
                            <i class="mdi mdi-dots-vertical fs-3"></i>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end">
                                <li>
                                    <button class="dropdown-item" data-bs-toggle="modal"
                                        data-bs-target="#editRecipeModal">
                                        <i class="mdi mdi-pencil"></i> Edit Recipe
                                    </button>
                                </li>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>
                                <li>
                                    <button class="dropdown-item text-danger" @click="deleteRecipe">
                                        <i class="mdi mdi-delete"></i> Delete Recipe
                                    </button>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <p>By {{ activeRecipe.creator.name }}</p>
                    </div>
                    <div class="col-md-12">
                        <div class="category-badge" :style="{ backgroundColor: categoryColor }">{{ activeRecipe.category }}</div>
                    </div>
                    <div class="col-md-12">
                        <h2>Ingredients:</h2>
                        <p v-if="!ingredients.length && !isOwner">No ingredients available for this recipe. Good Luck figuring it out!</p>
                        
                        <ul class="ingredient-list">
                            <li v-for="ingredient in ingredients" :key="ingredient.id" class="ingredient-item">
                                <span class="ingredient-text">{{ ingredient.quantity }} {{ ingredient.name }}</span>
                                <button v-if="isOwner" @click="deleteIngredient(ingredient.id)" class="btn btn-sm btn-danger delete-btn">
                                    <i class="mdi mdi-delete"></i>
                                </button>
                            </li>
                        </ul>
                        
                        <!-- Add ingredient form (only for owner) -->
                        <div v-if="isOwner" class="add-ingredient-form">
                            <form @submit.prevent="addIngredient" class="ingredient-form">
                                <input 
                                    v-model="newIngredient.quantity" 
                                    type="text" 
                                    class="form-control form-control-sm quantity-input" 
                                    placeholder="Quantity (e.g., 2 cups)"
                                    required
                                >
                                <input 
                                    v-model="newIngredient.name" 
                                    type="text" 
                                    class="form-control form-control-sm name-input" 
                                    placeholder="Ingredient name"
                                    required
                                >
                                <button type="submit" class="btn btn-sm btn-success add-btn">
                                    <i class="mdi mdi-plus"></i> Add
                                </button>
                            </form>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <h2>Instructions:</h2>
                        <p>{{ activeRecipe.instructions }}</p>
                    </div>
                </section>
            </div>
        </section>
    </div>
    
    <!-- Edit Recipe Modal (outside of dropdown) -->
    <RecipeModal :recipe="activeRecipe" :isEdit="true" @submit="updateRecipe" />
</template>


<style lang="scss" scoped>
.show-food {
    width: 100%;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.position-relative {
    position: relative; // Establishes positioning context
}

.favorite-badge {
    position: absolute;
    top: 15px;
    left: 15px;
    background-color: rgba(255, 255, 255, 0.9);
    padding: 8px 12px;
    border-radius: 20px;
    display: flex;
    align-items: center;
    gap: 5px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    font-weight: bold;

    i {
        color: #e74c3c; // Red heart color
        font-size: 1.2rem;
    }
}

// Ingredients section styles
.ingredient-list {
    list-style: none;
    padding-left: 0;
}

.ingredient-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 8px 0;
    border-bottom: 1px solid #e0e0e0;

    &:last-child {
        border-bottom: none;
    }
}

.ingredient-text {
    flex: 1;
}

.delete-btn {
    margin-left: 10px;
}

.add-ingredient-form {
    margin-top: 20px;
    padding-top: 15px;
    border-top: 2px solid #dee2e6;
}

.ingredient-form {
    display: flex;
    gap: 10px;
    align-items: center;
}

.quantity-input {
    max-width: 150px;
}

.name-input {
    flex: 1;
}

.add-btn {
    white-space: nowrap;
}

.category-badge {
    display: inline-block;
    color: white;
    padding: 5px 12px;
    border-radius: 15px;
    font-size: 0.85rem;
    font-weight: 600;
    text-transform: capitalize;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    margin-bottom: 15px;
}

</style>