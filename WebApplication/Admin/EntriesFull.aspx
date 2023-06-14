<%@ Page Title="EntriesFull" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntriesFull.aspx.cs" Inherits="WebApplication.EntriesFull" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h1>Записи</h1>
        <div class="centered-content">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="Поиск: "></asp:Label></h3>
            <asp:TextBox ID="TextBox1" runat="server" Height="35px" Width="317px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>&nbsp;<asp:ImageButton ID="ButtonFind" runat="server" Height="35px" Width="35px" ImageUrl="~/Images/search.png" OnClick="ButtonFind_Click" BorderStyle="Solid" BorderWidth="1px" />&nbsp;<asp:ImageButton ID="ButtonCansel" runat="server" Height="35px" Width="35px" ImageUrl="~/Images/cansel.png" OnClick="ButtonCansel_Click" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
        </div>
        <br />
        <br />
        <div class="centered-content">
            <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="False" ShowFooter="True" DataKeyNames="Number_Entry"
                ShowHeaderWhenEmpty="True"
                OnRowCommand="gvEntries_RowCommand" OnRowEditing="gvEntries_RowEditing" OnRowCancelingEdit="gvEntries_RowCancelingEdit"
                OnRowUpdating="gvEntries_RowUpdating" OnRowDeleting="gvEntries_RowDeleting"
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
                    <asp:TemplateField HeaderText="Индекс записи">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Number_Entry") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Номер читательского билета">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Ticket_Number") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTicket_Number" Text='<%# Eval("Ticket_Number") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtTicket_NumberFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Индекс книги">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Index_Book") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtIndex_Book" Text='<%# Eval("Index_Book") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtIndex_BookFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Дата выдачи">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Rent_Begin") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRent_Begin" Text='<%# Eval("Rent_Begin") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtRent_BeginFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Крайний срок возврата">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Rent_End") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRent_End" Text='<%# Eval("Rent_End") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtRent_EndFooter" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Индекс сотрудника">
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
                    <asp:TemplateField HeaderText="Книга возвращена">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Lease_completed") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLease_completed" Text='<%# Eval("Lease_completed") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtLease_completedFooter" runat="server" />
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
        </div>
        <br />
        <div class="centered-content">
            <asp:Label ID="lblErrorMessage" Text="" runat="server" ForeColor="Red" />
        </div>
    </main>
</asp:Content>

