using System;
using System.Web;
using System.Web.Security;

namespace WebApplication
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                var ctx = HttpContext.Current.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignOut("ApplicationCookie");
                foreach (var cookie in Request.Cookies.AllKeys)
                {
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();

            }            
            Response.Redirect("~/");
        }
    }
}