<%@ Page Title="Вход" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class=" centered-content">
        <div id="LoginForm">
            <h1>Вход</h1>
            <br />
            <div>
                <label for="txtLogin">Login:</label>
                <asp:TextBox ID="txtLogin" runat="server" Width="269px"></asp:TextBox>
            </div>
            <br />
            <div>
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="244px"></asp:TextBox>
            </div>
            <asp:Label ID="LabelWarning" runat="server" Text="" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="Войти" OnClick="btnLogin_Click" Style="border-radius: 5px;" BackColor="#CCCC99" BorderColor="Black" Width="86px" />
            &nbsp;&nbsp;
            <asp:Button Style="border-radius: 5px;" ID="ButtonRegistr" runat="server" OnClick="ButtonRegistr_Click" Text="Регистрация" BackColor="#CCCC99" BorderColor="Black" Width="225px" />
            <br />
        </div>
    </div>
</asp:Content>
