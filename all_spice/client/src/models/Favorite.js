

export class Favorite {
    constructor(data) {
        this.id = data.id
        this.createdAt = new Date(data.createdAt)
        this.updatedAt = new Date(data.updatedAt)
        this.accountId = data.accountId
        this.recipeId = data.recipeId
        // TODO add additional properties if needed
    }       
}