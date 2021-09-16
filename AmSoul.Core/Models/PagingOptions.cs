using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmSoul.Core.Models
{
    public sealed class PagingOptions
    {
        /// <summary>
        /// 记录条数
        /// </summary>
        [FromQuery]
        [Range(1, 1000, ErrorMessage = "Limit must >0 and <1000")]
        public int? Limit { get; set; }
        /// <summary>
        /// 偏移值
        /// </summary>
        [FromQuery]
        [Range(1, int.MaxValue, ErrorMessage = "Offset must > 0")]
        public int? Offset { get; set; }
        /// <summary>
        /// 排序方式：asc=true,desc=false
        /// </summary>
        [FromQuery]
        public bool Order { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        [FromQuery]
        public string SortField { get; set; }
        [FromQuery]
        public List<QueryOption> QueryOptions { get; set; }
        /// <summary>
        /// 库名称
        /// </summary>
        [FromQuery]
        public string Bucket { get; set; }
    }
    public class QueryOption
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 是否日期时间
        /// </summary>
        public bool IsDateTime { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 方法:AND OR IN EQ
        /// </summary>
        public string Method { get; set; }
    }
    public sealed class PageData
    {
        public int? Total { get; set; }
        public ICollection Rows { get; set; }
    }

}
