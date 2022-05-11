using Microsoft.EntityFrameworkCore;
using Pizza_Hut.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class ProductRepository : IProductRepositor
    {
        Context context;
        public ProductRepository(Context _context)
        {
            context = _context;
        }


        public List<Product> GetAll()
        {
            return context.products.ToList();
        }

        public Product GetById(int id)
        {
            return context.products.Include("Category").Include("ProductSizes").Include(p=>p.Comments).FirstOrDefault(C => C.ID == id);
        }

        public int Insert(Product product)
        {
            context.products.Add(product);
            return context.SaveChanges();
        }

        public int Update(int id, Product product)
        {
            Product oldProduct = GetById(id);
            if (oldProduct != null)
            {
                oldProduct.Name = product.Name;
                oldProduct.Photo = product.Photo;
                oldProduct.Description = product.Description;
                oldProduct.CategoryID = product.CategoryID;
                return context.SaveChanges();
            }
            return 0;
        }

        public int Delete(int id)
        {

            Product oldProduct = GetById(id);
            context.products.Remove(oldProduct);
            return context.SaveChanges();
        }

        public List<Product> GetByCategoryID(int id)
        {
            return context.products.Where(c => c.CategoryID == id).Include(p => p.ProductSizes).ToList();
        }
    }
}
