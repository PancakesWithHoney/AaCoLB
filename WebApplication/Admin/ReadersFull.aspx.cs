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
    public partial class ReadersFull : System.Web.UI.Page
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Readers", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvReaders.DataSource = dtbl;
                gvReaders.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvReaders.DataSource = dtbl;
                gvReaders.DataBind();
                gvReaders.Rows[0].Cells.Clear();
                gvReaders.Rows[0].Cells.Add(new TableCell());
                gvReaders.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvReaders.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvReaders.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void gvReaders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("AddNew"))
                {
                    if (((gvReaders.FooterRow.FindControl("txtSurnameFooter") as TextBox).Text != "") && ((gvReaders.FooterRow.FindControl("txtNameFooter") as TextBox).Text != "") && ((gvReaders.FooterRow.FindControl("txtBirthdateFooter") as TextBox).Text != "") && ((gvReaders.FooterRow.FindControl("txtPassportFooter") as TextBox).Text != "") && ((gvReaders.FooterRow.FindControl("txtRegistrationFooter") as TextBox).Text != ""))
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            string query = "INSERT INTO Readers (Surname, Name, Patronymic, Birthdate, Passport, Registration) VALUES (@Surname, @Name, @Patronymic, @Birthdate, @Passport, @Registration)";
                            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@Surname", (gvReaders.FooterRow.FindControl("txtSurnameFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Name", (gvReaders.FooterRow.FindControl("txtNameFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Patronymic", (gvReaders.FooterRow.FindControl("txtPatronymicFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Birthdate", (gvReaders.FooterRow.FindControl("txtBirthdateFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Passport", (gvReaders.FooterRow.FindControl("txtPassportFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Registration", (gvReaders.FooterRow.FindControl("txtRegistrationFooter") as TextBox).Text.Trim());
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
        protected void gvReaders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                gvReaders.EditIndex = e.NewEditIndex;
                string search = TextBox1.Text;
                int sech; Int32.TryParse(search, out sech);
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Readers WHERE Index_Reader = '" + sech + "' or Surname like '%" + search + "%' or Name like '%" + search + "%' or Passport like '%" + search + "%'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvReaders.DataSource = dtbl;
                    gvReaders.DataBind();
                }
            }
            else
            {
                gvReaders.EditIndex = e.NewEditIndex;
                PopulateGridview();
            }
        }
        protected void gvReaders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvReaders.EditIndex = -1;
            PopulateGridview();
        }
        protected void gvReaders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Readers SET Surname = @Surname, Name = @Name, Patronymic = @Patronymic, Birthdate = @Birthdate, Passport = @Passport, Registration = @Registration WHERE Index_Reader = @id";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Surname", (gvReaders.Rows[e.RowIndex].FindControl("txtSurname") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Name", (gvReaders.Rows[e.RowIndex].FindControl("txtName") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Patronymic", (gvReaders.Rows[e.RowIndex].FindControl("txtPatronymic") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Birthdate", (gvReaders.Rows[e.RowIndex].FindControl("txtBirthdate") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Passport", (gvReaders.Rows[e.RowIndex].FindControl("txtPassport") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Registration", (gvReaders.Rows[e.RowIndex].FindControl("txtRegistration") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvReaders.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    gvReaders.EditIndex = -1;
                    PopulateGridview();
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvReaders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Readers WHERE Index_Reader = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvReaders.DataKeys[e.RowIndex].Value.ToString()));
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Readers WHERE Index_Reader = '" + sech + "' or Surname like '%" + search + "%' or Name like '%" + search + "%' or Passport like '%" + search + "%'", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvReaders.DataSource = dtbl;
                gvReaders.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvReaders.DataSource = dtbl;
                gvReaders.DataBind();
                gvReaders.Rows[0].Cells.Clear();
                gvReaders.Rows[0].Cells.Add(new TableCell());
                gvReaders.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvReaders.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvReaders.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void ButtonCansel_Click(object sender, ImageClickEventArgs e)
        {
            TextBox1.Text = "";
            PopulateGridview();
        }
    }
}