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
    public partial class EntriesFull : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["LibraryAdminConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;

            if (user == null || user.FindFirst(ClaimTypes.Role)?.Value != "Admin")
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT Number_Entry, Ticket_Number, Index_Book, FORMAT(Rent_Begin, 'dd.MM.yyyy') AS Rent_Begin, FORMAT(Rent_End, 'dd.MM.yyyy') AS Rent_End, Index_Employee, Lease_completed FROM Entries", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvEntries.DataSource = dtbl;
                gvEntries.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvEntries.DataSource = dtbl;
                gvEntries.DataBind();
                gvEntries.Rows[0].Cells.Clear();
                gvEntries.Rows[0].Cells.Add(new TableCell());
                gvEntries.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvEntries.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvEntries.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void gvEntries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    if (((gvEntries.FooterRow.FindControl("txtTicket_NumberFooter") as TextBox).Text == "") || ((gvEntries.FooterRow.FindControl("txtIndex_BookFooter") as TextBox).Text == ""))
                    {
                        lblErrorMessage.Text = "Поля не могут быть пустыми.";
                    }
                    else
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            string query = "INSERT INTO Entries ( Ticket_Number, Index_Book, Rent_Begin, Rent_End, Index_Employee, Lease_completed) VALUES (@Ticket_Number, @Index_Book, @Rent_Begin, @Rent_End, @Index_Employee, @Lease_completed)";
                            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@Ticket_Number", (gvEntries.FooterRow.FindControl("txtTicket_NumberFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Index_Book", (gvEntries.FooterRow.FindControl("txtIndex_BookFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Rent_Begin", (gvEntries.FooterRow.FindControl("txtRent_BeginFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Rent_End", (gvEntries.FooterRow.FindControl("txtRent_EndFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Index_Employee", (gvEntries.FooterRow.FindControl("txtIndex_EmployeeFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Lease_completed", (gvEntries.FooterRow.FindControl("txtLease_completedFooter") as TextBox).Text.Trim());
                            sqlCmd.ExecuteNonQuery();
                            PopulateGridview();
                            lblErrorMessage.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvEntries_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                gvEntries.EditIndex = e.NewEditIndex;
                string search = TextBox1.Text;
                int sech; Int32.TryParse(search, out sech);
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Entries where Rent_Begin = '" + search + "' or Rent_End = '" + search + "' or Index_Book like'%" + sech + "%' or Number_Entry like'%" + sech + "%' or Ticket_Number like'" + search + "'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvEntries.DataSource = dtbl;
                    gvEntries.DataBind();
                }
            }
            else
            {
                gvEntries.EditIndex = e.NewEditIndex;
                PopulateGridview();
            }
        }
        protected void gvEntries_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEntries.EditIndex = -1;
            PopulateGridview();
        }
        protected void gvEntries_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Entries SET Ticket_Number = @Ticket_Number, Index_Book = @Index_Book, Rent_Begin = @Rent_Begin, Rent_End = @Rent_End, Index_Employee = @Index_Employee, Lease_completed = @Lease_completed WHERE Number_Entry = @Number_Entry";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Ticket_Number", (gvEntries.Rows[e.RowIndex].FindControl("txtTicket_Number") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Index_Book", (gvEntries.Rows[e.RowIndex].FindControl("txtIndex_Book") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Rent_Begin", (gvEntries.Rows[e.RowIndex].FindControl("txtRent_Begin") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Rent_End", (gvEntries.Rows[e.RowIndex].FindControl("txtRent_End") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Index_Employee", (gvEntries.Rows[e.RowIndex].FindControl("txtIndex_Employee") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Lease_completed", (gvEntries.Rows[e.RowIndex].FindControl("txtLease_completed") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Number_Entry", Convert.ToInt32(gvEntries.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    gvEntries.EditIndex = -1;
                    PopulateGridview();
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvEntries_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Entries WHERE Number_Entry = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvEntries.DataKeys[e.RowIndex].Value.ToString()));
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
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Entries where Rent_Begin = '" + search + "' or Rent_End = '" + search + "' or Index_Book like'%" + sech + "%' or Number_Entry like'%" + sech + "%' or Ticket_Number like'" + search + "'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvEntries.DataSource = dtbl;
                    gvEntries.DataBind();
                }
                else
                {
                    dtbl.Rows.Add(dtbl.NewRow());
                    gvEntries.DataSource = dtbl;
                    gvEntries.DataBind();
                    gvEntries.Rows[0].Cells.Clear();
                    gvEntries.Rows[0].Cells.Add(new TableCell());
                    gvEntries.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                    gvEntries.Rows[0].Cells[0].Text = "No Data Found ..!";
                    gvEntries.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch 
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Entries where Index_Book like'%" + sech + "%' or Number_Entry like'%" + sech + "%' or Ticket_Number like'" + search + "'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvEntries.DataSource = dtbl;
                    gvEntries.DataBind();
                }
                else
                {
                    dtbl.Rows.Add(dtbl.NewRow());
                    gvEntries.DataSource = dtbl;
                    gvEntries.DataBind();
                    gvEntries.Rows[0].Cells.Clear();
                    gvEntries.Rows[0].Cells.Add(new TableCell());
                    gvEntries.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                    gvEntries.Rows[0].Cells[0].Text = "No Data Found ..!";
                    gvEntries.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
           
        }

        protected void ButtonCansel_Click(object sender, ImageClickEventArgs e)
        {
            TextBox1.Text = "";
            PopulateGridview();
        }
    }
}