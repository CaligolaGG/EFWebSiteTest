using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer
{
    public abstract class Crud<T> : ICrud<T>
    {
        private MyDbContext _ctx;
        public Crud(MyDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<int> Create(T element)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int elementId)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(T element)
        {
            throw new NotImplementedException();
        }


    }
}
