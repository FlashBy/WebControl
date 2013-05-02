using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebControl.paging
{
    /// <summary>
    /// 数据结果
    /// </summary>
    public class DataResult
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public DataTable Data { get; set; }
    }
}
