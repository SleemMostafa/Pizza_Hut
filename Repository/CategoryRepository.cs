using Pizza_Hut.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Pizza_Hut.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        Context context;
        public CategoryRepository(Context _context)
        {
            context = _context;
        }
        public List<Category> GetAll()
        {
            var query = context.categories.ToList();
            return query;
        }
        public Category GetById(int id)
        {
            return context.categories.FirstOrDefault(c => c.ID == id);
        }
        public int Insert(Category category)
        {
            context.categories.Add(category);
            return context.SaveChanges();
        }

        public int Update(int id, Category category)
        {
            Category oldCategory = GetById(id);
            if (oldCategory != null)
            {
                oldCategory.Name = category.Name;
                oldCategory.Description = category.Description;
                oldCategory.Image = category.Image;
                return context.SaveChanges();
            }
            return 0;
        }

        public int Delete(int id)
        {
            Category OldCategory = GetById(id);
            context.categories.Remove(OldCategory);
            return context.SaveChanges();
        }
        public Cart CartByCustomerId(string id)
        {
            var query = context.carts.FirstOrDefault(c => c.CustomerID == id);
            return query;
        }
    }
}