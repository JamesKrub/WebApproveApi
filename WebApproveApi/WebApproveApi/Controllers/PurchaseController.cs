using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Xml.Schema;
using WebApproveApi.Models.PurchaseModel;

namespace WebApproveApi.Controllers
{
    public class PurchaseController : ApiController
    {

        // GET: api/purchase
        [HttpGet]
        [Route("api/data")]
        public List<sp_PurchaseSelect_Result> Get()
        {
            PurchaseServices service = new PurchaseServices();
            string xml = "<PurchaseOrders><PurchaseOrder purchaseId=\"1\"><Name>385</Name><Date>2002-05-30T09:00:00</Date><Price>500.50</Price><NumberQty>5</NumberQty></PurchaseOrder></PurchaseOrders>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            //return jsonText;
            return service.getPurchase();
        }

        // GET: api/purchase/
        [HttpGet]
        [Route("api/data/{id?}")]
        public Purchase Get(string id)
        {
            PurchaseServices service = new PurchaseServices();
            return service.getPurchasebyKey(id);
        }

        // GET: Money In years
        [HttpGet]
        [Route("api/moneyByMonth")]
        public List<sp_PurchaseSelectTtlMoneyEchMonthByYear_Result> GetMoney()
        {
            PurchaseServices service = new PurchaseServices();
            return service.getMoneyByMonth();
        }

        // GET: All years
        [HttpGet]
        [Route("api/allYear")]
        public List<sp_PurchaseSelectYear_Result> Getyear()
        {
            PurchaseServices service = new PurchaseServices();
            return service.getAllYear();
        }

        // POST: api/purchase
        [HttpPost]
        [Route("api/dataAdd")]
        public void Post([FromBody]PurchaseCriteria criteria)
        {
            PurchaseServices service = new PurchaseServices();
            //PurchaseCriteria criteria = new PurchaseCriteria();
            service.insertPurchase(criteria);
        }

        // POST: api/purchase
        [HttpPost]
        [Route("api/dataAddtest")]
        public string Post2([FromBody]string criteria)
        {
            criteria = "<Answer><ShortDesc>385</ShortDesc><AnswerValue>Z</AnswerValue></Answer>";
            XmlSchemaValidator validator = new XmlSchemaValidator();
            string url = @"D:\vsWork\XSDSchemaTest.xsd";
            string retVal = "<VALIDXML>" +
                   validator.ValidXmlDoc(criteria, "",
                   url) +
                   "</VALIDXML>";

            if (!validator.IsValidXml)
            {
                retVal = retVal + validator.ValidationError;
            }

            return retVal;
        }

        // PUT: api/purchase/5
        [HttpPut]
        [Route("api/data/{id?}")]
        public void Put(int id, [FromBody]PurchaseCriteria criteria)
        {
            PurchaseServices service = new PurchaseServices();
            service.updatePurchase(criteria, id);
        }

        // DELETE: api/purchase/5
        [HttpDelete]
        [Route("api/data/{id?}")]
        public void Delete(string id)
        {
            PurchaseServices service = new PurchaseServices();
            service.deletePurchase(id);
        }
    }

    
}
