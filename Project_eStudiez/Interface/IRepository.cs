using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_eStudiez.Interface
{
    internal interface IRepository<T> where T : class
    {
        Task<int> Create(T obj);
        Task<int> Update(T obj);
        Task<IEnumerable<T>> Read();
        Task<int> Delete(int id);
        Task<int> DeleteByRow(T obj);
        //For SaveChanges
        Task<int> Save();
        //Getting id and return a whole row/obj
        Task<T> Get_id(int id);
    }
}
