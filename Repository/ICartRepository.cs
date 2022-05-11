using Pizza_Hut.Models;
using System.Collections.Generic;

namespace Pizza_Hut.Repository
{
    public interface ICartRepository : IRepository<Cart, int>
    {
        Cart GetCartByCustomerId(string id);
        bool ProductSizeIsExistInCart(int CartId, int productSizeId);
    }
}