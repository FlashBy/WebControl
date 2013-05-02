using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace WebControl.paging
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PagingParameters
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示个数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 总记录数，为输出参数
        /// </summary>
        public int Total
        {
            get
            {
                if (_parameters.Count > 0)
                {
                    if (_parameters[5].Value.ToString() == "")
                        return 1;
                    else
                        return (int)_parameters[5].Value;
                }
                return 0;
            }
        }

        private IList<SqlParameter> _parameters = new List<SqlParameter>();

        /// <summary>
        /// 存储过程参数
        /// </summary>
        /// <returns></returns>
        public SqlParameter[] ProcedureParameters
        {
            get
            {
                if (_parameters.Count == 0)
                {
                    _parameters.Add(new SqlParameter("PageIndex", PageIndex));
                    _parameters.Add(new SqlParameter("PageSize", PageSize));
                    _parameters.Add(new SqlParameter("Where", Where));
                    _parameters.Add(new SqlParameter("OrderBy", OrderBy));
                    _parameters.Add(new SqlParameter("Sort", Sort));
                    var outputTotal = new SqlParameter
                    {
                        Direction = ParameterDirection.Output,
                        ParameterName = "Total",
                        DbType = DbType.Int32,
                        Size = 4
                    };
                    _parameters.Add(outputTotal);
                }

                return _parameters.ToArray();
            }

        }
    }
}
