using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApproveApi.Models.PersonnelModel
{
    public class PersonnelService
    {
        PersonnelEntities _conn;
        PersonnelEntities conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new PersonnelEntities();
                }
                return _conn;
            }
            set
            {
                _conn = value;
            }
        }

        public string chkLogin(string un, string p)
        {
            try
            {
                //List<sp_chckLogin_Result> result = conn.sp_chckLogin().ToList();
                var qry = from ln in conn.Logins
                          where (ln.username == un)
                                && (ln.password == p)
                          select ln.empId;

                return qry.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}