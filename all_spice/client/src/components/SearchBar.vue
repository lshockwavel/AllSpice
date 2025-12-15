<script setup>
import { AppState } from '@/AppState.js';
import { recipesService } from '@/services/RecipesService.js';
import { Pop } from '@/utils/Pop.js';
import { ref } from 'vue';


const searchTerm = ref('');


async function searchRecipes()
{
    try {
        // Implement search logic here, e.g., call a service to fetch search results
        console.log('Searching for recipes...', searchTerm.value);

        await recipesService.searchRecipes(searchTerm.value);
        searchTerm.value = '';

    } catch (error) {
        Pop.toast('Error searching recipes', 'error');
        console.error('Error searching recipes:', error);
    }
}

async function clearSearch()
{
    try {
        console.log('Clearing search...');
        await recipesService.getAllRecipes();
        searchTerm.value = '';
    } catch (error) {
        Pop.toast('Error clearing search', 'error');
        console.error('Error clearing search:', error);
    }
}


</script>


<template>
    <form @submit.prevent="searchRecipes" class="d-flex" role="search">
        <input v-model="searchTerm" class="form-control me-2" type="search" placeholder="Search Recipes">
        <button class="btn btn-success me-2" type="submit">Search</button>
        <button class="btn btn-danger" type="button" @click="clearSearch">Clear</button>
    </form>
</template>


<style lang="scss" scoped>

</style>