<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exportSignupList.aspx.cs" Inherits="SignupSys.exportSignupList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="ExcelFile" runat="server" />
        <asp:HiddenField ID="LastPage" runat="server" />
        <asp:Panel ID="script" runat="server">
            <script type="text/javascript">
                if (confirm("导出成功，请点击\"确定\"按钮下载。")) {
                    location.href = document.all.ExcelFile.value;
                } else {
                    location.href = document.all.LastPage.value;
                }
            </script>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
