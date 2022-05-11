using System.Collections.Generic;

namespace Pizza_Hut.Repository
{
    public interface IRepository<T1,T2>
    {
        T1 GetById(T2 id);
        List<T1> GetAll();
        int Insert(T1 entity);
        int Update(T2 id, T1 entity);
        int Delete(T2 id);
       
    }
}
