using Project_eStudiez.Interface;
using Project_eStudiez.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Project_eStudiez.OtherMethods
{
    public class RepositoryClass<T> : IRepository<T> where T : class
    {
        //1st make context class obj;
        private ModeleStudiez db_context;
        //2nd make IDbSet a Generic table DbSet<>
        private IDbSet<T> tbl_name;
        public RepositoryClass()
        {
            db_context = new ModeleStudiez();
            tbl_name = db_context.Set<T>();
        }
        //3rd implement all Interface methods
        public async Task<int> Create(T obj)
        {
            tbl_name.Add(obj);
            return await Save();
        }

        public async Task<int> Delete(int id)
        {
            T row = await Get_id(id);
            if (row != null)
            {
                tbl_name.Remove(row);
                return await Save();
            }
            return -1;
        }

        public async Task<int> DeleteByRow(T obj)
        {
            if (obj != null)
            {
                tbl_name.Remove(obj);
                return await Save();
            }
            return -1;
        }

        public async Task<T> Get_id(int id)
        {
            return await Task.Run(() => tbl_name.Find(id));
        }

        public async Task<IEnumerable<T>> Read()
        {
            return await tbl_name.ToListAsync();
        }


        public async Task<int> Save()
        {
            return await db_context.SaveChangesAsync();
        }

        public async Task<int> Update(T obj)
        {
            db_context.Entry(obj).State = EntityState.Modified;
            return await Save();
        }
    }
}