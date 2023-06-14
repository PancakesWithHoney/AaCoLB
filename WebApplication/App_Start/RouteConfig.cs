using Microsoft.AspNet.FriendlyUrls;
using System.Web.Routing;

namespace WebApplication
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
            routes.MapPageRoute(
                "Login",
                "Login",
                "~/Account/Login.aspx"
            );
            routes.MapPageRoute(
                "Logout",
                "Logout",
                "~/Account/Logout.aspx"
            );
            routes.MapPageRoute(
                "Registration",
                "Registration",
                "~/Account/Registration.aspx"
            );
            routes.MapPageRoute(
                "RegistrationFinale",
                "RegistrationFinale",
                "~/Account/RegistrationFinale.aspx"
            );
            routes.MapPageRoute(
                "Profile",
                "Profile",
                "~/Account/Profile.aspx"
            );
        }
    }
}
