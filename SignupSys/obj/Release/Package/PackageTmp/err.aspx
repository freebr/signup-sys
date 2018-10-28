<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="err.aspx.cs" Inherits="SignupSys.err" Theme="default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="container">
        <div class="errTitle"><p>错误提示</p></div>
        <div class="errContent">
            <p><asp:Literal ID="ltrSignup" runat="server"></asp:Literal></p>
        </div>
        <div class="errOperation">
            <asp:Button ID="btnIndex" runat="server" OnClick="btnIndex_Click" Text="返回首页" />
        </div>
        </div>
    </div>
    </form>
</body>
</html>
