using Microsoft.EntityFrameworkCore;
using Pizza_Hut.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class ProductSizeOrderRepository : IProductSizeOrderRepository
    {
        Context context;
        public ProductSizeOrderRepository(Context _context)
        {
            context = _context;
        }
        public List<ProductSizeOrder> GetAll()
        {
            var query = context.productSizeOrders.ToList();
            return query;
        }
        public ProductSizeOrder GetById(int id)
        {
            var query = context.productSizeOrders.FirstOrDefault(p => p.ID == id);
            return query;
        }
        public int Insert(ProductSizeOrder productSizeOrder)
        {
            context.productSizeOrders.Add(productSizeOrder);
            return context.SaveChanges();
        }
        public int Update(int id, ProductSizeOrder productSizeOrderNew)
        {
            var query = GetById(id);
            if (query != null)
            {
                query.Quantity = productSizeOrderNew.Quantity;
                query.OrderID = productSizeOrderNew.OrderID;
                query.ProductSizeID = productSizeOrderNew.ProductSizeID;
                return context.SaveChanges();
            }
            return 0;
        }
        public int Delete(int id)
        {
            var query = GetById(id);
            if (query != null)
            {
                context.productSizeOrders.Remove(query);
                return context.SaveChanges();
            }
            return 0;
        }

        public List<ProductSizeOrder> UserOrder(int OrderID)
        {
            var productSizes = context.productSizeOrders.Include(ps => ps.ProductSize)
                .ThenInclude(pro => pro.Product)
                .Where(p => p.OrderID == OrderID).ToList();
            return productSizes;
        }
    }
}
