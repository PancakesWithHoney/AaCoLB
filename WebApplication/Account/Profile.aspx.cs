using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                string userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
                lblUserName.Text = userName;
                UserRoleLabel.Text = "Роль на сайте: " + CookieSec.Decrypt(Request.Cookies["UserRole"].Value);
                if (CookieSec.Decrypt(Request.Cookies["UserRole"].Value) == "User")
                {
                    UserRoleLabel.Text = "Роль на сайте: Читатель";
                    string ticketNumber = CookieSec.Decrypt(Request.Cookies["TicketNumber"].Value);
                    Label1.Text = ticketNumber;
                    string connectionString = ConfigurationManager.ConnectionStrings["LibraryUserConnectionString"].ConnectionString;
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {

                        sqlCon.Open();
                        string query = "SELECT FORMAT(End_Date,'dd.MM.yyyy') AS End_Date from Tickets where Ticket_Number = @Ticket_Number";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Ticket_Number", CookieSec.Decrypt(Request.Cookies["TicketNumber"].Value));
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime dateCheck = Convert.ToDateTime(reader["End_Date"]);
                                if (DateTime.Today > dateCheck)
                                {
                                    LabelData.Visible = true; LabelData.Text = reader["End_Date"].ToString()+" ";
                                    Label2.Visible = true; ButtonCon.Visible = true;
                                }
                                else
                                {
                                    LabelData.Visible = true; LabelData.Text = reader["End_Date"].ToString() + " ";
                                    Label2.Text = "Дата окончания читательского билета: "; ButtonCon.Visible = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ButtonChange.Visible = false;
                    if (CookieSec.Decrypt(Request.Cookies["UserRole"].Value) == "Employee")
                        UserRoleLabel.Text = "Роль на сайте: Сотрудник";
                    else
                        UserRoleLabel.Text = "Роль на сайте: Администратор";
                    LabelData.Visible = false ;
                    Label5.Visible  = false;
                    Label2.Visible = false; ButtonCon.Visible = false;
                    Label1.Visible = false;
                    GridView1.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void ButtonCon_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryUserConnectionString"].ConnectionString;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Tickets SET End_Date = @End_Date WHERE Ticket_Number = @Ticket_Number";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Ticket_Number", CookieSec.Decrypt(Request.Cookies["TicketNumber"].Value));
                sqlCmd.Parameters.AddWithValue("@End_Date", DateTime.Today.AddMonths(12).ToString());
                LabelData.Text = DateTime.Today.AddMonths(12).ToString();
                sqlCmd.ExecuteNonQuery();
            }
            ButtonCon.Visible = false;
            Label2.Text = "Дата окончания читательского билета: "; ButtonCon.Visible = false;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string dateString = DataBinder.Eval(e.Row.DataItem, "Дата окончания срока выдачи книги").ToString();
                DateTime endDate = DateTime.ParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                bool isReturned = (bool)DataBinder.Eval(e.Row.DataItem, "Книга возвращена");

                if (endDate < DateTime.Now && !isReturned)
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/LoginGhange.aspx");
        }
    }
}