<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication.Profile" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2>
            <asp:Label ID="lblUserName" runat="server" Text="" /></h2>
        <br />
        <asp:Label ID="UserRoleLabel" runat="server" />&nbsp;&nbsp; <asp:Button ID="ButtonChange" runat="server" Text="Сменить логин/пароль" Height="25px" OnClick="ButtonChange_Click" Style="border-radius: 5px; margin-left: 110px;" BackColor="#CCCC99" BorderColor="Black" CssClass="float-end"/>
        <br />
        <asp:Label ID="Label5" runat="server">Номер вашего читательского билета: </asp:Label><asp:Label ID="Label1" runat="server"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server">Вы оформляли читательский билет до: </asp:Label><asp:Label ID="LabelData" runat="server" Text=""></asp:Label><asp:Button ID="ButtonCon" runat="server" Text="Продлить" Height="25px" OnClick="ButtonCon_Click" Style="border-radius: 5px;" BackColor="#CCCC99" BorderColor="Black"/>
        <br />
        <br />
        <div class="grid-container">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" CssClass="grid-view-table" BackColor="#212529" BorderColor="#212529" BorderWidth="1px" CellPadding="3" CellSpacing="2" BorderStyle="Solid"
                Style="width: 100%; border: 1px solid black;"
                OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Название книги" HeaderText="Название книги" SortExpression="Название книги" />
                    <asp:BoundField DataField="Автор" HeaderText="Автор" SortExpression="Автор" />
                    <asp:BoundField DataField="Дата выдачи книги" HeaderText="Дата выдачи книги" SortExpression="Дата выдачи книги" ReadOnly="True" />
                    <asp:BoundField DataField="Дата окончания срока выдачи книги" HeaderText="Дата окончания срока выдачи книги" SortExpression="Дата окончания срока выдачи книги" ReadOnly="True" />
                    <asp:CheckBoxField DataField="Книга возвращена" HeaderText="Книга возвращена" SortExpression="Книга возвращена" />
                </Columns>
                <FooterStyle BackColor="#FFFFFF" ForeColor="#FFFFFF" />
                <HeaderStyle BackColor="#212529" Font-Bold="True" ForeColor="White" />
                <PagerStyle ForeColor="#212529" HorizontalAlign="Center" />
                <RowStyle BackColor="#FFFFFF" ForeColor="#000000" />
                <SelectedRowStyle BackColor="#FFFFFF" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FFFFFF" />
                <SortedAscendingHeaderStyle BackColor="#FFFFFF" />
                <SortedDescendingCellStyle BackColor="#FFFFFF" />
                <SortedDescendingHeaderStyle BackColor="#FFFFFF" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LibraryUserConnectionString %>" SelectCommand="SELECT Books.Name AS [Название книги], Books.Author AS Автор, FORMAT(Entries.Rent_Begin,'dd.MM.yyyy')  AS [Дата выдачи книги], FORMAT(Entries.Rent_End,'dd.MM.yyyy') AS [Дата окончания срока выдачи книги], Entries.Lease_completed AS [Книга возвращена] FROM Entries INNER JOIN Books ON Entries.Index_Book = Books.Index_Book WHERE Entries.Ticket_Number = @Ticket_Number">
                <SelectParameters>
                    <asp:ControlParameter ControlID="Label1" Name="Ticket_Number" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </main>
</asp:Content>
