using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assitor.Utils
{
    /// <summary>
    /// Http请求结果
    /// </summary>
    /// <typeparam name="T">请求结果数据类型</typeparam>
    public class HttpResult<T>
    {
        /// <summary>
        /// 请求状态码
        /// </summary>
        [JsonProperty("code")]
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// 请求结果
        /// </summary>
      
        public T Content { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Error { get; set; }
    }

    public class BatchResult<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }

    public class OneResult<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    /// <summary>
    /// http请求结果数据结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultData<T>
    {
        public int Code { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>

        public List<T> DataList { get; set; }
    }

    /// <summary>
    /// 错误数据结果
    /// </summary>
    public class ErrorData
    {
        /// <summary>
        /// 错误描述
        /// </summary>
        public string Error { get; set; }
    }

    /// <summary>
    /// 分页信息结构
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 开始页数
        /// </summary>
        public int BeginPage { get; set; }

        /// <summary>
        /// 结束页数
        /// </summary>
        public int EndPage { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }
    }
}
