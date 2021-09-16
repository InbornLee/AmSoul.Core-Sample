using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AmSoul.Core.Interfaces
{
    public interface IGridFSService
    {
        Task<string> UploadFile(string bucketName, string fileName, Stream fileStream, BsonDocument metaData = null, CancellationToken cancellationToken = default);
        Task<GridFSDownloadStream> DownloadFile(string bucketName, string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteFile(string bucketName, string id, CancellationToken cancellationToken = default);
        Task<List<GridFSFileInfo>> FindFilesAsync(string bucketName, FilterDefinition<GridFSFileInfo> filter, CancellationToken cancellationToken = default);
        string GetMD5Hash(Stream stream);
    }
}
