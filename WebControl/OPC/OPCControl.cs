using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OPC.Common;
using OPC.Data.Interface;
using OPC.Data;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;
using WebControl.Models;



namespace WebControl.OPC
{
    public static class OPCControl
    {
        #region OPC变量
        const UInt16 MAX_ITEM_COUNT = 1024;
        public static values value=new values();
        public static readonly string[] item = { "$毫秒.Value", "$秒.Value", "$分.Value", "$时.Value", "$日.Value", "$月.Value", "$年.Value", "yc1.Value" };
        public static readonly string serverProgID = "KingView.view";                  //这里是连接组态王的OPC   
      //  public static readonly string serverProgID = "view.OPC.1"; 

        //          public static readonly string[] item ={ "S7:[S7 connection_1]MB100,2", "S7:[S7 connection_1]QB0", "S7:[S7 connection_1]MW200", "S7:[S7 connection_1]IB0,7","S7:[S7 connection_1]PIW752,4" ,"S7:[S7 connection_1]PQW752,2","S7:[S7 connection_1]DB1,B0,20"};
        //        string serverProgID = "OPC.SimaticNET";                //这里是连接西门子的OPC                                


        public static string groupName = "OPCGroup";
        public static UInt32[] updateCount = new UInt32[item.GetLength(0)];
        public static OpcServer srv;
        public static OpcGroup grp;
        public static OPCItemDef[] itemDefs = new OPCItemDef[item.GetLength(0)];
        public static int[] handlesSrv = new int[item.GetLength(0)];
        static public string GetItemName(int index)
        {
            if (index <= item.GetLength(0))
                return item[index];
            else
                return null;
        }
        #endregion
        public static void ConnnectOPC()                   //连接OPC
        {
            if (grp != null)
            {
                int[] aE;
                grp.RemoveItems(handlesSrv, out aE);
                grp.Remove(false);
                srv.Disconnect();
                grp = null;
                srv = null;
                Thread.Sleep(100);
            }
            srv = new OpcServer();
            srv.Connect(serverProgID);
            Thread.Sleep(500);
            grp = srv.AddGroup(groupName, false, 900);
            for (int i = 0; i < item.GetLength(0); i++)
                itemDefs[i] = new OPCItemDef(item[i], true, i, VarEnum.VT_EMPTY);
            OPCItemResult[] rItm;
            grp.AddItems(itemDefs, out rItm);
            if (rItm == null)
                return;
            for (int i = 0; i < item.GetLength(0); i++)
                //      handlesSrv[i] = i;
                handlesSrv[i] = rItm[i].HandleServer;
            grp.SetEnable(true);          //异步读取变量
            grp.Active = true;
            grp.DataChanged += new DataChangeEventHandler(OPCControl.grp_DataChange);

        }
        public static void grp_DataChange(object sender, DataChangeEventArgs e)
        {
            //   this.ItemView.Columns.Add("fdafd");
            if (value.AI== null)
            {
                value.AI=new List<ushort?>();
                for (int i = 0; i < 10; i++)
                    value.AI.Add(null);
                value.AO = new List<ushort?>();
                for (int i = 0; i < 10; i++)
                    value.AO.Add(null);
                value.DO = new List<byte?>();
                for (int i = 0; i < 10; i++)
                    value.DO.Add(null);
                value.DI = new List<byte?>();
                for (int i = 0; i < 10; i++)
                    value.DI.Add(null);
            }
            foreach (OPCItemState s in e.sts)
            {

                if (OPCControl.handlesSrv.Length < s.HandleClient)
                {
                    return;
                }
                if (s.DataValue.GetType().IsArray == true)
                {
                    Array ar;
                    ar = s.DataValue as Array;
                    for (int k = 0; k < ar.GetLength(0); k++)
                        value.AI[k] = Convert.ToUInt16(ar.GetValue(k));
                    //  s.DataValue;
                }
                else
                {
                    value.AI[s.HandleClient] = Convert.ToUInt16(s.DataValue);
                }
            }
        }
        public static bool SaveToDB()
        {
            if (value == null)
                return false;
            if (value.AI == null)
                return false;
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //   SqlConnection con=new SqlConnection(@"Data Source=20120621-0913\SQLEXPRESS;Initial Catalog=ControlSys;Integrated Security=True");
            System.DateTime dt = new DateTime();
            dt = System.DateTime.Now;
            string insertQuery = "INSERT INTO History (";
            insertQuery += "time,";
            for (int i = 0; i < 8; i++)
            {
                if (value.AI[i] != null)
                {
                    insertQuery += "AI" + i+",";
                }
                else
                {
                    if (i == 0)
                        return false;
                    break;
                }
            }
            insertQuery=insertQuery.Remove(insertQuery.Length - 1);
            insertQuery += ") Values(";
            insertQuery += "'";
            insertQuery += System.DateTime.Now.ToString();
            insertQuery += "',";
            for (int i = 0; i < 8; i++)
            {
                if (value.AI[i] != null)
                {
                    insertQuery += "'" + value.AI[i] + "',";
                }
                else
                    break;  
            }
            insertQuery=insertQuery.Remove(insertQuery.Length - 2, 2);
            insertQuery += "')";
         //   string myInsertQuery = "INSERT INTO Customers (CustomerID, CompanyName) Values('NWIND', 'Northwind Traders')";
            SqlCommand myCommand = new SqlCommand(insertQuery);
            myCommand.Connection = con;
            con.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            return true;
        }
        public static void vaildateDB()              //每天对昨天的数据做验证，去除重复的数据
        {
            System.DateTime dt = new DateTime();
            dt = System.DateTime.Now.AddDays(-1);
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            string condition = " where time> '";
            condition += System.DateTime.Now.AddDays(-1).ToString();
            condition += "' and time < '";
            condition += System.DateTime.Now.AddDays(1).ToString();
            condition += "'";
          //  string createTemp = "SELECT * INTO temp_History FROM (SELECT DISTINCT  * FROM History )";
            string createTemp = "Insert INTO temp_History SELECT DISTINCT  * FROM History";
            createTemp=createTemp.Insert(createTemp.Length, condition);      //选择出昨天入库的数据到临时表
            string deleteOld = "DELETE FROM History";
            deleteOld=deleteOld.Insert(deleteOld.Length, condition);        //删除入库的数据
            string updateNew = "insert into History select * from temp_History";  //去掉重复后的昨天的数据入库
            string deleteTemp = "DELETE FROM temp_History";
            //  string deleteTemp = "drop table temp_History";              //删除临时表
            SqlCommand myCommand = new SqlCommand(deleteTemp);
            myCommand.Connection = con;
            con.Open();
            myCommand.ExecuteNonQuery();
            myCommand.CommandText = createTemp;
            myCommand.ExecuteNonQuery();
            myCommand.CommandText = deleteOld;
            myCommand.ExecuteNonQuery();
            myCommand.CommandText = updateNew;
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }
    }
}