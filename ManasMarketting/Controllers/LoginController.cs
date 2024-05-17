using BusinessLogic.Repository;
using ManasMarketting.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Web;


namespace ManasMarketting.Controllers
{
    public class LoginController : Controller
    {
        readonly ILogin _iLogin;
        readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        readonly ISession _session;
        public LoginController(ILogin iLogin, IHttpContextAccessor accessor, IWebHostEnvironment env, IHttpContextAccessor session)
        {
            _iLogin = iLogin;
            _accessor = accessor;
            _env = env;
            _session = session.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewMoodel req)
        {          
            var res = _iLogin.Login(req);
            if (res.emp_id > 0)
            {
                _session.SetInt32("emp_id", res.emp_id);
                _session.SetString("emp_name", res.emp_name);
                _session.SetInt32("role_id", res.role_id);
                return Json(new { status = 1 });
            }
            else
            {
                return Json(new { status = 0, msg="Invalid Login" });
            }
        }

        public IActionResult Logout()
        {
            _session.Clear();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult SendOTP(string email)
        {
            var res = _iLogin.SendOTP(email);
            if (res.status>0)
            {
                _session.SetInt32("email_id", res.status);    
            }
            return Json(res);
        }

        [HttpPost]
        public IActionResult VerifyOTP(LoginViewMoodel req)
        {
            var res = _iLogin.VerifyOTP(req);
            if (res.status > 0)
            {
                _session.SetInt32("email_id", res.status);
            }
            return Json(res);
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {    
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(LoginViewMoodel req)
        {
            req.emp_id=Convert.ToInt32(_session.GetInt32("emp_id"));
            var res = _iLogin.ChangePassword(req);
            return Json(res);
        }
    }
}
