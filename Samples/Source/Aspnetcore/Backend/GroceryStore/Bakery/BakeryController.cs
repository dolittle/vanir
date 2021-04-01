using System.Collections.Generic;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.GraphQL;

namespace Backend.GroceryStore.Bakery
{
    [GraphRoot("groceryStore/bakery")]
    public class BakeryController : GraphController
    {
        [Query("pastries/search")]
        public async Task<IEnumerable<Danish>> SearchPastries(string nameLike)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            return new Danish[] {
                new Danish()
            };
        }

        [Query("pastries/recipe")]
        public async Task<Recipe> RetrieveRecipe(RecipeId id)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            return new Recipe();
        }

        [Query("breadCounter/orders")]
        public async Task<IEnumerable<BreadOrder>> FindOrders(OrderId id)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            return new BreadOrder[] {
                new BreadOrder()
            };
        }
    }
}
