<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewQRCode.aspx.cs" Inherits="SignupSys.viewQRCode" Theme="default" %>

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
        <span>查看签到二维码</span>
        </div>
        <div class="top_panel">
        <p><asp:Button ID="btnIndex" runat="server" OnClick="btnIndex_Click" Text="返回首页" />
        </p>
        </div>
        <div class="container">
        <div>
            <p align="center"><asp:Literal ID="ltrSignupUrl" runat="server"></asp:Literal></p>
            <p align="center"><asp:Image ID="imgQRCode" runat="server" /></p>
        </div></div>
    </div>
    </form>
</body>
</html>
