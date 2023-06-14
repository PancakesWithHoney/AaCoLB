using System;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication
{
    public partial class EmployeesFull : System.Web.UI.Page
    {

        string connectionString = ConfigurationManager.ConnectionStrings["LibraryAdminConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;

            if (user == null || user.FindFirst(ClaimTypes.Role)?.Value != "Admin")
            {
                // Перенаправление на страницу "Недостаточно прав доступа" или любую другую страницу.
                Response.Redirect("~/Default.aspx");
            }
            if (!IsPostBack)
            {
                PopulateGridview();
            }
        }
        void PopulateGridview()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT Index_Employee, Surname, Name, Patronymic, FORMAT(Birthdate, 'dd.MM.yyyy') as Birthdate, Passport, Registration FROM Employees", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvEmployees.DataSource = dtbl;
                gvEmployees.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvEmployees.DataSource = dtbl;
                gvEmployees.DataBind();
                gvEmployees.Rows[0].Cells.Clear();
                gvEmployees.Rows[0].Cells.Add(new TableCell());
                gvEmployees.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvEmployees.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvEmployees.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("AddNew"))
                {
                    if (((gvEmployees.FooterRow.FindControl("txtSurnameFooter") as TextBox).Text != "") && ((gvEmployees.FooterRow.FindControl("txtNameFooter") as TextBox).Text != "") && ((gvEmployees.FooterRow.FindControl("txtBirthdateFooter") as TextBox).Text != "") && ((gvEmployees.FooterRow.FindControl("txtPassportFooter") as TextBox).Text != "") && ((gvEmployees.FooterRow.FindControl("txtRegistrationFooter") as TextBox).Text != ""))
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            string query = "INSERT INTO Employees (Surname, Name, Patronymic, Birthdate, Passport, Registration) VALUES (@Surname, @Name, @Patronymic, @Birthdate, @Passport, @Registration)";
                            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@Surname", (gvEmployees.FooterRow.FindControl("txtSurnameFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Name", (gvEmployees.FooterRow.FindControl("txtNameFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Patronymic", (gvEmployees.FooterRow.FindControl("txtPatronymicFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Birthdate", (gvEmployees.FooterRow.FindControl("txtBirthdateFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Passport", (gvEmployees.FooterRow.FindControl("txtPassportFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Registration", (gvEmployees.FooterRow.FindControl("txtRegistrationFooter") as TextBox).Text.Trim());
                            sqlCmd.ExecuteNonQuery();
                            PopulateGridview();
                            lblErrorMessage.Text = "";
                        }
                    }
                }
                else
                {
                    lblErrorMessage.Text = "Важные поля пусты";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvEmployees_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                gvEmployees.EditIndex = e.NewEditIndex;
                string search = TextBox1.Text;
                int sech; Int32.TryParse(search, out sech);
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Employees WHERE Index_Employee = '" + sech + "' or Surname like '%" + search + "%' or Name like '%" + search + "%' or Passport like '%" + search + "%'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvEmployees.DataSource = dtbl;
                    gvEmployees.DataBind();
                }
            }
            else
            {
                gvEmployees.EditIndex = e.NewEditIndex;
                PopulateGridview();
            }
        }
        protected void gvEmployees_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEmployees.EditIndex = -1;
            PopulateGridview();
        }
        protected void gvEmployees_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Employees SET Surname = @Surname, Name = @Name, Patronymic = @Patronymic, Birthdate = @Birthdate, Passport = @Passport, Registration = @Registration WHERE Index_Employee = @id";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Surname", (gvEmployees.Rows[e.RowIndex].FindControl("txtSurname") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Name", (gvEmployees.Rows[e.RowIndex].FindControl("txtName") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Patronymic", (gvEmployees.Rows[e.RowIndex].FindControl("txtPatronymic") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Birthdate", (gvEmployees.Rows[e.RowIndex].FindControl("txtBirthdate") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Passport", (gvEmployees.Rows[e.RowIndex].FindControl("txtPassport") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Registration", (gvEmployees.Rows[e.RowIndex].FindControl("txtRegistration") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvEmployees.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    gvEmployees.EditIndex = -1;
                    PopulateGridview();
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvEmployees_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Employees WHERE Index_Employee = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvEmployees.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    PopulateGridview();
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }

        protected void ButtonFind_Click(object sender, ImageClickEventArgs e)
        {
            string search = TextBox1.Text;
            int sech; Int32.TryParse(search, out sech);
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Employees WHERE Index_Employee = '" + sech + "' or Surname like '%" + search + "%' or Name like '%" + search + "%' or Passport like '%" + search + "%'", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvEmployees.DataSource = dtbl;
                gvEmployees.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvEmployees.DataSource = dtbl;
                gvEmployees.DataBind();
                gvEmployees.Rows[0].Cells.Clear();
                gvEmployees.Rows[0].Cells.Add(new TableCell());
                gvEmployees.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvEmployees.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvEmployees.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void ButtonCansel_Click(object sender, ImageClickEventArgs e)
        {
            TextBox1.Text = "";
            PopulateGridview();
        }
    }
}