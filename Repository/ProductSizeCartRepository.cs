using Microsoft.EntityFrameworkCore;
using Pizza_Hut.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class ProductSizeCartRepository : IProductSizeCartRepository
    {
        Context context;
        public ProductSizeCartRepository(Context _context)
        {
            context = _context;
        }
        public List<ProductSizeCart> GetAll()
        {
            return context.ProductSizeCarts.ToList();
        }

        public ProductSizeCart GetById(int id)
        {
            return context.ProductSizeCarts.Include(c=>c.Cart).Include(p=>p.ProductSize).FirstOrDefault(P => P.ID == id);
        }

        public int Insert(ProductSizeCart product)
        {
            context.ProductSizeCarts.Add(product);
            return context.SaveChanges();
        }

        public int Update(int id, ProductSizeCart product)
        {
            ProductSizeCart oldProduct = GetById(id);
            if (oldProduct != null)
            {
                oldProduct.Quantity = product.Quantity;
                oldProduct.ProductSizeID = product.ProductSizeID;
                oldProduct.CartID = product.CartID;
                return context.SaveChanges();
            }
            return 0;
        }

        public int Delete(int id)
        {
            ProductSizeCart oldProduct = GetById(id);
            context.ProductSizeCarts.Remove(oldProduct);
            return context.SaveChanges();
        }
    }
}
