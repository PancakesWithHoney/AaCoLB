using System;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;

namespace WebApplication
{
    public partial class Books : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["LibraryEmployeeConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;

            if (user == null || user.FindFirst(ClaimTypes.Role)?.Value != "Employee")
            {
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Books", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvBooks.DataSource = dtbl;
                gvBooks.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvBooks.DataSource = dtbl;
                gvBooks.DataBind();
                gvBooks.Rows[0].Cells.Clear();
                gvBooks.Rows[0].Cells.Add(new TableCell());
                gvBooks.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvBooks.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvBooks.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void gvBooks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    if (((gvBooks.FooterRow.FindControl("txtNameFooter") as TextBox).Text == "") || ((gvBooks.FooterRow.FindControl("txtAuthorFooter") as TextBox).Text == "") || ((gvBooks.FooterRow.FindControl("txtIn_StockFooter") as TextBox).Text == "") || ((gvBooks.FooterRow.FindControl("txtIn_TotalFooter") as TextBox).Text == ""))
                    {
                        lblErrorMessage.Text = "Поля не могут быть пустыми.";
                    }
                    else
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            string query = "INSERT INTO Books (Name, Author, In_Stock, In_Total) VALUES (@Name, @Author, @In_Stock, @In_Total)";
                            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@Name", (gvBooks.FooterRow.FindControl("txtNameFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Author", (gvBooks.FooterRow.FindControl("txtAuthorFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@In_Stock", (gvBooks.FooterRow.FindControl("txtIn_StockFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@In_Total", (gvBooks.FooterRow.FindControl("txtIn_TotalFooter") as TextBox).Text.Trim());
                            sqlCmd.ExecuteNonQuery();
                            PopulateGridview();
                            lblErrorMessage.Text = "";
                        }
                    }
                }
            }
            catch
            {
                lblErrorMessage.Text = "Добавить данные не удалось. Проверьте корректность введённых данных и попробуйте снова.";
            }
        }
        protected void gvBooks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                gvBooks.EditIndex = e.NewEditIndex;
                string search = TextBox1.Text;
                int sech; Int32.TryParse(search, out sech);
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Books where Name like'%" + search + "%' or Author like'%" + search + "%' or Index_Book like'%" + sech + "%'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvBooks.DataSource = dtbl;
                    gvBooks.DataBind();
                }
            }
            else
            {
                gvBooks.EditIndex = e.NewEditIndex;
                PopulateGridview();
            }
        }
        protected void gvBooks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBooks.EditIndex = -1;
            PopulateGridview();
        }
        protected void gvBooks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                if (((gvBooks.Rows[e.RowIndex].FindControl("txtName") as TextBox).Text == "") || ((gvBooks.Rows[e.RowIndex].FindControl("txtAuthor") as TextBox).Text == "") || ((gvBooks.Rows[e.RowIndex].FindControl("txtIn_Stock") as TextBox).Text == "") || ((gvBooks.Rows[e.RowIndex].FindControl("txtIn_Total") as TextBox).Text == ""))
                {
                    lblErrorMessage.Text = "Поля не могут быть пустыми.";
                }
                else
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        string query = "UPDATE Books SET Name = @Name, Author = @Author, In_Stock = @In_Stock, In_Total = @In_Total WHERE Index_Book = @Index_Book";

                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Name", (gvBooks.Rows[e.RowIndex].FindControl("txtName") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Author", (gvBooks.Rows[e.RowIndex].FindControl("txtAuthor") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@In_Stock", (gvBooks.Rows[e.RowIndex].FindControl("txtIn_Stock") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@In_Total", (gvBooks.Rows[e.RowIndex].FindControl("txtIn_Total") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Index_Book", Convert.ToInt32(gvBooks.DataKeys[e.RowIndex].Value.ToString()));
                        sqlCmd.ExecuteNonQuery();
                        gvBooks.EditIndex = -1;
                        PopulateGridview();
                        lblErrorMessage.Text = "";
                    }
                }
            }
            catch
            {
                lblErrorMessage.Text = "Обновить данные не удалось. Проверьте корректность введённых данных и попробуйте снова.";
            }
        }
        protected void gvBooks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Books WHERE Index_Book = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvBooks.DataKeys[e.RowIndex].Value.ToString()));
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Books where Name like'%" + search + "%' or Author like'%" + search + "%' or Index_Book like'%" + sech + "%'", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvBooks.DataSource = dtbl;
                gvBooks.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvBooks.DataSource = dtbl;
                gvBooks.DataBind();
                gvBooks.Rows[0].Cells.Clear();
                gvBooks.Rows[0].Cells.Add(new TableCell());
                gvBooks.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvBooks.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvBooks.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void ButtonCansel_Click(object sender, ImageClickEventArgs e)
        {
            TextBox1.Text = "";
            PopulateGridview();
        }
    }
}