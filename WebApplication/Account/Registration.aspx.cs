using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Routing;

namespace WebApplication
{
    public partial class Registration : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryAdminConnectionString"].ConnectionString;
            string number = TextBox1.Text;
            string surname = TextBox2.Text;
            string name = TextBox3.Text;
            string date = TextBox4.Text;

            bool CheckData = false;
            string surnameCheck = "";
            string nameCheck = "";
            string dateCheck = "";
            string logincheck = "";
            string id_reader = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string readersQuery = "SELECT r.Ticket_Number, i.Index_Reader, i.Surname, i.Name, FORMAT(i.Birthdate,'yyyy-MM-dd') AS BirthdateFormatted, ar.Login FROM Tickets r JOIN Readers i ON r.Index_Reader = i.Index_Reader LEFT JOIN Authorization_Readers ar ON i.Index_Reader = ar.Index_Reader WHERE r.Ticket_Number = @Number";
                using (SqlCommand cmd = new SqlCommand(readersQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Number", number);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            surnameCheck = reader["Surname"].ToString().Trim();
                            nameCheck = reader["Name"].ToString().Trim();
                            dateCheck = reader["BirthdateFormatted"].ToString().Trim();
                            logincheck = reader["Login"].ToString().Trim();
                            id_reader = reader["Index_Reader"].ToString();
                            Session["id_reader"] = id_reader;
                            if (logincheck.Length == 0)
                            {
                                if ((surname == surnameCheck) && (name == nameCheck) && (date == dateCheck))
                                {
                                    CheckData = true;
                                }
                            }
                        }
                    }
                }
            }
            if (CheckData)
            {
                var url1 = RouteTable.Routes.GetVirtualPath(null, "RegistrationFinale", null).VirtualPath;
                Response.Redirect(url1);
            }
            else
            {
                if (logincheck.Length != 0)
                {
                    LabelWarning.Text = "Пользователь уже зарегистрирован.";
                }
                else
                    LabelWarning.Text = "Введёные данные некорректны";
            }
        }
    }
}