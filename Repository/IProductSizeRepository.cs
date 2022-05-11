using Pizza_Hut.Models;
using System.Collections.Generic;

namespace Pizza_Hut.Repository
{
    public interface IProductSizeRepository:IRepository<ProductSize,int>
    {
        public List<ProductSize> productSizesForProduct(int id);
        public ProductSize GetProductSize(int id, Size size);
        public int DeleteSizeProduct(int id);
    }
}
