<script setup>
import { AppState } from '@/AppState.js';
import { recipesService } from '@/services/RecipesService.js';
import { Pop } from '@/utils/Pop.js';
import { computed, onMounted, ref, watch } from 'vue';
import RecipeCard from '@/components/RecipeCard.vue';
import { favoritesService } from '@/services/FavoritesService.js';
import { accountService } from '@/services/AccountService.js';
import RecipeModal from '@/components/RecipeModal.vue';
import SearchBar from '@/components/SearchBar.vue';

// import Example from '@/components/Example.vue';


const recipes = computed(() => AppState.recipes);
const account = computed(() => AppState.account);
const favoriteRecipes = computed(() => AppState.favoriteRecipes);

const activeFilter = ref('all');
const searchQuery = ref('');

const filteredRecipes = computed(() => {
  let filtered = recipes.value;
  
  // Filter by category/ownership
  if (activeFilter.value === 'myRecipes') {
    filtered = filtered.filter(recipe => recipe.creatorId === account.value?.id);
  } else if (activeFilter.value === 'favorites') {
    filtered = filtered.filter(recipe => isFavorited(recipe.id));
  }
  
  // Filter by search query
  if (searchQuery.value.trim()) {
    const query = searchQuery.value.toLowerCase();
    filtered = filtered.filter(recipe => 
      recipe.title.toLowerCase().includes(query) ||
      recipe.category.toLowerCase().includes(query)
    );
  }
  
  return filtered;
});

function setFilter(filter) {
  activeFilter.value = filter;
}


onMounted(() => {
  AppState.activeRecipe = null; //REVIEW: Clear active recipe on home page load. Best method?
  getRecipes();
  //  getFavoritesByAccountId();
})

watch(account, (newAccount) => {
  if (newAccount) {
    getFavoritesByAccountId();
  }
}, { immediate: true });


async function getRecipes() {
  try {
    await recipesService.getAllRecipes();
  } catch (error) {
    Pop.toast("There was an error loading recipes", "error");
    console.error(error);
  }
}

async function getFavoritesByAccountId() {
  try {
    if (account?.value) {
      await accountService.getFavoritesByAccount();
    }
  } catch (error) {
    Pop.toast("There was an error loading favorite recipes", "error");
    console.error(error);
  }
}

function isFavorited(recipeId) {
  return favoriteRecipes.value.some(fav => fav.id === recipeId);
}

</script>

<template>
  <div class="container-fluid grains-bg mb-5 position-relative">
    <!-- Search bar positioned top right of hero image -->
    <div class="search-bar-container">
      <SearchBar />
    </div>
    
    <section class="row flex-grow-1">
      <div class="col d-flex flex-column justify-content-center align-items-center text-center">
        <h1 class="text-white">Welcome to All Spice</h1>
        <h2 class="text-white">Cherish Your Family</h2>
        <h2 class="text-white">And Their Cooking</h2>
      </div>
    </section>
    <div class="container-fluid button-group-container">
      <div class="btn-group" role="group">
        <button type="button" class="btn" :class="activeFilter === 'all' ? 'btn-success' : 'btn-light'"
          @click="setFilter('all')">Home</button>
        <button type="button" class="btn" :class="activeFilter === 'myRecipes' ? 'btn-success' : 'btn-light'"
          @click="setFilter('myRecipes')" :disabled="!account" :title="!account ? 'Login to view your recipes' : ''">
          My Recipes
        </button>
        <button type="button" class="btn" :class="activeFilter === 'favorites' ? 'btn-success' : 'btn-light'"
          @click="setFilter('favorites')" :disabled="!account" :title="!account ? 'Login to view favorites' : ''">
          Favorites
        </button>
      </div>
    </div>
  </div>
  <div class="container-fluid">
    <section class="row">
      <div v-for="recipe in filteredRecipes" :key="recipe.id" class="col-md-4 mb-4">
        <!-- {{ recipe.title }} -->
        <RecipeCard :recipe="recipe" :isFavorited="isFavorited(recipe.id)" />
      </div>
    </section>
  </div>
  <button class="create-recipe-fab" data-bs-toggle="modal" data-bs-target="#createRecipeModal" title="Add Recipe">
    <i class="mdi mdi-plus fs-3"></i>
  </button>
  <RecipeModal />
</template>

<style scoped lang="scss">
.create-recipe-fab {
  position: fixed;
  bottom: 30px;
  right: 30px;
  width: 60px;
  height: 60px;
  border-radius: 50%;
  background-color: var(--bs-success);
  border: none;
  color: white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
  cursor: pointer;
  z-index: 1000; //Makes sure it stays on top of other elements (priority level)
  transition: transform 0.2s, box-shadow 0.2s;

  &:hover {
    transform: scale(1.1);
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.4);
  }
}

.grains-bg {
  background-image: url('https://images.unsplash.com/photo-1716816211590-c15a328a5ff0?q=80&w=1646&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D');
  background-size: cover;
  background-position: center;
  height: 40vh;
  color: white;
  text-shadow: 2px 2px 4px #000000;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  position: relative;
}

.search-bar-container {
  position: absolute;
  top: 20px;
  right: 20px;
  width: auto;
  max-width: 500px;
}

.button-group-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: -2rem; // Adjust as needed to overlap the section above
}

.btn-group {
  border-radius: 25px; // Makes it pill-shaped
  // overflow: hidden; // Ensures buttons stay within rounded corners
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2); // Optional shadow for depth
  position: relative;
  bottom: -1em;

  .btn {
    border-radius: 0; // Remove individual button rounding

    &:first-child {
      border-top-left-radius: 25px;
      border-bottom-left-radius: 25px;
    }

    &:last-child {
      border-top-right-radius: 25px;
      border-bottom-right-radius: 25px;
    }
  }
}
</style>
