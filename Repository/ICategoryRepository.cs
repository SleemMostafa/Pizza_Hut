using Pizza_Hut.Models;
using System.Collections.Generic;

namespace Pizza_Hut.Repository
{
    public interface ICategoryRepository: IRepository<Category,int>
    {
        public Cart CartByCustomerId(string id);
    }
}