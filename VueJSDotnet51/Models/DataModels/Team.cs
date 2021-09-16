using AmSoul.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace VueJSDotnet51.Models
{
    public class Team : IDataModel
    {
        /// <summary>
        /// 团队ID
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        /// <summary>
        /// 团队名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 团队标识
        /// </summary>
        public List<string> TeamTags { get; set; }
        /// <summary>
        /// 团队状态（0：停用 1：启用）
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
    }
}
