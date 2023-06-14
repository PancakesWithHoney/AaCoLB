using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class LoginGhange : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["LibraryUserConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;

            if (user == null || user.FindFirst(ClaimTypes.Role)?.Value != "User")
            {
                // Перенаправление на страницу "Недостаточно прав доступа" или любую другую страницу.
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string readersQuery = "SELECT Login, Password from Authorization_Readers WHERE Index_Reader = " + CookieSec.Decrypt(Request.Cookies["UserIndex"].Value);
                    using (SqlCommand cmd = new SqlCommand(readersQuery, sqlCon))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtLogin.Text = reader["Login"].ToString();
                                HttpCookie pasNumberCookie = new HttpCookie("as");
                                pasNumberCookie.Value = CookieSec.Encrypt(reader["Password"].ToString());
                                pasNumberCookie.Expires = DateTime.Now.AddDays(7); // срок хранения - 7 дней
                                Response.Cookies.Add(pasNumberCookie);
                            }
                        }
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if ((txtLogin.Text != "") && (txtPassword.Text != "") && (TextBox1.Text != "") && (TextBox2.Text != ""))
            {
                if (CookieSec.Decrypt(Request.Cookies["as"].Value.ToString()).Trim() == txtPassword.Text)
                {
                    if (TextBox1.Text == TextBox2.Text)
                    {
                        if (CheckBox1.Checked)
                        {
                            using (SqlConnection sqlCon = new SqlConnection(connectionString))
                            {
                                sqlCon.Open();
                                string readersQuery = "SELECT COUNT(*) from Authorization_Readers WHERE Login = @Login";
                                using (SqlCommand cmd = new SqlCommand(readersQuery, sqlCon))
                                {
                                    cmd.Parameters.AddWithValue("@Login", txtLogin.Text.Trim());
                                    int count = (int)cmd.ExecuteScalar();
                                    if (count == 0)
                                    {
                                        string insert = "UPDATE Authorization_Readers SET Login = @Login, Password = @Password WHERE Index_Reader = @id";
                                        using (SqlCommand cmd1 = new SqlCommand(insert, sqlCon))
                                        {
                                            cmd1.Parameters.AddWithValue("@id", CookieSec.Decrypt(Request.Cookies["UserIndex"].Value));
                                            cmd1.Parameters.AddWithValue("@Login", txtLogin.Text.Trim());
                                            cmd1.Parameters.AddWithValue("@Password", TextBox1.Text.Trim());
                                            cmd1.ExecuteNonQuery();
                                            Response.Redirect("~/Account/Profile");
                                        }
                                    }
                                    else
                                    {
                                        LabelWarning.Text = "Пользователь с таким логином уже существует";
                                    }
                                }

                            }
                        }
                        else
                        {
                            using (SqlConnection sqlCon = new SqlConnection(connectionString))
                            {
                                sqlCon.Open();
                                string readersQuery = "UPDATE Authorization_Readers SET Password = @Password WHERE Index_Reader = @id";
                                using (SqlCommand cmd = new SqlCommand(readersQuery, sqlCon))
                                {
                                    cmd.Parameters.AddWithValue("@id", CookieSec.Decrypt(Request.Cookies["UserIndex"].Value).Trim());
                                    cmd.Parameters.AddWithValue("@Password", TextBox1.Text.Trim());
                                    cmd.ExecuteNonQuery();
                                    Response.Redirect("~/Account/Profile");
                                }
                            }
                        }

                    }
                    else
                        LabelWarning.Text = "Новый пароль неверно повторён";
                }
                else
                {
                    LabelWarning.Text = "Неверный старый пароль";
                }
            }
            else
            {
                LabelWarning.Text = "Поля не должны быть пустыми";
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Profile.aspx");
        }
    }

}
