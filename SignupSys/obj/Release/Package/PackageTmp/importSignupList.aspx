<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="importSignupList.aspx.cs" Inherits="SignupSys.ImportSignupList" Theme="default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="sprite_name">
        <span>导入待签到用户名单</span>
        </div>
        <div class="top_panel">
        <p><asp:Button ID="btnIndex" runat="server" OnClick="btnIndex_Click" Text="返回首页" />&emsp;
        <asp:FileUpload ID="fileImport" runat="server" Width="318px" />&nbsp;<asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" Text="从 Excel 文件导入" />
        </p>
        <p>上传文件格式：*.xls 或 *.xlsx，<a href="template/signuplist.xlsx" target="_blank">点击下载导入模板</a>。</p>
        </div>
    </div>
    </form>
</body>
</html>
