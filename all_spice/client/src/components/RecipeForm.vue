<script setup>
import { AppState } from '@/AppState.js';
import { ref, watch } from 'vue';

const props = defineProps({
    recipe: { type: Object, default: null }
});

const emit = defineEmits(['submit']);

const recipeForm = ref({
    title: '',
    category: 'breakfast',
    instructions: '',
    img: ''
});

// Watch for recipe prop changes (for edit mode)
watch(() => AppState.activeRecipe, (newRecipe) => {
    if (newRecipe) {
        recipeForm.value = {
            title: newRecipe.title,
            category: newRecipe.category,
            instructions: newRecipe.instructions,
            img: newRecipe.img
        };
    } else {
        resetForm();
    }
}, { immediate: true });

function resetForm() {
    recipeForm.value = {
        title: '',
        category: 'breakfast',
        instructions: '',
        img: ''
    };
}

function handleSubmit() {
    emit('submit', recipeForm.value);
}

</script>


<template>
    <form @submit.prevent="handleSubmit">
        <div class="mb-3">
            <label class="form-label">Recipe Title</label>
            <input v-model="recipeForm.title" type="text" class="form-control" required>
        </div>
        <div class="mb-3">
            <label class="form-label">Category</label>
            <select v-model="recipeForm.category" class="form-select" required>
                <option value="breakfast">Breakfast</option>
                <option value="lunch">Lunch</option>
                <option value="dinner">Dinner</option>
                <option value="dessert">Dessert</option>
                <option value="snack">Snack</option>
            </select>
        </div>
        <div class="mb-3">
            <label class="form-label">Image URL</label>
            <input v-model="recipeForm.img" type="url" class="form-control" required>
        </div>
        <div class="mb-3">
            <label class="form-label">Instructions</label>
            <textarea v-model="recipeForm.instructions" class="form-control" rows="5" required></textarea>
        </div>
        <slot name="actions">
            <button type="submit" class="btn btn-success">Submit</button>
        </slot>
    </form>
</template>


<style lang="scss" scoped></style>