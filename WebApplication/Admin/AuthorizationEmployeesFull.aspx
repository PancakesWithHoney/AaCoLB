<%@ Page Title="AuthorizationEmployeesFull" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthorizationEmployeesFull.aspx.cs" Inherits="WebApplication.AuthorizationEmployeesFull" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h1>Авторизация сотрудников</h1>
        <div class="centered-content">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="Поиск: "></asp:Label></h3>
            <asp:TextBox ID="TextBox1" runat="server" Height="35px" Width="317px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;<asp:ImageButton ID="ButtonFind" runat="server" Height="35px" Width="35px" ImageUrl="~/Images/search.png" OnClick="ButtonFind_Click" BorderStyle="Solid" BorderWidth="1px" />&nbsp;<asp:ImageButton ID="ButtonCansel" runat="server" Height="35px" Width="35px" ImageUrl="~/Images/cansel.png" OnClick="ButtonCansel_Click" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
        </div>
        <br />
        <br />
        <div class="grid-container">
            <asp:GridView ID="gvAuthorizationEmployees" runat="server" AutoGenerateColumns="False" ShowFooter="True" DataKeyNames="Index_Employee"
                ShowHeaderWhenEmpty="True"
                OnRowCommand="gvAuthorizationEmployees_RowCommand" OnRowEditing="gvAuthorizationEmployees_RowEditing" OnRowCancelingEdit="gvAuthorizationEmployees_RowCancelingEdit"
                OnRowUpdating="gvAuthorizationEmployees_RowUpdating" OnRowDeleting="gvAuthorizationEmployees_RowDeleting"
                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal"
                Style="width: 100%;border: 1px solid black;" CssClass="grid-view-table">
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />

                <Columns>
                    <asp:TemplateField HeaderText="Index_Employee">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Index_Employee") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtIndex_Employee" Text='<%# Eval("Index_Employee") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtIndex_EmployeeFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Login">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Login") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLogin" Text='<%# Eval("Login") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtLoginFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Password">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Password") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPassword" Text='<%# Eval("Password") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtPasswordFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Role">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Role") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRole" Text='<%# Eval("Role") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtRoleFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Images/edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" />
                            <asp:ImageButton ImageUrl="~/Images/delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="20px" Height="20px" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton ImageUrl="~/Images/save.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px" />
                            <asp:ImageButton ImageUrl="~/Images/cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:ImageButton ImageUrl="~/Images/addnew.png" runat="server" CommandName="AddNew" ToolTip="Add New" Width="20px" Height="20px" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="lblErrorMessage" Text="" runat="server" ForeColor="Red" />
        </div>
    </main>
</asp:Content>

