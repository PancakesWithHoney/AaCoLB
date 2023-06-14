<%@ Page Title="Readers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Readers.aspx.cs" Inherits="WebApplication.Readers" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
            <h1>Читатели/Билеты</h1>
        <div class="centered-content">
            <h3><asp:Label ID="Label1" runat="server" Text="Поиск: "></asp:Label></h3><asp:TextBox ID="TextBox1" runat="server" Height="35px" Width="317px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;<asp:ImageButton ID="ButtonFind" runat="server" Height="35px" Width="35px" ImageUrl="~/Images/search.png" OnClick="ButtonFind_Click" BorderStyle="Solid" BorderWidth="1px" />&nbsp;<asp:ImageButton ID="ButtonCansel" runat="server" Height="35px" Width="35px" ImageUrl="~/Images/cansel.png" OnClick="ButtonCansel_Click" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
        </div>
        <br />
        <br />
        <div class="grid-container">
            <asp:GridView ID="gvReaders" runat="server" AutoGenerateColumns="False" ShowFooter="True" DataKeyNames="Index_Reader"
                ShowHeaderWhenEmpty="True"
                OnRowCommand="gvReaders_RowCommand" OnRowEditing="gvReaders_RowEditing" OnRowCancelingEdit="gvReaders_RowCancelingEdit"
                OnRowUpdating="gvReaders_RowUpdating"
                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal"
                Style="width: 100%;border: 1px solid black;" CssClass="grid-view-table">
                <%-- Theme Properties --%>
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />

                <Columns>
                    <asp:TemplateField HeaderText="Ticket_Number">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Ticket_Number") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTicket_Number" Text='<%# Eval("Ticket_Number") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Surname">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Surname") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSurname" Text='<%# Eval("Surname") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtSurnameFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Name") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" Text='<%# Eval("Name") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNameFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Patronymic">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Patronymic") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPatronymic" Text='<%# Eval("Patronymic") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtPatronymicFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Birthdate">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Birthdate") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBirthdate" Text='<%# Eval("Birthdate") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtBirthdateFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Passport">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Passport") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPassport" Text='<%# Eval("Passport") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtPassportFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Registration">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Registration") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRegistration" Text='<%# Eval("Registration") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtRegistrationFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Images/edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" />
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
