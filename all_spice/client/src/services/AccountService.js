import { FavoriteRecipe } from '@/models/FavoriteRecipe.js'
import { AppState } from '../AppState.js'
import { Account } from '../models/Account.js'
import { logger } from '../utils/Logger.js'
import { api } from './AxiosService.js'

class AccountService {
  async getAccount() {
    try {
      const res = await api.get('/account')
      AppState.account = new Account(res.data)
    } catch (err) {
      logger.error('HAVE YOU STARTED YOUR SERVER YET???', err)
    }
  }

  // async GetFavoritesByAccount(accountId) {
  //   const response = await api.get(`api/accounts/${accountId}/favorites`);
  //   console.log('GetFavoritesByAccount response:', response);
  //   AppState.favoriteRecipes = response.data.map(favData => new FavoriteRecipe(favData));
  // }

  async getFavoritesByAccount() {
        const response = await api.get(`account/favorites`);
        console.log('getFavoritesByAccountId response:', response);
        AppState.favoriteRecipes = response.data.map(favData => new FavoriteRecipe(favData));
    }
}

export const accountService = new AccountService()
