using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer
{
    /// <summary>
    /// General Crud interface to access a db
    /// </summary>
    /// <typeparam name="T">table of the db</typeparam>
    public interface ICrud<T>
    {
        /// <summary>
        /// Insert a new element of type T in the db
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The number of rows inserted in the db</returns>
        public Task<int> Create(T element);//Create
        public Task<List<T>> GetAllAsync();     //Read
        public Task<T> GetById(int id);
        public Task<int> UpdateAsync(T element);  //Update
        public Task<int> DeleteAsync(int elementId);     //Delete
    }
}
