﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApproveApi.Models.WebApvModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class WebApproveEntities : DbContext
    {
        public WebApproveEntities()
            : base("name=WebApproveEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DocumentDetail> DocumentDetails { get; set; }
        public virtual DbSet<Register> Registers { get; set; }
        public virtual DbSet<EmpPermission> EmpPermissions { get; set; }
    
        public virtual ObjectResult<sp_RegisterInsert_Result> sp_RegisterInsert(string schemaName, string schema, string targetNameSpace, string transform)
        {
            var schemaNameParameter = schemaName != null ?
                new ObjectParameter("schemaName", schemaName) :
                new ObjectParameter("schemaName", typeof(string));
    
            var schemaParameter = schema != null ?
                new ObjectParameter("Schema", schema) :
                new ObjectParameter("Schema", typeof(string));
    
            var targetNameSpaceParameter = targetNameSpace != null ?
                new ObjectParameter("targetNameSpace", targetNameSpace) :
                new ObjectParameter("targetNameSpace", typeof(string));
    
            var transformParameter = transform != null ?
                new ObjectParameter("Transform", transform) :
                new ObjectParameter("Transform", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_RegisterInsert_Result>("sp_RegisterInsert", schemaNameParameter, schemaParameter, targetNameSpaceParameter, transformParameter);
        }
    
        public virtual ObjectResult<sp_RegisterSelect_Result> sp_RegisterSelect()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_RegisterSelect_Result>("sp_RegisterSelect");
        }
    
        public virtual ObjectResult<sp_CheckUnApprove_Result> sp_CheckUnApprove(string groupPermission)
        {
            var groupPermissionParameter = groupPermission != null ?
                new ObjectParameter("groupPermission", groupPermission) :
                new ObjectParameter("groupPermission", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_CheckUnApprove_Result>("sp_CheckUnApprove", groupPermissionParameter);
        }
    
        public virtual ObjectResult<sp_DocDetailSelect_Result> sp_DocDetailSelect()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_DocDetailSelect_Result>("sp_DocDetailSelect");
        }
    
        public virtual int sp_DocDetailInsert(string subjectName, string dataRequest, Nullable<System.DateTime> dateRequest, string empId, string statusId, Nullable<int> regId)
        {
            var subjectNameParameter = subjectName != null ?
                new ObjectParameter("subjectName", subjectName) :
                new ObjectParameter("subjectName", typeof(string));
    
            var dataRequestParameter = dataRequest != null ?
                new ObjectParameter("dataRequest", dataRequest) :
                new ObjectParameter("dataRequest", typeof(string));
    
            var dateRequestParameter = dateRequest.HasValue ?
                new ObjectParameter("dateRequest", dateRequest) :
                new ObjectParameter("dateRequest", typeof(System.DateTime));
    
            var empIdParameter = empId != null ?
                new ObjectParameter("empId", empId) :
                new ObjectParameter("empId", typeof(string));
    
            var statusIdParameter = statusId != null ?
                new ObjectParameter("statusId", statusId) :
                new ObjectParameter("statusId", typeof(string));
    
            var regIdParameter = regId.HasValue ?
                new ObjectParameter("regId", regId) :
                new ObjectParameter("regId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_DocDetailInsert", subjectNameParameter, dataRequestParameter, dateRequestParameter, empIdParameter, statusIdParameter, regIdParameter);
        }
    
        public virtual int sp_EmpPermissionInsert(Nullable<int> regId, string groupPermission)
        {
            var regIdParameter = regId.HasValue ?
                new ObjectParameter("regId", regId) :
                new ObjectParameter("regId", typeof(int));
    
            var groupPermissionParameter = groupPermission != null ?
                new ObjectParameter("groupPermission", groupPermission) :
                new ObjectParameter("groupPermission", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_EmpPermissionInsert", regIdParameter, groupPermissionParameter);
        }
    
        public virtual ObjectResult<sp_selectWaitingStatus_Result> sp_selectWaitingStatus(Nullable<int> regId)
        {
            var regIdParameter = regId.HasValue ?
                new ObjectParameter("regId", regId) :
                new ObjectParameter("regId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_selectWaitingStatus_Result>("sp_selectWaitingStatus", regIdParameter);
        }
    
        public virtual int sp_ApproveOrRejectUpdate(Nullable<int> docDetailId, string statusChange)
        {
            var docDetailIdParameter = docDetailId.HasValue ?
                new ObjectParameter("docDetailId", docDetailId) :
                new ObjectParameter("docDetailId", typeof(int));
    
            var statusChangeParameter = statusChange != null ?
                new ObjectParameter("statusChange", statusChange) :
                new ObjectParameter("statusChange", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_ApproveOrRejectUpdate", docDetailIdParameter, statusChangeParameter);
        }
    
        public virtual ObjectResult<sp_getDocNameByUserID_Result> sp_getDocNameByUserID(string groupPermission)
        {
            var groupPermissionParameter = groupPermission != null ?
                new ObjectParameter("groupPermission", groupPermission) :
                new ObjectParameter("groupPermission", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_getDocNameByUserID_Result>("sp_getDocNameByUserID", groupPermissionParameter);
        }
    
        public virtual int sp_historyInsert(Nullable<int> docDetailId, string statusId, string groupPermission)
        {
            var docDetailIdParameter = docDetailId.HasValue ?
                new ObjectParameter("docDetailId", docDetailId) :
                new ObjectParameter("docDetailId", typeof(int));
    
            var statusIdParameter = statusId != null ?
                new ObjectParameter("statusId", statusId) :
                new ObjectParameter("statusId", typeof(string));
    
            var groupPermissionParameter = groupPermission != null ?
                new ObjectParameter("groupPermission", groupPermission) :
                new ObjectParameter("groupPermission", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_historyInsert", docDetailIdParameter, statusIdParameter, groupPermissionParameter);
        }
    
        public virtual ObjectResult<sp_HistoryApproving_Result> sp_HistoryApproving(string groupPermission)
        {
            var groupPermissionParameter = groupPermission != null ?
                new ObjectParameter("groupPermission", groupPermission) :
                new ObjectParameter("groupPermission", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_HistoryApproving_Result>("sp_HistoryApproving", groupPermissionParameter);
        }
    
        public virtual ObjectResult<sp_showApprovedetail_Result> sp_showApprovedetail(Nullable<int> docDetailId)
        {
            var docDetailIdParameter = docDetailId.HasValue ?
                new ObjectParameter("docDetailId", docDetailId) :
                new ObjectParameter("docDetailId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_showApprovedetail_Result>("sp_showApprovedetail", docDetailIdParameter);
        }
    
        public virtual ObjectResult<sp_getAllDocDetailByUserID_Result> sp_getAllDocDetailByUserID(string groupPermission)
        {
            var groupPermissionParameter = groupPermission != null ?
                new ObjectParameter("groupPermission", groupPermission) :
                new ObjectParameter("groupPermission", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_getAllDocDetailByUserID_Result>("sp_getAllDocDetailByUserID", groupPermissionParameter);
        }
    }
}