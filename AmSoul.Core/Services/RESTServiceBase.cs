using AmSoul.Core.DependencyInjection;
using AmSoul.Core.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmSoul.Core.Services
{
    public abstract class RESTServiceBase<T> : IAsyncRESTService<T> where T : class, IDataModel
    {
        private readonly IMongoCollection<T> _collection;
        public RESTServiceBase(IDatabaseSettings settings)
        {
            _collection = IdentityExtensions.GetCollection<T>(settings, typeof(T).Name.ToLowerInvariant() + "s");
        }
        public async virtual Task<T> CreateAsync(T obj, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(obj, cancellationToken: cancellationToken);
            return obj;
        }
        public async virtual Task<List<T>> GetAsync(CancellationToken cancellationToken) => await _collection.Find(obj => true).ToListAsync(cancellationToken: cancellationToken);
        public async virtual Task<T> GetAsync(string id, CancellationToken cancellationToken) => await _collection.Find(obj => obj.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        public async virtual Task<DeleteResult> DeleteAsync(T objIn, CancellationToken cancellationToken) => await _collection.DeleteOneAsync(obj => obj.Id == objIn.Id, cancellationToken: cancellationToken);
        public async virtual Task<DeleteResult> DeleteAsync(string id, CancellationToken cancellationToken) => await _collection.DeleteOneAsync(obj => obj.Id == id, cancellationToken: cancellationToken);
        public async virtual Task<ReplaceOneResult> PutAsync(string id, T objIn, CancellationToken cancellationToken) => await _collection.ReplaceOneAsync(obj => obj.Id == id, objIn, cancellationToken: cancellationToken);

    }
}
