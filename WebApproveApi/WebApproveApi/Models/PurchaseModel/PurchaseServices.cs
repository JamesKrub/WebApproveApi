using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApproveApi.Models.PurchaseModel
{
    public class PurchaseServices
    {
        SalePurchaseEntities _conn;
        SalePurchaseEntities conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new SalePurchaseEntities();
                }
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }
        public List<sp_PurchaseSelect_Result> getPurchase()
        {
            //using (SalePurchaseEntities conn = new SalePurchaseEntities()) {
            try
            {
                return conn.sp_PurchaseSelect().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}  
        }

        public Purchase getPurchasebyKey(string id)
        {
            try
            {
                return conn.Purchases.Where(a => a.purchaseId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int insertPurchase(PurchaseCriteria criteria)
        {
            try
            {
                conn.sp_PurchaseInsert(
                    criteria.purchaseId,
                    criteria.Name,
                    criteria.Date,
                    criteria.Price,
                    criteria.NumberQty
                );
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }

        }

        public int updatePurchase(PurchaseCriteria criteria, int id)
        {
            try
            {
                conn.sp_PurchaseUpdate(
                    criteria.purchaseId,
                    criteria.Name,
                    criteria.Date,
                    criteria.Price,
                    criteria.NumberQty
                );
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }

        }

        public int deletePurchase(string id)
        {
            try
            {
                conn.sp_PurchaseDelete(id);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
        }

        public List<sp_PurchaseSelectTtlMoneyEchMonthByYear_Result> getMoneyByMonth()
        {
            try
            {
                return conn.sp_PurchaseSelectTtlMoneyEchMonthByYear().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_PurchaseSelectYear_Result> getAllYear()
        {
            try
            {
                return conn.sp_PurchaseSelectYear().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class PurchaseCriteria
    {
        public string purchaseId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int NumberQty { get; set; }
    }
}