using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WebApplication
{
    public partial class AuthorizationEmployeesFull : System.Web.UI.Page
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Authorization_Employees", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvAuthorizationEmployees.DataSource = dtbl;
                gvAuthorizationEmployees.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvAuthorizationEmployees.DataSource = dtbl;
                gvAuthorizationEmployees.DataBind();
                gvAuthorizationEmployees.Rows[0].Cells.Clear();
                gvAuthorizationEmployees.Rows[0].Cells.Add(new TableCell());
                gvAuthorizationEmployees.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvAuthorizationEmployees.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvAuthorizationEmployees.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void gvAuthorizationEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("AddNew"))
                {
                    if (((gvAuthorizationEmployees.FooterRow.FindControl("txtIndex_EmployeeFooter") as TextBox).Text != "") && ((gvAuthorizationEmployees.FooterRow.FindControl("txtLoginFooter") as TextBox).Text != "") && ((gvAuthorizationEmployees.FooterRow.FindControl("txtPasswordFooter") as TextBox).Text != "") && ((gvAuthorizationEmployees.FooterRow.FindControl("txtRoleFooter") as TextBox).Text != ""))
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            string query = "INSERT INTO Authorization_Employees (Index_Employee, Login, Password, Role) VALUES (@Index_Employee, @Login, @Password, @Role)";
                            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@Index_Employee", (gvAuthorizationEmployees.FooterRow.FindControl("txtIndex_EmployeeFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Login", (gvAuthorizationEmployees.FooterRow.FindControl("txtLoginFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Password", (gvAuthorizationEmployees.FooterRow.FindControl("txtPasswordFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Role", (gvAuthorizationEmployees.FooterRow.FindControl("txtRoleFooter") as TextBox).Text.Trim());

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
        protected void gvAuthorizationEmployees_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                gvAuthorizationEmployees.EditIndex = e.NewEditIndex;
                string search = TextBox1.Text;
                int sech; Int32.TryParse(search, out sech);
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Authorization_Employees WHERE Index_Employee = '" + sech + "'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvAuthorizationEmployees.DataSource = dtbl;
                    gvAuthorizationEmployees.DataBind();
                }
            }
            else
            {
                gvAuthorizationEmployees.EditIndex = e.NewEditIndex;
                PopulateGridview();
            }
        }//
        protected void gvAuthorizationEmployees_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAuthorizationEmployees.EditIndex = -1;
            PopulateGridview();
        }
        protected void gvAuthorizationEmployees_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Authorization_Employees SET Index_Employee = @Index_Employee,Login = @Login, Password = @Password, Role = @Role WHERE Index_Employee = @id";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvAuthorizationEmployees.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.Parameters.AddWithValue("@Index_Employee", (gvAuthorizationEmployees.Rows[e.RowIndex].FindControl("txtIndex_Employee") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Login", (gvAuthorizationEmployees.Rows[e.RowIndex].FindControl("txtLogin") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Password", (gvAuthorizationEmployees.Rows[e.RowIndex].FindControl("txtPassword") as TextBox).Text);
                    sqlCmd.Parameters.AddWithValue("@Role", (gvAuthorizationEmployees.Rows[e.RowIndex].FindControl("txtRole") as TextBox).Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    gvAuthorizationEmployees.EditIndex = -1;
                    PopulateGridview();
                    lblErrorMessage.Text = "";
                    TextBox1.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvAuthorizationEmployees_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Authorization_Employees WHERE Index_Employee = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvAuthorizationEmployees.DataKeys[e.RowIndex].Value.ToString()));
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Authorization_Employees WHERE Index_Employee = '" + sech + "'", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvAuthorizationEmployees.DataSource = dtbl;
                gvAuthorizationEmployees.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvAuthorizationEmployees.DataSource = dtbl;
                gvAuthorizationEmployees.DataBind();
                gvAuthorizationEmployees.Rows[0].Cells.Clear();
                gvAuthorizationEmployees.Rows[0].Cells.Add(new TableCell());
                gvAuthorizationEmployees.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvAuthorizationEmployees.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvAuthorizationEmployees.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void ButtonCansel_Click(object sender, ImageClickEventArgs e)
        {
            TextBox1.Text = "";
            PopulateGridview();
        }
    }
}