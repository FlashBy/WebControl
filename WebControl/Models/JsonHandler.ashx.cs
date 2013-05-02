using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebControl.Models
{
    /// <summary>
    /// JsonHandler 的摘要说明
    /// </summary>
    public class JsonHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StreamReader sr = new StreamReader("C:\\datagrid_data.json");
            if (sr == null)
                return;
            string json = sr.ReadToEnd();
         //   string json = "{'total':239,'rows':[{'code':'001','name':'Name 1','addr':'Address 11','col4':'col4 data'},{'code':'002','name':'Name 1','addr':'Address 11','col4':'col4 data'},{'code':'003','name':'Name 1','addr':'Address 11','col4':'col4 data'}]}";
            context.Response.Write(json);
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