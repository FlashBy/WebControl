using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebControl.Models;
using System.Web.UI.DataVisualization.Charting;
using WebControl.OPC;
using System.Collections;

namespace WebControl.Controllers
{
    public class WorkController : Controller
    {
        //
        // GET: /Work/
     
        #region GetValues
        public values  GetValues()
        {
            values vs=new values();
            vs.AI = GetAI();
            vs.AO = GetAO();
            vs.DI = GetDI();
            vs.DO = GetDO();
            vs.DO.Add(1);
            vs.DI.Add(1);
            vs.AI.Add(1);
            vs.AO.Add(1); 
            vs.DO.Add(1);
            vs.DI.Add(1);
            vs.AI.Add(1);
            vs.AO.Add(1);

            vs.DO.Add(3);
            ValidateRank(ref vs);
            return vs;
        }
        protected void ValidateRank(ref values vs)
        {
            int rank = vs.DO.Count > vs.DI.Count ? vs.DO.Count : vs.DI.Count;
            rank = rank > vs.AI.Count ? rank : vs.AI.Count;
            rank = rank > vs.AO.Count ? rank : vs.AO.Count;
            for (int i = vs.DO.Count; i < rank; i++)
                vs.DO.Add(null);
            for (int i = vs.DI.Count; i < rank; i++)
                vs.DI.Add(null);
            for (int i = vs.AI.Count; i < rank; i++)
                vs.AI.Add(null);
            for (int i = vs.AO.Count; i < rank; i++)
                vs.AO.Add(null);
        }
        protected List<Byte?> GetDI()
        {
            List<Byte?> DI = new List<byte?>();
            return DI;
        }
        protected List<Byte?> GetDO()
        {
            List<Byte?> DO = new List<byte?>();
            return DO;
        }
        protected List<UInt16?> GetAI()
        {
            List<UInt16?> AI = new List<UInt16?>();
            return AI;
        }
        protected List<UInt16?> GetAO()
        {
            List<UInt16?> AO = new List<UInt16?>();
            return AO;
        }
        protected values vs;
        #endregion

        public ActionResult test()
        {
            //   vs = GetValues();
            return View();
        }
        public ActionResult Values()
        {
         //   vs = GetValues();
            return View("Values",OPCControl.value);
        }
        public ActionResult Current()
        {
            Chart chart1=new Chart();
            chart1.Width=600;
            chart1.Height=400;
            chart1.EnableTheming=true;
            return View("Current",chart1);
        }
    
        public ActionResult History()
        {
            ViewBag.Message = "";
            ViewBag.Date = DateTime.Now.ToShortDateString();
            return View();
        }
        public ActionResult Statements()
        {
            return View();
        }
        public string GetStatus()
        {
            return "Status OK at " + DateTime.Now.ToLongTimeString();
        }

        public string UpdateForm(string textBox1)
        {
            if (textBox1 != "Enter text")
            {
                return "You entered: \"" + textBox1.ToString() + "\" at " +
                    DateTime.Now.ToLongTimeString();
            }

            return String.Empty;
        }
        public ActionResult LoadData()
        {
            return Values();
        }
        public string UpdateData(string textBox1)
        {
            if (textBox1 != "Enter text")
            {
                return "You entered: \"" + textBox1.ToString() + "\" at " +
                    DateTime.Now.ToLongTimeString();
            }

            return String.Empty;
        }

        static int iTest=0;
        public ActionResult realInt()
        {
            iTest++;
            return Json(iTest.ToString(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetJSON()
        {
            ArrayList list = new ArrayList();

            Hashtable ht1 = new Hashtable();

            ht1.Add("name", "myName");

            Hashtable h2 = new Hashtable();

            h2.Add("name", "Test名称");

            list.Add(ht1);

            list.Add(h2);

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AIToWeb()
        {
            ArrayList list = new ArrayList();
            foreach (ushort? u in OPCControl.value.AI)
            {
                list.Add(u);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        static float [] fVal=new float[7];
        public JsonResult retRan7()
        {
            for (int i = 6; i >0; i--)
                fVal[i] = fVal[i - 1];
         //   fVal[1] = fVal[0];
        //    fVal[2] = fVal[1];
        //    fVal[3] = fVal[2];
            Random ran = new Random();
            fVal[0] = ran.Next() % 100;
            return Json(fVal, JsonRequestBehavior.AllowGet);
        }
    }
}
