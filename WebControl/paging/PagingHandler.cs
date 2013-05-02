using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebControl.helper;

namespace WebControl.paging
{
    /// <summary>
    /// 抽象分页http处理程序类
    /// </summary>
    public abstract class PagingHandler : IHttpHandler
    {
        public virtual void ProcessRequest(HttpContext context)
        {
            var parameters = new PagingParameters
            {
                // 接收PageIndex参数
                PageIndex = int.Parse(context.Request["PageIndex"]),
                // 接收PageSize参数
                PageSize = int.Parse(context.Request["PageSize"]),
                // 接收OrderBy参数
                OrderBy = context.Request["OrderBy"],
                // 接收Sort参数
                Sort = context.Request["Sort"],
                // 存储过程名称
                ProcedureName = ProcName,
                // Where条件
                Where = string.Empty
            };
            this.PagingParameters = parameters;

            context.Response.ContentType = "application/json";
            context.Response.Write(GetJsonResult());

        }

        /// <summary>
        /// 获取输出到客户端的json
        /// </summary>
        public virtual string GetJsonResult()
        {
            DataSet ds = SqlHelper.ExecuteDataset(PagingParameters.ProcedureName, PagingParameters.ProcedureParameters);
            DataTable table = ds.Tables[0];

            // 存储过程已经执行完毕，可以获取到输出参数Total的值
            int total = PagingParameters.Total;
            // 计算总页数
            int pageCount = total % PagingParameters.PageSize == 0 ? total / PagingParameters.PageSize : total / PagingParameters.PageSize + 1;
            var dataResult = new DataResult
            {
                Data = table,
                PageIndex = PagingParameters.PageIndex,
                PageCount = pageCount,
                Total = total
            };
            // 序列化DataResult对象
            string json = JsonHelper.Serialize(dataResult);
            return json;
        }

        protected PagingParameters PagingParameters { get; set; }


        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 必须初始化的属性：存储过程名称
        /// </summary>
        public abstract string ProcName { get; }

    }
}
