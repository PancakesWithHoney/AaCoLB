<%@ Page Title="Регистрация" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrationFinale.aspx.cs" Inherits="WebApplication.RegistrationFinale" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class=" centered-content">
            <div id="RegistrationForm">
                <h1>Регистрация</h1>
                <br />
                <div>
                    <asp:Label ID="Label5" runat="server" Text="Придумайте логин:"></asp:Label>
                    <asp:TextBox ID="LoginTextBox" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Label ID="Label6" runat="server" Text="Придумайте пароль:"></asp:Label>
                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Label ID="Label7" runat="server" Text="Повторите пароль:"></asp:Label>
                    <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
                    <br />
                    <asp:Label ID="LabelWarning" runat="server" Text="" ForeColor =" red"></asp:Label>
                    <br />
                    <asp:Button ID="Button2" runat="server" Text="Зарегистрироваться" OnClick="Button2_Click" Style="border-radius: 5px;" BackColor="#CCCC99" BorderColor="Black" Width="978px" />
                </div>
            </div>
        </div>
    </main>
</asp:Content>
