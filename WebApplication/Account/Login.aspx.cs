using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace WebApplication
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Profile.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryAdminConnectionString"].ConnectionString;
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            bool isAuthenticated = false;
            string fullName = "";
            string userType = "Reader";
            string userrole = "";
            string userindex = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Проверьте, является ли пользователь читателем:
                string readersQuery = "SELECT r.Index_Reader, r.Login, r.Password, i.Surname, i.Name, q.Ticket_Number FROM Authorization_Readers r JOIN Readers i ON r.Index_Reader = i.Index_Reader JOIN Tickets q ON r.Index_Reader = q.Index_Reader WHERE r.Login = @Login AND r.Password = @Password";
                using (SqlCommand cmd = new SqlCommand(readersQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isAuthenticated = true;
                            fullName = reader["Surname"].ToString() + " " + reader["Name"].ToString();
                            userrole = "User";
                            userindex = reader["Index_Reader"].ToString();

                            HttpCookie ticketNumberCookie = new HttpCookie("TicketNumber");
                            ticketNumberCookie.Value = CookieSec.Encrypt(reader["Ticket_Number"].ToString());
                            ticketNumberCookie.Expires = DateTime.Now.AddDays(7); // срок хранения - 7 дней
                            Response.Cookies.Add(ticketNumberCookie);
                        }
                    }
                }

                // Если пользователь не является читателем, проверьте, является ли он сотрудником:
                if (!isAuthenticated)
                {
                    string employeesQuery = "SELECT e.Index_Employee, e.Login, e.Password, e.Role, i.Surname, i.Name FROM Authorization_Employees e JOIN Employees i ON e.Index_Employee = i.Index_Employee WHERE e.Login = @Login AND e.Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(employeesQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Login", login);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isAuthenticated = true;
                                fullName = $"{reader["Surname"]} {reader["Name"]}";
                                userType = "Employee";
                                userrole = reader["Role"].ToString();
                                userindex = reader["Index_Employee"].ToString();
                            }
                        }
                    }
                }
            }
            if (isAuthenticated)
            {

                HttpCookie userRoleCookie = new HttpCookie("UserRole");
                userRoleCookie.Value = CookieSec.Encrypt(userrole);
                userRoleCookie.Expires = DateTime.Now.AddDays(7); // срок хранения - 7 дней
                Response.Cookies.Add(userRoleCookie);

                HttpCookie userIndexCookie = new HttpCookie("UserIndex");
                userIndexCookie.Value = CookieSec.Encrypt(userindex);
                userIndexCookie.Expires = DateTime.Now.AddDays(7); // срок хранения - 7 дней
                Response.Cookies.Add(userIndexCookie);

                var identity = new ClaimsIdentity("ApplicationCookie");
                identity.AddClaim(new Claim(ClaimTypes.Name, fullName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, login));
                identity.AddClaim(new Claim("UserType", userType));
                if (userrole != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, userrole));
                }
                var ctx = HttpContext.Current.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
                var url = RouteTable.Routes.GetVirtualPath(null, "Profile", null).VirtualPath;
                Response.Redirect(url);
            }
            else
            {
                // Сообщение об ошибке илидругая обратная связь в случае неудачного входа.
                LabelWarning.Text = "Неверные логин и/или пароль";
            }
        }

        protected void ButtonRegistr_Click(object sender, EventArgs e)
        {
            var url = RouteTable.Routes.GetVirtualPath(null, "Registration", null).VirtualPath;
            Response.Redirect(url);
        }
    }
}