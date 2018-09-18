using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using WebApproveApi.Models.WebApvModel;

namespace WebApproveApi.Controllers
{
    public class ValidateSchemaController : ApiController
    {
        public string scheA;

        // ฟังก์ชันการ Register
        #region RegisDocument
        [HttpPost]
        [Route("regis/Document")]
        public string RegisDocument([FromBody]RegisData value)
        {
            WebApvService service = new WebApvService();

            string groupApp = JsonConvert.DeserializeObject(value.groupPermission).ToString();
            string schema = (string)JsonConvert.DeserializeObject(value.Schema);
            string tranform = (string)JsonConvert.DeserializeObject(value.Transform);

            StringReader str = new StringReader(schema);
            XmlReader xmlr = XmlReader.Create(str);
            XmlSchema sch = XmlSchema.Read(xmlr, verificaErros);
            string targetnamespace = sch.TargetNamespace;

            if ( service.validateRegister(targetnamespace) != null )
            {
                return "Duplication of targetNamespace";
            }

            value.Schema = schema;
            value.Transform = tranform;
            value.targetNamespace = targetnamespace;
            sp_RegisterInsert_Result result = service.registerDocument(value);

            Int32 regid = (Int32)result.reg_ID;
            string[] eachPerson = groupApp.Split(',');
            foreach (var row in eachPerson)
            {
                service.insertPermission(regid, row);
            }
            return "success";
        }
        #endregion

        // ตรวจสอบในการเรียกเก็บ targetNamespace ขณะ Register
        #region verificaErros
        private void verificaErros(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine(args.Message);
        }
        #endregion
        
        // ฟังก์ชัน Request #1
        #region Request Document and Check TargetNamespace #1
        [HttpPost]
        [Route("request/Document")]
        public string RequestDocument([FromBody]RequestData value)
        {
            WebApvService service = new WebApvService();
            XmlDocument findXML = new XmlDocument();

            //var xml = "<PurchaseOrders xmlns=\"PurchaseForm\"><PurchaseOrder purchaseId=\"1\"><Header>ไปเที่ยว</Header><Name>385</Name><Date>2002-05-30T09:00:00</Date><Price>500.50</Price><NumberQty>5</NumberQty></PurchaseOrder></PurchaseOrders>";
            var xml = value.dataRequest;

            try
            {
                findXML.LoadXml(xml); // เรียกใช้งานเอกสาร XML
                string str = findXML.LastChild.NamespaceURI; // ดึงชื่อ namespace จาก XML

                Register data = service.validateRegister(str);

                if (data == null)
                {
                    return "The XML does not match our records.";
                }

                if (vaidateSchema(data.Schema, data.targetNameSpace, xml) == "True")
                {
                    value.regId = data.regId;
                    service.insertDocumentDetail(value);
                    return "Success";
                }
                return "Fail";
            }
            catch (Exception ex)
            {
                return "The XML does not match with register's records.";
                throw ex;
            }    
            
        }
        #endregion

        // ตรวจสอบ Schema #2 (ถูกเรียกใช้โดย #1)
        #region validate Schema #2
        public string vaidateSchema(string schema, string targetNamespace, string xml)
        {
            WebApvService service = new WebApvService();
            
            XmlSchemaValidator validator = new XmlSchemaValidator();
            string retVal = validator.ValidXmlDoc(xml, targetNamespace, schema).ToString();

            if (!validator.IsValidXml)
            {
                retVal = retVal + validator.ValidationError;
            }

            return retVal;
        }
        #endregion
        

    }

    // เรียกใช้ในการตรวจสอบ Request Data
    #region XmlSchemaValidator
    public class XmlSchemaValidator
    {
        private bool isValidXml = true;
        private string validationError = "";

        /// <SUMMARY>
        /// Empty Constructor.
        /// </SUMMARY>
        public XmlSchemaValidator()
        {

        }

        /// <SUMMARY>
        /// Public get/set access to the validation error.
        /// </SUMMARY>
        public String ValidationError
        {
            get
            {
                return "<VALIDATIONERROR>" + this.validationError
                       + "</VALIDATIONERROR>";
            }
            set
            {
                this.validationError = value;
            }
        }

        /// <SUMMARY>
        /// Public get access to the isValidXml attribute.
        /// </SUMMARY>
        public bool IsValidXml
        {
            get
            {
                return this.isValidXml;
            }
        }

        /// <SUMMARY>
        /// This method is invoked when the XML does not match
        /// the XML Schema.
        /// </SUMMARY>
        /// <PARAM name="sender"></PARAM>
        /// <PARAM name="args"></PARAM>
        private void ValidationCallBack(object sender,
                                   ValidationEventArgs args)
        {
            // The xml does not match the schema.
            isValidXml = false;
            this.ValidationError = args.Message;
        }


        /// <SUMMARY>
        /// This method validates an xml string against an xml schema.
        /// </SUMMARY>
        /// <PARAM name="xml">XML string</PARAM>
        /// <PARAM name="schemaNamespace">XML Schema Namespace</PARAM>
        /// <PARAM name="schemaUri">XML Schema Uri</PARAM>
        /// <RETURNS>bool</RETURNS>
        public bool ValidXmlDoc(string xml,
               string schemaNamespace, string schemaUri)
        {
            try
            {
                // Is the xml string valid?
                if (xml == null || xml.Length < 1)
                {
                    return false;
                }

                StringReader srXml = new StringReader(xml);
                return ValidXmlDoc(srXml, schemaNamespace, schemaUri);
            }
            catch (Exception ex)
            {
                this.ValidationError = ex.Message;
                return false;
            }
        }

        /// <SUMMARY>
        /// This method validates an xml document against an xml 
        /// schema.
        //public bool ValidXmlDoc(XmlDocument xml,
        //       string schemaNamespace, string schemaUri)
        //{
        //    try
        //    {
        //        // Is the xml object valid?
        //        if (xml == null)
        //        {
        //            return false;
        //        }

        //        // Create a new string writer.
        //        StringWriter sw = new StringWriter();
        //        // Set the string writer as the text writer 
        //        // to write to.
        //        XmlTextWriter xw = new XmlTextWriter(sw);
        //        // Write to the text writer.
        //        xml.WriteTo(xw);
        //        // Get 
        //        string strXml = sw.ToString();

        //        StringReader srXml = new StringReader(strXml);

        //        return ValidXmlDoc(srXml, schemaNamespace, schemaUri);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ValidationError = ex.Message;
        //        return false;
        //    }
        //}

        /// <SUMMARY>
        /// This method validates an xml string against an xml schema.
        /// </SUMMARY>
        /// <PARAM name="xml">StringReader containing xml</PARAM>
        /// <PARAM name="schemaNamespace">XML Schema Namespace</PARAM>
        /// <PARAM name="schemaUri">XML Schema Uri</PARAM>
        /// <RETURNS>bool</RETURNS>
        public bool ValidXmlDoc(StringReader xml,
               string schemaNamespace, string schemaUri)
        {
            // Continue?
            if (xml == null || schemaNamespace == null || schemaUri == null)
            {
                return false;
            }

            isValidXml = true;
            XmlValidatingReader vr;
            XmlTextReader tr;
            XmlSchemaCollection schemaCol = new XmlSchemaCollection();
            
            StringReader conv = new StringReader(schemaUri);
            XmlReader data = XmlReader.Create(conv);

            schemaCol.Add(schemaNamespace, data);
            try
            {
                // Read the xml.
                tr = new XmlTextReader(xml);
                // Create the validator.
                vr = new XmlValidatingReader(tr);
                // Set the validation tyep.
                vr.ValidationType = ValidationType.Auto;
                // Add the schema.
                if (schemaCol != null)
                {
                    vr.Schemas.Add(schemaCol);
                }
                // Set the validation event handler.
                vr.ValidationEventHandler +=
                   new ValidationEventHandler(ValidationCallBack);
                // Read the xml schema.
                while (vr.Read())
                {
                }

                vr.Close();

                return isValidXml;
            }
            catch (Exception ex)
            {
                this.ValidationError = ex.Message;
                return false;
            }
            finally
            {
                // Clean up...
                vr = null;
                tr = null;
            }
        }
    }
    #endregion
}
