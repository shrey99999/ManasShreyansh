
using BusinessLogic.Repository;
using ManasMarketting.Models;
using NuGet.Protocol.Plugins;
using System.Data.SqlClient;

namespace BusinessLogic.Concrete
{
    public class LoginConcrete:ILogin
    {
        readonly IConnectionString _IConn;
        readonly ICommonFunction _ICommom;
        readonly IDAL _IDAL;
        readonly IWebHostEnvironment _env;
        readonly IHttpContextAccessor _accesssor;
        public LoginConcrete(IHttpContextAccessor accesssor, IWebHostEnvironment env)
        {
            _IConn = new ConnectionStr(accesssor, env);
            _ICommom = new CommonFunctionConcrete();
            _IDAL = new DAL(_IConn.GetConnectionString());
        }
        public LoginViewMoodel Login(LoginViewMoodel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@u_id", req.u_id),
                new SqlParameter("@password", req.password)
            };

            var dt = _IDAL.GetByProcedureAdapter("proc_login", param);
            LoginViewMoodel res = new LoginViewMoodel();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<LoginViewMoodel>(dt)[0];
            }
            return res;
        }
        public Response SendOTP(string email)
        {
            SqlParameter[] param = {
             
                new SqlParameter("@email_id", email),
            };

            var dt = _IDAL.GetByProcedureAdapter("proc_GetEmail_tbl_master_registration", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }

        public Response VerifyOTP(LoginViewMoodel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@otp", req.otp),
                new SqlParameter("@email", req.email),
                new SqlParameter("@password", req.NewPassword),
            };

            var dt = _IDAL.GetByProcedureAdapter("proc_verify_forgetpassword_otp", param);
            Response res = new Response();
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }
        public Response ChangePassword(LoginViewMoodel req)
        {
            SqlParameter[] param = {
                new SqlParameter("@email", req.email),
                new SqlParameter("@emp_id", req.emp_id),
                new SqlParameter("@newpassword", req.NewPassword)
            };

            Response res = new Response();
            var dt = _IDAL.GetByProcedureAdapter("proc_emp_changepassword", param);
            if (dt != null && dt.Rows.Count > 0)
            {
                res = _ICommom.ConvertDataTable<Response>(dt)[0];
            }
            return res;
        }


    }
}
