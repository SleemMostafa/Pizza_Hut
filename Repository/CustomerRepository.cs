using Pizza_Hut.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pizza_Hut.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Context context;

        public CustomerRepository(Context _context)
        {
            context = _context;
        }
        public int Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public List<Customer> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Customer GetById(string id)
        {
            var query = context.customers.FirstOrDefault(c => c.Id == id);
            return query;
        }

        public int Insert(Customer entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(string id, Customer entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
