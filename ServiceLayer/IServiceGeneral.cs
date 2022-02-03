using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    /// <summary>
    /// general interface that can be used from the services
    /// </summary>
    /// <typeparam name="T1">a projection of the entity type to be fetched from the db</typeparam>
    /// <typeparam name="T2">another projection of the entity type to be fetched from the db</typeparam>
    public interface IServiceGeneral <T1,T2,T>
    {
        Task<EntityPage<T1>> GetPageAsync (int pageNum, int pageSize, string searchByName);
        Task<T2> GetDetailAsync (int Id);

        Task<IEnumerable<T>> GetAll();
    }
}
