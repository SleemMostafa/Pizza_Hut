using Microsoft.EntityFrameworkCore;
using Pizza_Hut.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        Context context;
        public ProductSizeRepository(Context _context)
        {
            context = _context;
        }
        public int Delete(int id)
        {
            
            try
            {
                ProductSize productSize = context.productSizes.Find(id);
                context.productSizes.Remove(productSize);
                return SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int DeleteSizeProduct(int id)
        {
            var query = context.productSizes.Where(p => p.ProductID == id);
            foreach (var item in query)
            {
                context.productSizes.Remove(item);
            }
            return context.SaveChanges();

        }

        public List<ProductSize> GetAll()
        {
            return context.productSizes.ToList();
        }

        public List<ProductSize> GetById(int id)
        {
            return context.productSizes.Where(p => p.ProductID == id).ToList();
        }

        public ProductSize GetProductSize(int id, Size size)
        {
           var query = context.productSizes.Include(p=>p.Product).FirstOrDefault(p => p.ProductID == id && p.size == size);
            return query;
        }

        public int Insert(ProductSize entity)
        {
            try
            {
                if (entity != null)
                {
                    context.productSizes.Add(entity);
                    return SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<ProductSize> productSizesForProduct(int id)
        {
            if(id != 0)
            {
                return context.productSizes.Where(p => p.ProductID == id).ToList();
            }
            return null;
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public int Update(int id, ProductSize entity)
        {
            try
            {
                if (entity != null)
                {
                    ProductSize productSizeOld = context.productSizes.FirstOrDefault(p => p.ID == id);
                    productSizeOld.size = entity.size;
                    productSizeOld.ProductID = entity.ProductID;
                    productSizeOld.Price = entity.Price;
                    return SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        ProductSize IRepository<ProductSize, int>.GetById(int id)
        {
            throw new NotImplementedException();
        }
        //ProductSize IRepository<ProductSize>.GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
