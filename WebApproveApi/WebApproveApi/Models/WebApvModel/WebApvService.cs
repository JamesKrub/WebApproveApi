using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApproveApi.Models.WebApvModel
{
    public class WebApvService
    {
        // สร้างการเชื่อมจ่อกับ Database
        #region Conenction
        private WebApproveEntities _conn;
        public WebApproveEntities conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new WebApproveEntities();
                }
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }
        #endregion

        // ตรวจสอบว่ามี Namespace อยู่หรือไม่
        #region validateRegister
        public Register validateRegister(string targetNamespace)
        {
            try
            {
                //var qry = from reg in conn.Registers where reg.targetNameSpace == targetNamespace select reg;
                return conn.Registers.Where(a => a.targetNameSpace == targetNamespace).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        // เพิ่มข้อมูลการ Register ลง Database
        #region Add Register Document
        public sp_RegisterInsert_Result registerDocument(RegisData data)
        {
            try
            {
                var a = conn.sp_RegisterInsert(
                    data.schemaName,
                    data.Schema,
                    data.targetNamespace,
                    data.Transform
                );
                return a.FirstOrDefault();
            }
            catch (Exception ex)
            {
                //return "fail";
                throw ex;
            }
        }
        #endregion

        // เพิ่มการ Request Data
        #region Add Request Document
        public void insertDocumentDetail(RequestData data)
        {
            try
            {
                data.dateRequest = DateTime.Now;
                data.statusId = "01";
                conn.sp_DocDetailInsert(
                    data.subjectName,
                    data.dataRequest,
                    data.dateRequest,
                    data.empId,
                    data.statusId,
                    data.regId
                    );
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        // เพิ่ม permission
        #region AddPermission
        public void insertPermission(Int32 regId, string empId)
        {
            try
            {
                conn.sp_EmpPermissionInsert(
                    regId,
                    empId
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        // ดึง schema จาก database
        #region getSchema
        public Register getSchema(){
            try
            {
                return conn.Registers.Where(a => a.regId == 1).FirstOrDefault();
            }
            catch ( Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        // แสดงจำนวนเอกสารที่ยังไม่ได้ Approve 
        #region checkNumberOfUnApprove
        public List<sp_CheckUnApprove_Result> checkNumberOfUnApprove(string empPermit)
        {
            try
            {
                return conn.sp_CheckUnApprove(empPermit).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        // แสดงเอกสารทั้งหมดจากตัวเลือกทั้งหมด 
        #region getAllDocDetail
        public List<sp_getAllDocDetailByUserID_Result> getAllDocDetail(string empPermitId)
        {
            return conn.sp_getAllDocDetailByUserID(empPermitId).ToList();
        }
        #endregion

        // แสดงข้อมูลใน Dropdown เอกสารตามผู้ใช้งาน
        #region docDetailByPermission
        public List<sp_getDocNameByUserID_Result> getDocName_By_Permission(string empId)
        {
            return conn.sp_getDocNameByUserID(empId).ToList();
        }
        #endregion

        // แสดงข้อมูลเอกสารที่รอการ approve ตาม regId 
        #region docDetailData_By_Permission
        public List<sp_selectWaitingStatus_Result> getDocWaitingData_By_regId(int regId)
        {
            return conn.sp_selectWaitingStatus(regId).ToList();
        }
        #endregion

        // จัดการข้อมูลการอนุมัติ // update สถานะของ DataRequest
        #region manageApproving
        public string manageApproving(manageAprrove data)
        {
            try
            {
                conn.sp_ApproveOrRejectUpdate(
                        data.docDetailId,
                        data.statusId
                     );
                return "Success";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        // เพิ่มประวัติการอนุมัติ 
        #region  addHistoryApproving
        public string addHistoryApproving (manageAprrove data)
        {
            try
            {
                conn.sp_historyInsert(
                    data.docDetailId,
                    data.statusId,
                    data.empPermission
                    );
                return "success";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        // แสดงประวัติการอนุมัติ ตามสิทธิ์การเข้าใช้
        #region showHistory
        public List<sp_HistoryApproving_Result> showHistory(string empPermitId)
        {
            try
            {
                return conn.sp_HistoryApproving(empPermitId).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion

        // แสดงรายละเอียดข้อมูลในเอกสาร
        #region showApproveDetail
        public sp_showApprovedetail_Result showApproveDetail(int docId)
        {
            try
            {
                //var qry = from doc in conn.DocumentDetails
                //          join reg in conn.Registers on doc.regId equals reg.regId
                //          where doc.docDetailId == docId
                //          select new { reg.Transform, doc.dataRequest };

                return conn.sp_showApprovedetail(docId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }


    // คลาสของการ Register
    #region RegisData
    public class RegisData
    {
        public string schemaName { get; set; }
        public string Schema { get; set; }
        public string targetNamespace { get; set; }
        public string Transform { get; set; }
        public string groupPermission { get; set; }
    }
    #endregion

    // คลาสของการ Request
    #region RequestData
    public class RequestData
    {
        public string subjectName { get; set; }
        public string dataRequest { get; set; }
        public DateTime dateRequest { get; set; }
        public string empId { get; set; }
        public string statusId { get; set; }
        public int regId { get; set; }
    }
    #endregion

    // คลาส การจัดการการอนุมัติ
    #region manageAprrove
    public class manageAprrove
    {
        public string statusId { get; set; }
        public int docDetailId { get; set; }
        public string empPermission { get; set; }
    }
    #endregion
}