using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmSoul.Core.Interfaces
{
    public interface IAsyncRESTService<T> where T : class, IDataModel
    {
        Task<List<T>> GetAsync(CancellationToken cancellationToken = default);
        Task<T> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T obj, CancellationToken cancellationToken = default);
        Task<ReplaceOneResult> PutAsync(string id, T objIn, CancellationToken cancellationToken = default);
        //Task<UpdateResult> PatchAsync(string id, Hashtable args);
        Task<DeleteResult> DeleteAsync(T objIn, CancellationToken cancellationToken = default);
        Task<DeleteResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
