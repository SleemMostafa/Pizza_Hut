using Pizza_Hut.Models;
using System.Collections.Generic;

namespace Pizza_Hut.Repository
{
    public interface IProductSizeOrderRepository:IRepository<ProductSizeOrder,int>
    {
        public List<ProductSizeOrder> UserOrder(int OrderID);
    }
}