using System;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebApplication
{
    public partial class SiteMaster : MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAuthenticationStatus();
        }
        private void CheckAuthenticationStatus()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                LoginLink.Visible = false;
                ProfileLink.Visible = true;
                LogoutLink.Visible = true;
                var user = HttpContext.Current.User as ClaimsPrincipal;
                if (user == null || user.FindFirst(ClaimTypes.Role)?.Value != "Employee")
                {
                    DropDownList1.Visible = false;
                }
                if (user == null || user.FindFirst(ClaimTypes.Role)?.Value != "Admin")
                {
                    DropDownList2.Visible = false;
                }
            }
            else
            {
                DropDownList2.Visible = false;
                DropDownList1.Visible = false;
                LoginLink.Visible = true;
                ProfileLink.Visible = false;
                LogoutLink.Visible = false;
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (DropDownList1.SelectedIndex)
            {
                case 1:
                    {
                        Response.Redirect("~/Employee/Readers.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 2:
                    {
                        Response.Redirect("~/Employee/Books.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 3:
                    {
                        Response.Redirect("~/Employee/Entries.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
            }
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (DropDownList2.SelectedIndex)
            {
                case 1:
                    {
                        Response.Redirect("~/Admin/AuthorizationEmployeesFull.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 2:
                    {
                        Response.Redirect("~/Admin/AuthorizationReadersFull.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 3:
                    {
                        Response.Redirect("~/Admin/BooksFull.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 4:
                    {
                        Response.Redirect("~/Admin/EmployeesFull.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 5:
                    {
                        Response.Redirect("~/Admin/EntriesFull.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 6:
                    {
                        Response.Redirect("~/Admin/ReadersFull.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
                case 7:
                    {
                        Response.Redirect("~/Admin/TicketsFull.aspx");
                        DropDownList1.SelectedIndex = 0;
                        break;
                    }
            }
        }
    }
}