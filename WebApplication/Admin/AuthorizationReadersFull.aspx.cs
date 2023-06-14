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
    public partial class AuthorizationReadersFull : System.Web.UI.Page
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Authorization_Readers", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvAuthorizationReaders.DataSource = dtbl;
                gvAuthorizationReaders.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvAuthorizationReaders.DataSource = dtbl;
                gvAuthorizationReaders.DataBind();
                gvAuthorizationReaders.Rows[0].Cells.Clear();
                gvAuthorizationReaders.Rows[0].Cells.Add(new TableCell());
                gvAuthorizationReaders.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvAuthorizationReaders.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvAuthorizationReaders.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void gvAuthorizationReaders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("AddNew"))
                {
                    if (((gvAuthorizationReaders.FooterRow.FindControl("txtTicket_NumberFooter") as TextBox).Text != "") && ((gvAuthorizationReaders.FooterRow.FindControl("txtIndex_ReaderFooter") as TextBox).Text != ""))
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            string query = "INSERT INTO Authorization_Readers (Index_Reader, Login, Password) VALUES (@Index_Reader, @Login, @Password)";
                            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@Index_Reader", (gvAuthorizationReaders.FooterRow.FindControl("txtIndex_ReaderFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Login", (gvAuthorizationReaders.FooterRow.FindControl("txtLoginFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Password", (gvAuthorizationReaders.FooterRow.FindControl("txtPasswordFooter") as TextBox).Text.Trim());

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
        protected void gvAuthorizationReaders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                gvAuthorizationReaders.EditIndex = e.NewEditIndex;
                string search = TextBox1.Text;
                int sech; Int32.TryParse(search, out sech);
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Authorization_Readers WHERE Index_Reader = '" + sech + "'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvAuthorizationReaders.DataSource = dtbl;
                    gvAuthorizationReaders.DataBind();
                }
            }
            else
            {
                gvAuthorizationReaders.EditIndex = e.NewEditIndex;
                PopulateGridview();
            }
        }//
        protected void gvAuthorizationReaders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAuthorizationReaders.EditIndex = -1;
            PopulateGridview();
        }
        protected void gvAuthorizationReaders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Authorization_Readers SET Index_Reader = @Index_Reader,Login = @Login, Password = @Password WHERE Index_Reader = @id";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvAuthorizationReaders.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.Parameters.AddWithValue("@Index_Reader", (gvAuthorizationReaders.Rows[e.RowIndex].FindControl("txtIndex_Reader") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Login", (gvAuthorizationReaders.Rows[e.RowIndex].FindControl("txtLogin") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Password", (gvAuthorizationReaders.Rows[e.RowIndex].FindControl("txtPassword") as TextBox).Text);
                    sqlCmd.ExecuteNonQuery();
                    gvAuthorizationReaders.EditIndex = -1;
                    PopulateGridview();
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvAuthorizationReaders_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Authorization_Readers WHERE Index_Reader = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvAuthorizationReaders.DataKeys[e.RowIndex].Value.ToString()));
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Authorization_Readers WHERE Index_Reader = '" + sech + "'", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvAuthorizationReaders.DataSource = dtbl;
                gvAuthorizationReaders.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvAuthorizationReaders.DataSource = dtbl;
                gvAuthorizationReaders.DataBind();
                gvAuthorizationReaders.Rows[0].Cells.Clear();
                gvAuthorizationReaders.Rows[0].Cells.Add(new TableCell());
                gvAuthorizationReaders.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvAuthorizationReaders.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvAuthorizationReaders.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void ButtonCansel_Click(object sender, ImageClickEventArgs e)
        {
            TextBox1.Text = "";
            PopulateGridview();
        }
    }
}