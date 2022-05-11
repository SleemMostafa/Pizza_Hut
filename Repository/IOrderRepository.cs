using Pizza_Hut.Models;
using System.Collections.Generic;

namespace Pizza_Hut.Repository
{
    public interface IOrderRepository:IRepository<Order,int>
    {
        List<Order> GetUserOreders(string id);
    }
}