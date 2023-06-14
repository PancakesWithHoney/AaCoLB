using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Routing;

namespace WebApplication
{
    public partial class RegistrationFinale : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Profile.aspx");
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryAdminConnectionString"].ConnectionString;
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;
            string confirmpassword = ConfirmPasswordTextBox.Text;
            if ((login != "") && (password != "") && (confirmpassword != ""))
            {
                if (password == confirmpassword)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string readersQuery = "SELECT COUNT(*) from Authorization_Readers WHERE Login = @Login";
                        using (SqlCommand cmd = new SqlCommand(readersQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Login", login);

                            int count = (int)cmd.ExecuteScalar();
                            if (count == 0)
                            {
                                string insert = "INSERT INTO Authorization_Readers (Index_Reader, Login, Password) VALUES (@Id,@Login ,@Password);";
                                using (SqlCommand cmd1 = new SqlCommand(insert, connection))
                                {
                                    cmd1.Parameters.AddWithValue("@Id", Session["id_reader"].ToString().Trim());
                                    cmd1.Parameters.AddWithValue("@Login", login.Trim());
                                    cmd1.Parameters.AddWithValue("@Password", password.Trim());
                                    cmd1.ExecuteNonQuery();
                                    var url1 = RouteTable.Routes.GetVirtualPath(null, "Login", null).VirtualPath;
                                    Response.Redirect(url1);
                                }
                            }
                            else
                            {
                               LabelWarning.Text = "Такой логин уже занят!";
                            }
                        }
                    }
                }
                else
                {
                    LabelWarning.Text = "Пароли не совпадают!";
                }
            }
            else
                LabelWarning.Text = "Поля не должны быть пустыми!";
        }
    }
}