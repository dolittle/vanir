using System.Collections.Generic;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.GraphQL;

namespace Backend.GroceryStore.Bakery
{
    [GraphRoot("groceryStore/bakery")]
    public class BakeryController : GraphController
    {
        [Mutation("pastries/search")]
        public async Task<bool> BuyPastry(string nameLike)
        {
            await Task.CompletedTask;
            return true;
        }

        [Query("pastries/search")]
        public async Task<IEnumerable<Danish>> SearchPastries(string nameLike)
        {
            await Task.CompletedTask;

            return new Danish[] {
                new Danish()
            };
        }

        [Query("pastries/recipe")]
        public async Task<Recipe> RetrieveRecipe(RecipeId id)
        {
            await Task.CompletedTask;

            return new Recipe();
        }

        [Query("breadCounter/orders")]
        public async Task<IEnumerable<BreadOrder>> FindOrders(OrderId id)
        {
            await Task.CompletedTask;

            return new BreadOrder[] {
                new BreadOrder()
            };
        }
    }
}
