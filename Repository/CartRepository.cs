using Microsoft.EntityFrameworkCore;
using Pizza_Hut.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class CartRepository : ICartRepository
    {
        Context context;
        public CartRepository(Context _context)
        {
            context= _context;
        }


        public List<Cart> GetAll()
        {
            return context.carts.ToList();
        }

        public Cart GetById(int id)
        {
            return context.carts.FirstOrDefault(C => C.ID == id);
        }

        public int Insert(Cart cart)
        {
            context.carts.Add(cart);
            return context.SaveChanges();
        }

        public int Update(int id, Cart cart)
        {
            Cart oldCart = GetById(id);
            if (oldCart != null)
            {
                oldCart.TotalPrice = cart.TotalPrice;
                oldCart.CustomerID = cart.CustomerID;
                return context.SaveChanges();
            }
            return 0;
        }

        public int Delete(int id)
        {
            Cart oldCart = GetById(id);
            context.carts.Remove(oldCart);
            return context.SaveChanges();
        }
        public Cart GetCartByCustomerId(string id)
        {
            return context.carts.AsSplitQuery().Include(c=>c.ProductSizeCarts).ThenInclude(p=>p.ProductSize).ThenInclude(p=>p.Product).FirstOrDefault(c=> c.CustomerID == id);    
        }
        public bool ProductSizeIsExistInCart(int CartId, int productSizeId)
        {
            var cart = context.carts.Include(p=>p.ProductSizeCarts).FirstOrDefault(c => c.ID == CartId);
            foreach (var item in cart.ProductSizeCarts)
            {
                if(item.ProductSizeID==productSizeId)
                    return true;
            }
            return false;
        }
    }
}
