using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmSoul.Core.Interfaces
{
    public interface IDataModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }
    }
}
