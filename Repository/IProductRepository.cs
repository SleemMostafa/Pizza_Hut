using Pizza_Hut.Models;
using System.Collections.Generic;

namespace Pizza_Hut.Repository
{
    public interface IProductRepositor:IRepository<Product,int>
    {
        List<Product> GetByCategoryID(int id);
    }
}