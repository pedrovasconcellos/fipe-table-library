using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vasconcellos.FipeTable.UploadService.Repositories.Interfaces
{
    public interface IRepository
    {
        bool HaveReferenceIdGreaterOrEquals(long referenceId);
        Task<IList<T>> GetAllAsync<T>();
        Task<bool> InsertOneAsync<T>(T entity);
        Task<bool> InsertManyAsync<T>(IList<T> entities);
    }
}
