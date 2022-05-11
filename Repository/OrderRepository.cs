using Pizza_Hut.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class OrderRepository : IOrderRepository
    {
        Context context;
        public OrderRepository(Context _context)
        {
            context = _context;
        }
        public List<Order> GetAll()
        {
            var query = context.orders.ToList();
            return query;
        }
        public Order GetById(int id)
        {
            var query = context.orders.FirstOrDefault(o => o.ID == id);
            return query;
        }
        public int Insert(Order order)
        {
            context.orders.Add(order);
            return context.SaveChanges();
        }
        public int Update(int id, Order orderNew)
        {
            var query = GetById(id);
            if (query != null)
            {
                query.CustomerID = orderNew.CustomerID;
                query.Date = orderNew.Date;
                query.Location = orderNew.Location;
                query.TotalPrice = orderNew.TotalPrice;
                return context.SaveChanges();
            }
            return 0;
        }
        public int Delete(int id)
        {
            var query = GetById(id);
            if (query != null)
            {
                context.orders.Remove(query);
                return context.SaveChanges();
            }
            return 0;
        }

        public List<Order> GetUserOreders(string id)
        {
            return context.orders.Where(o => o.CustomerID == id).ToList();
        }
    }
}
