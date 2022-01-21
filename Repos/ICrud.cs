using System;
using System.Collections.Generic;

namespace Repos
{
    /// <summary>
    /// Generic Interface for Crud operations
    /// </summary>
    public interface ICrud<T>
    {
        void Insert(T entity);
        List<T> GetAll();
        T GetById(Guid id);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(Guid id);
        void DeleteAll();

    }
}
