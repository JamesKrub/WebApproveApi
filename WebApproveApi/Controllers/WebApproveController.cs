using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using WebApproveApi.Models.WebApvModel;

namespace WebApproveApi.Controllers
{
    public class WebApproveController : ApiController
    {
        // แสดงข้อมูลใน Dropdown เอกสารตามผู้ใช้งาน (EX. SO PO)
        #region getDocumentName
        [HttpGet]
        [Route("webApprove/getDocName/{id?}")]
        public List<sp_getDocNameByUserID_Result> getDocName_By_Permission(string id)
        {
            WebApvService service = new WebApvService();
             return service.getDocName_By_Permission(id);
        }
        #endregion

        // แสดงข้อมูลเอกสารที่รอการ approve ตาม regId 
        #region getDocDetail Waiting Approve
        [HttpGet]
        [Route("webApprove/getWaitingDocDetail/{id?}")]
        public List<sp_selectWaitingStatus_Result> getDocWaiting_By_regId(int id) 
        {
            WebApvService service = new WebApvService();
            return service.getDocWaitingData_By_regId(id);
        }
        #endregion

        // แสดงจำนวนของเอกสารที่ยังไม่ได้ทำการยืนยันทั้งหมด ตามบุคคล
        #region DocumentDetail
        [HttpGet]
        [Route("webApprove/getNumOfUnApprove/{empPermitId?}")]
        public List<sp_CheckUnApprove_Result> GetNumOfUnapprove(string empPermitId)
        {
            WebApvService service = new WebApvService();
            return service.checkNumberOfUnApprove(empPermitId);
        }
        #endregion

        // แสดงเอกสารทั้งหมดจากตัวเลือกทั้งหมด
        #region getAllDocDetail
        [HttpGet]
        [Route("webApprove/getAllDocDetail/{empPermitId?}")]
        public List<sp_getAllDocDetailByUserID_Result> getAllDocDetail(string empPermitId)
        {
            WebApvService service = new WebApvService();
            return service.getAllDocDetail(empPermitId);
        }
        #endregion

        // จัดการข้อมูลการอนุมัติ // update สถานะของ DataRequest
        #region manageApproving
        [HttpPost]
        [Route("webApprove/manageApproving")]
        public string manageApproving(manageAprrove data)
        {
            WebApvService service = new WebApvService();
            return service.manageApproving(data);
        }
        #endregion

        // เพิ่มประวัติการอนุมัติ //ถ้ามีการกดอนุมัติจะ Insert ลงตาราง HistoryApprove ด้วย
        #region addHistoryApproving
        [HttpPost]
        [Route("webApprove/historyApproving")]
        public string addHistoryApproving(manageAprrove data)
        {
            WebApvService service = new WebApvService();
            return service.addHistoryApproving(data);
        }
        #endregion

        // แสดงประวัติการอนุมัติ
        #region showHistory
        [HttpGet]
        [Route("webApprove/historyShow/{empPermitId?}")]
        public List<sp_HistoryApproving_Result> showHistory(string empPermitId)
        {
            WebApvService service = new WebApvService();
            return service.showHistory(empPermitId);
        }
        #endregion

        // แสดงรายละเอียดของเอกสาร //แสดงรายระเอียดโดยใช้ xslt (เก่า)
        #region showApproveDetail
        //[HttpGet]
        //[Route("webApprove/showAppriveDetail/{docId?}")]
        //public string showApproveDetail(int docId)
        //{
        //    WebApvService service = new WebApvService();
        //    var data = service.showApproveDetail(docId);
        //    //string xslt = JsonConvert.SerializeObject(data.Transform);
        //    //string xslt = JsonConvert.DeserializeObject(data.Transform).ToString();

        //    XslCompiledTransform transform = new XslCompiledTransform();
        //    using (XmlReader reader = XmlReader.Create(new StringReader(data.Transform)))
        //    {
        //        transform.Load(reader);
        //    }
        //    StringWriter results = new StringWriter();
        //    using (XmlReader reader = XmlReader.Create(new StringReader(data.dataRequest)))
        //    {
        //        transform.Transform(reader, null, results);
        //    }
        //    return results.ToString().Replace("\r\n", string.Empty);
        //}
        #endregion

        // แสดงรายละเอียดของเอกสารโดยใช้ XSD // ไม่ได้ใช้
        #region getNameFromXSD
        [HttpGet] 
        [Route("webApprove/a")]
        public List<string> getNameFromXSD ()
        {
            WebApvService service = new WebApvService();
            //string xml = "<PurchaseOrders xmlns=\"PurchaseForm\"><PurchaseOrder purchaseId=\"2\"><Header>กินมาม่า</Header><Name>เก้าอี้ไม้ไฟฟ้า</Name><Date>2002-05-30T09:00:00</Date><Price>150000.00</Price><NumberQty>20</NumberQty></PurchaseOrder></PurchaseOrders>";

            List<string> termsList = new List<string>();
            var xml = service.getSchema().Schema.Trim();
            var xs = XNamespace.Get("http://www.w3.org/2001/XMLSchema");
            var doc = XDocument.Parse(xml);
            foreach (var element in doc.Descendants(xs + "element"))
            {
                //Console.WriteLine(element.Attribute("name").Value);
                string a = element.Attribute("name").Value;
                termsList.Add(a);
            }
            return termsList;
        }
        #endregion

        // แสดงรายละเอียดของเอกสารโดยใช้เพียง XML
        #region getDetailByXML
        [HttpGet] 
        [Route("webApprove/getdetail_InDocumentBy_XML/{docId?}")]
        public List<readFromXML> getDetailByXML(int docId)
        {
            WebApvService service = new WebApvService();
            var qry_data = service.showApproveDetail(docId);
            string file = qry_data.dataRequest.ToString();
            List<readFromXML> dataXML = new List<readFromXML>();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(file); // โหลด string
            foreach (System.Xml.XmlElement document in doc.DocumentElement.ChildNodes)
            {
                foreach (System.Xml.XmlElement field in document.ChildNodes)
                {
                    string a = field.Name;
                    string b = field.InnerText;

                    dataXML.Add(new readFromXML { index = field.Name , data = field.InnerText });
                }
            }
            return dataXML;
        }
        #endregion

        // service สำหรับระบบภายนอก (Pulling)
        #region pullingService
        [HttpGet]
        [Route("webApprove/pullingStatusApprove/")]
        public List<sp_PullingService_Result> pullingService()
        {
            WebApvService service = new WebApvService();
            return service.PullingService();
        }
        #endregion
    }
        
    public class readFromXML
    {
        public string index { get; set; }
        public string data { get; set; }
    }
}