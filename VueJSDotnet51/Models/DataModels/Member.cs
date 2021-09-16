using AmSoul.Core.Converters;
using AmSoul.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VueJSDotnet51.Models
{
    public class Member : IDataModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Organization { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityCode { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telecom { get; set; }
        /// <summary>
        /// 是否Active
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// 性别（只读，按照身份证生成）
        /// </summary>
        [BsonIgnore]
        public string Gender => IDCardValidation.GetIDCardGender(IdentityCode);
        /// <summary>
        /// 年龄（只读，按照身份证生成）
        /// </summary>
        [BsonIgnore]
        public string Age => IDCardValidation.GetIdCardAge(IdentityCode);

    }
}
