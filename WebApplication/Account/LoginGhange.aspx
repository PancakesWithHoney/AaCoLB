<%@ Page Title="Смена логина/пароля" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoginGhange.aspx.cs" Inherits="WebApplication.LoginGhange" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        < <div class=" centered-content">
        <div id="LoginForm">
            <h1>Смена логина/пароля</h1>
            <br />
            <div>
                <label for="txtLogin">Введите логин:</label>
                <asp:TextBox ID="txtLogin" runat="server" Width="269px"></asp:TextBox>&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="CheckBox1" text="<-- поменять логин" runat="server" />
            </div>
            <br />
            <div>
                <label for="txtPassword">Введите старый пароль:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="244px"></asp:TextBox>
            </div>
            <br />
            <div>
                <label for="txtPassword">Введите новый пароль:</label>
                <asp:TextBox ID="TextBox1" runat="server" TextMode="Password" Width="244px"></asp:TextBox>
            </div>
            <br />
            <div>
                <label for="txtPassword">Повторите новый пароль:</label>
                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" Width="244px"></asp:TextBox>
            </div>
            <asp:Label ID="LabelWarning" runat="server" Text="" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Сменить" Style="border-radius: 5px;" BackColor="#CCCC99" BorderColor="Black" Width="240px" OnClick="Button1_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" style="border-radius: 5px;" Text="Отмена" BackColor="#CCCC99" BorderColor="Black" Width="86px" OnClick="Button2_Click" />
            <br />
        </div>
            <asp:HiddenField ID="HiddenOldPassword" runat="server" />
    </div>
    </main>
</asp:Content>
