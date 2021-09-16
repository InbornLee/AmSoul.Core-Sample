using AmSoul.Core.DependencyInjection;
using AmSoul.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace AmSoul.Core.Services
{
    public class GridFSServiceBase : IGridFSService
    {
        private readonly IMongoDatabase _mongoDatabase;
        public GridFSServiceBase(IDatabaseSettings settings)
        {
            _mongoDatabase = IdentityExtensions.GetDatabase(settings);
        }
        public async Task<bool> DeleteFile(string bucketName, string id, CancellationToken cancellationToken)
        {
            var bucket = new GridFSBucket(_mongoDatabase, new GridFSBucketOptions { BucketName = bucketName });
            using Task task = bucket.DeleteAsync(new ObjectId(id), cancellationToken: cancellationToken);
            await task;
            return task.IsCompleted;
        }

        public async Task<GridFSDownloadStream> DownloadFile(string bucketName, string id, CancellationToken cancellationToken)
        {
            var bucket = new GridFSBucket(_mongoDatabase, new GridFSBucketOptions { BucketName = bucketName });
            return await bucket.OpenDownloadStreamAsync(new ObjectId(id), new GridFSDownloadOptions() { CheckMD5 = true }, cancellationToken: cancellationToken);
        }

        public async Task<List<GridFSFileInfo>> FindFilesAsync(string bucketName, FilterDefinition<GridFSFileInfo> filter, CancellationToken cancellationToken)
        {
            var bucket = new GridFSBucket(_mongoDatabase, new GridFSBucketOptions { BucketName = bucketName });
            using var result = await bucket.FindAsync(filter, cancellationToken: cancellationToken);
            return await result.ToListAsync(cancellationToken: cancellationToken);
        }

        public string GetMD5Hash(Stream stream)
        {
            string result = null;
            byte[] arrbytHashValue;
            using MD5CryptoServiceProvider md5Hasher = new();
            try
            {
                arrbytHashValue = md5Hasher.ComputeHash(stream);//计算指定Stream 对象的哈希值
                                                                //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
                string hashData = BitConverter.ToString(arrbytHashValue);
                //替换-
                hashData = hashData.Replace("-", "");
                result = hashData;
            }
            catch (Exception)
            {
            }
            return result;
        }

        public async Task<string> UploadFile(string bucketName, string fileName, Stream fileStream, BsonDocument metaData = null, CancellationToken cancellationToken = default)
        {
            var bucket = new GridFSBucket(_mongoDatabase, new GridFSBucketOptions { BucketName = bucketName });
            return (await bucket.UploadFromStreamAsync(fileName, fileStream, new GridFSUploadOptions { Metadata = metaData }, cancellationToken: cancellationToken)).ToString();
        }
    }
}
