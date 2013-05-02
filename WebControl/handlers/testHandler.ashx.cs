using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using WebControl.helper;
using WebControl.paging;

namespace WebControl.handlers
{
    /// <summary>
    /// testHandler 的摘要说明
    /// </summary>
    public class testHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            string sql = "select top 50 OrderID,CustomerID,ShipName,OrderDate from Orders ";
            DataTable dt = SqlHelper.ExecuteDataset(sql).Tables[0];
            int pageSize = 10;
            // 记录总数
            int rowCount = dt.Rows.Count;
            // 总页数
            int pageCount = rowCount % pageSize == 0 ? rowCount / pageSize : rowCount / pageSize + 1;

            var resultObj = new DataResult
            {
                // 总页数
                PageCount = pageCount,
                // 当前页
                PageIndex = 1,
                // 总记录数
                Total = rowCount,
                // 数据
                Data = dt
            };

            string resultJson = JsonHelper.Serialize(resultObj);

            context.Response.ContentType = "text/plain";
          //  context.Response.Write("{\r\n  \"total\": 10,\r\n  \"pagecount\": 1,\r\n  \"pageindex\": 1,\r\n  \"rows\": [\r\n    {\r\n      \"OrderID\": 10248,\r\n      \"CustomerID\": \"VINET\",\r\n      \"ShipName\": \"Vins et alcools Chevalier\",\r\n      \"OrderDate\": \"1996-07-04 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10249,\r\n      \"CustomerID\": \"TOMSP\",\r\n      \"ShipName\": \"Toms Spezialitäten\",\r\n      \"OrderDate\": \"1996-07-05 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10250,\r\n      \"CustomerID\": \"HANAR\",\r\n      \"ShipName\": \"Hanari Carnes\",\r\n      \"OrderDate\": \"1996-07-08 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10251,\r\n      \"CustomerID\": \"VICTE\",\r\n      \"ShipName\": \"Victuailles en stock\",\r\n      \"OrderDate\": \"1996-07-08 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10252,\r\n      \"CustomerID\": \"SUPRD\",\r\n      \"ShipName\": \"Suprêmes délices\",\r\n      \"OrderDate\": \"1996-07-09 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10253,\r\n      \"CustomerID\": \"HANAR\",\r\n      \"ShipName\": \"Hanari Carnes\",\r\n      \"OrderDate\": \"1996-07-10 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10254,\r\n      \"CustomerID\": \"CHOPS\",\r\n      \"ShipName\": \"Chop-suey Chinese\",\r\n      \"OrderDate\": \"1996-07-11 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10255,\r\n      \"CustomerID\": \"RICSU\",\r\n      \"ShipName\": \"Richter Supermarkt\",\r\n      \"OrderDate\": \"1996-07-12 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10256,\r\n      \"CustomerID\": \"WELLI\",\r\n      \"ShipName\": \"Wellington Importadora\",\r\n      \"OrderDate\": \"1996-07-15 00:00:00\"\r\n    },\r\n    {\r\n      \"OrderID\": 10257,\r\n      \"CustomerID\": \"HILAA\",\r\n      \"ShipName\": \"HILARION-Abastos\",\r\n      \"OrderDate\": \"1996-07-16 00:00:00\"\r\n    }\r\n  ]\r\n}");
            context.Response.Write(resultJson);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}