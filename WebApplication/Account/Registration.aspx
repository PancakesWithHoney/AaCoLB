<%@ Page Title="Регистрация" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="WebApplication.Registration" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class=" centered-content">
            <div id="RegistrationForm">
                <h1>Регистрация</h1>
                <div>
                    <asp:Label ID="Label1" runat="server" Text="Номер читательского билета:"></asp:Label>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </div>
                <br />
                <div>
                    <asp:Label ID="Label2" runat="server" Text="Фамилия:"></asp:Label>
                    <asp:TextBox ID="TextBox2" runat="server" Width="351px"></asp:TextBox>
                </div>
                <br />
                <div>
                    <asp:Label ID="Label3" runat="server" Text="Имя:"></asp:Label>
                    <asp:TextBox ID="TextBox3" runat="server" Width="437px"></asp:TextBox>
                </div>
                <br />
                <div>
                    <asp:Label ID="Label4" runat="server" Text="Дата рождения:"></asp:Label>
                    <asp:TextBox ID="TextBox4" runat="server" TextMode="Date" ToolTip="ГГГГ-ММ-ДД" Width="129px"></asp:TextBox>
                </div>
                <asp:Label ID="LabelWarning" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
                <br />
                <asp:Button ID="Button1" runat="server" Text="Проверить данные" OnClick="Button1_Click" Style="border-radius: 5px;" BackColor="#CCCC99" BorderColor="Black" Width="978px" />
            </div>
    </main>
</div>
</asp:Content>
