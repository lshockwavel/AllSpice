<script setup>
import { recipesService } from '@/services/RecipesService.js';
import { Pop } from '@/utils/Pop.js';
import { Modal } from 'bootstrap';
import RecipeForm from './RecipeForm.vue';
import { computed } from 'vue';

const props = defineProps({
    recipe: { type: Object, default: null },
    isEdit: { type: Boolean, default: false }
});

const emit = defineEmits(['submit']);

const modalId = computed(() => props.isEdit ? 'editRecipeModal' : 'createRecipeModal');
const modalTitle = computed(() => props.isEdit ? 'Edit Recipe' : 'Create New Recipe');
const submitButtonText = computed(() => props.isEdit ? 'Update Recipe' : 'Create Recipe');
const submitButtonClass = computed(() => props.isEdit ? 'btn-primary' : 'btn-success');

async function handleSubmit(recipeData) {
    try {
        if (props.isEdit) {
            // If parent passed @submit handler, use it
            emit('submit', recipeData);
        } else {
            // Create mode
            await recipesService.createRecipe(recipeData);
            Pop.success('Recipe created!');
            await recipesService.getAllRecipes();
        }
        Modal.getOrCreateInstance(`#${modalId.value}`).hide();
    } catch (error) {
        Pop.error(props.isEdit ? 'Failed to update recipe' : 'Failed to create recipe');
        console.error(error);
    }
}

</script>


<template>
    <div class="modal fade" :id="modalId" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" :class="isEdit ? 'bg-primary text-white' : 'bg-success text-white'">
                    <h5 class="modal-title">{{ modalTitle }}</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <RecipeForm :recipe="recipe" @submit="handleSubmit">
                        <template #actions>
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                            <button type="submit" class="btn" :class="submitButtonClass">{{ submitButtonText }}</button>
                        </template>
                    </RecipeForm>
                </div>
            </div>
        </div>
    </div>
</template>


<style lang="scss" scoped></style>