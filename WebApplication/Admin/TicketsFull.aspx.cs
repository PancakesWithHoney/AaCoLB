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
    public partial class TicketsFull : System.Web.UI.Page
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT Ticket_Number, Index_Reader, FORMAT(Begin_Date, 'dd.MM.yyyy') AS Begin_Date, FORMAT(End_Date, 'dd.MM.yyyy') AS End_Date , Index_Emloyee FROM Tickets", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvTickets.DataSource = dtbl;
                gvTickets.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvTickets.DataSource = dtbl;
                gvTickets.DataBind();
                gvTickets.Rows[0].Cells.Clear();
                gvTickets.Rows[0].Cells.Add(new TableCell());
                gvTickets.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvTickets.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvTickets.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        protected void gvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("AddNew"))
                {
                    if (((gvTickets.FooterRow.FindControl("txtTicket_NumberFooter") as TextBox).Text != "") && ((gvTickets.FooterRow.FindControl("txtIndex_ReaderFooter") as TextBox).Text != "") && ((gvTickets.FooterRow.FindControl("txtBegin_DateFooter") as TextBox).Text != "") && ((gvTickets.FooterRow.FindControl("End_Date") as TextBox).Text != "") && ((gvTickets.FooterRow.FindControl("Index_Emloyee") as TextBox).Text != ""))
                    {
                        using (SqlConnection sqlCon = new SqlConnection(connectionString))
                        {
                            sqlCon.Open();
                            string query = "INSERT INTO Tickets (Ticket_Number, Index_Reader, Begin_Date, End_Date, Index_Emloyee) VALUES (@Ticket_Number, @Index_Reader, @Begin_Date, @End_Date, @Index_Emloyee)";
                            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                            sqlCmd.Parameters.AddWithValue("@Ticket_Number", (gvTickets.FooterRow.FindControl("txtTicket_NumberFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Index_Reader", (gvTickets.FooterRow.FindControl("txtIndex_ReaderFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Begin_Date", (gvTickets.FooterRow.FindControl("txtBegin_DateFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@End_Date", (gvTickets.FooterRow.FindControl("txtEnd_DateFooter") as TextBox).Text.Trim());
                            sqlCmd.Parameters.AddWithValue("@Index_Emloyee", (gvTickets.FooterRow.FindControl("txtIndex_EmloyeeFooter") as TextBox).Text.Trim());
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
        protected void gvTickets_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (TextBox1.Text != "")
            {
                gvTickets.EditIndex = e.NewEditIndex;
                string search = TextBox1.Text;
                int sech; Int32.TryParse(search, out sech);
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Tickets WHERE Index_Reader = '" + sech + "' or Surname like '%" + search + "%' or Name like '%" + search + "%' or Passport like '%" + search + "%'", sqlCon);
                    sqlDa.Fill(dtbl);
                }
                if (dtbl.Rows.Count > 0)
                {
                    gvTickets.DataSource = dtbl;
                    gvTickets.DataBind();
                }
            }
            else
            {
                gvTickets.EditIndex = e.NewEditIndex;
                PopulateGridview();
            }
        }
        protected void gvTickets_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTickets.EditIndex = -1;
            PopulateGridview();
        }
        protected void gvTickets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE Tickets SET Ticket_Number = @Ticket_Number, Index_Reader = @Index_Reader, Begin_Date = @Begin_Date, End_Date = @End_Date, Index_Emloyee = @Index_Emloyee WHERE Ticket_Number = @id";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvTickets.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.Parameters.AddWithValue("@Ticket_Number", (gvTickets.Rows[e.RowIndex].FindControl("txtTicket_Number") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Index_Reader", (gvTickets.Rows[e.RowIndex].FindControl("txtIndex_Reader") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Begin_Date", (gvTickets.Rows[e.RowIndex].FindControl("txtBegin_Date") as TextBox).Text);
                    sqlCmd.Parameters.AddWithValue("@End_Date", (gvTickets.Rows[e.RowIndex].FindControl("txtEnd_Date") as TextBox).Text);
                    sqlCmd.Parameters.AddWithValue("@Index_Emloyee", (gvTickets.Rows[e.RowIndex].FindControl("txtIndex_Emloyee") as TextBox).Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    gvTickets.EditIndex = -1;
                    PopulateGridview();
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }
        protected void gvTickets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Tickets WHERE Ticket_Number = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvTickets.DataKeys[e.RowIndex].Value.ToString()));
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from Tickets WHERE Ticket_Number = '" + sech + "' or  Index_Reader like '%" + search + "%' or Index_Emloyee like '%" + search + "%'", sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvTickets.DataSource = dtbl;
                gvTickets.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvTickets.DataSource = dtbl;
                gvTickets.DataBind();
                gvTickets.Rows[0].Cells.Clear();
                gvTickets.Rows[0].Cells.Add(new TableCell());
                gvTickets.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvTickets.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvTickets.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void ButtonCansel_Click(object sender, ImageClickEventArgs e)
        {
            TextBox1.Text = "";
            PopulateGridview();
        }
    }
}