<script setup>
import { Favorite } from '@/models/Favorite.js';
import { FavoriteRecipe } from '@/models/FavoriteRecipe.js';
import { Recipe } from '@/models/Recipe.js';
import { computed } from 'vue';

const props = defineProps({
    recipe: {type: Recipe, required: true},
    isFavorited: {type: Boolean, required: false}
});

const categoryColors = {
    breakfast: '#FFA726', // Orange
    lunch: '#42A5F5',     // Blue
    dinner: '#EF5350',    // Red
    dessert: '#AB47BC',   // Purple
    snack: '#66BB6A'      // Green
};

const categoryColor = computed(() => {
    return categoryColors[props.recipe.category] || '#757575'; // Default gray
});

</script>


<template>
    <RouterLink :to="{ name: 'RecipeDetails', params: { recipeId: recipe.id } }" class="text-decoration-none text-dark">
        <div class="card">
            <img :src="recipe.img" class="card-img-top" :alt="recipe.title">
            
            <!-- Category badge top left -->
            <div class="category-badge" :style="{ backgroundColor: categoryColor }">
                {{ recipe.category }}
            </div>
            
            <!-- Heart icon top right -->
            <div class="heart-icon">
                <i v-if="isFavorited" class="mdi mdi-heart"></i>
                <i v-else class="mdi mdi-heart-outline"></i>
            </div>
            
            <div class="card-body">
                <h5 class="card-title">{{ recipe.title }}</h5>
            </div>
        </div>
    </RouterLink>
</template>


<style lang="scss" scoped>

.card {
    position: relative;
    height: 100%;
    display: flex;
    flex-direction: column;
}

.card-img-top {
    height: 350px;
    object-fit: cover;
}

.card-body {
    flex-grow: 1;
}

.category-badge {
    position: absolute;
    top: 10px;
    left: 10px;
    color: white;
    padding: 5px 12px;
    border-radius: 15px;
    font-size: 0.85rem;
    font-weight: 600;
    text-transform: capitalize;
    z-index: 10;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}

.heart-icon {
    position: absolute;
    top: 10px;
    right: 10px;
    font-size: 1.5rem;
    z-index: 10;
    
    .mdi-heart {
        color: #e74c3c; // Filled red heart
    }
    
    .mdi-heart-outline {
        color: white; // Outlined white heart
        text-shadow: 0 0 3px rgba(0, 0, 0, 0.5); // Shadow so it shows on any background
    }
}

</style>