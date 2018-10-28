<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editPageSignup.aspx.cs" Inherits="SignupSys.editPageSignup" Theme="default" ValidateRequest="false" %>

<%@ Register assembly="FreeTextBox" namespace="FreeTextBoxControls" tagprefix="FTB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <div class="sprite_name">
        <span>编辑签到页模板</span>
        </div>
        <div class="top_panel"><p></p>
        <p><asp:Button ID="btnIndex" runat="server" OnClick="btnIndex_Click" Text="返回首页" OnClientClick="this.form.target='';" />&emsp;|&emsp;
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存页面模板" OnClientClick="this.form.target='';" />&emsp;
        <asp:Button ID="btnPreview" runat="server" OnClick="btnPreview_Click" Text="预览页面模板" OnClientClick="this.form.target='_blank';" />&emsp;
        </p>
        </div>
        <div class="container">
        <div style="width: 100%; text-align: left"><p>标题：<asp:TextBox ID="txtTitle" runat="server" Width="400"></asp:TextBox></p></div>
        <div style="width: 100%">
            <FTB:FreeTextBox ID="ftbPage" runat="server" Width="100%" Height="300"
                SupportFolder="aspnet_client/FreeTextBox/" UseToolbarBackGroundImage="True" BackColor="White" Language="zh-cn"
                ToolbarLayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu;Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat;JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule;Cut,Copy,Paste|Undo,Redo"></FTB:FreeTextBox>
        </div></div>
    </div>
    </form>
</body>
</html>
