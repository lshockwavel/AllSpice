

export class Ingredient {
    constructor(data) {
        this.id = data.id
        this.name = data.name
        this.createdAt = new Date(data.createdAt)
        this.updatedAt = new Date(data.updatedAt)
        this.quantity = data.quantity
        this.recipeId = data.recipeId
        // TODO add additional properties if needed
    }
}