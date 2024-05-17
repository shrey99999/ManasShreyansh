using ManasMarketting.Models;         

namespace BusinessLogic.Repository
{
    public interface ILogin
    {
        LoginViewMoodel Login(LoginViewMoodel req);
        public Response VerifyOTP(LoginViewMoodel req);
        public Response SendOTP(string email);
        public Response ChangePassword(LoginViewMoodel req);
    }
}
