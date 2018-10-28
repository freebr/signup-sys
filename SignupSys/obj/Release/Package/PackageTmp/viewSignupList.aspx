<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewSignupList.aspx.cs" Inherits="SignupSys.ViewSignupList" Theme="default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="sprite_name">
        <span>查看签到名单</span>
        </div>
        <div class="top_panel">
        <p><asp:Button ID="btnIndex" runat="server" OnClick="btnIndex_Click" Text="返回首页" />&emsp;|&emsp;
        <asp:Label ID="lblRecordInfo" runat="server"></asp:Label>&emsp;
        <asp:Button ID="btnRefresh" runat="server" Text="刷新" />&nbsp;
        <asp:Button ID="btnRecoverAll" runat="server" OnClick="btnRecoverAll_Click" Text="清空全部签到信息" OnClientClick="if(!confirmRecoverAll())return false;" />&nbsp;
        <asp:Button ID="btnDeleteAll" runat="server" OnClick="btnDeleteAll_Click" Text="删除全部用户记录" OnClientClick="if(!confirmDeleteAll())return false;" />&nbsp;
        <asp:Button ID="btnImport" runat="server" Text="从 Excel 文件导入" PostBackUrl="~/importSignupList.aspx" />
        <asp:Button ID="btnExport" runat="server" Text="导出到 Excel 文件" PostBackUrl="~/exportSignupList.aspx" />
        </p>
        </div>
        <div class="container">
            <asp:GridView ID="gridSignupList" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True">
                <Columns>
                    <asp:BoundField HeaderText="#" DataField="ID" >
                    <HeaderStyle CssClass="column-id" />
                    <ItemStyle CssClass="column-id" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="签到编号" DataField="AssignID" />
                    <asp:BoundField HeaderText="姓名" DataField="Name" />
                    <asp:BoundField HeaderText="身份证号码" DataField="CardID" />
                    <asp:BoundField HeaderText="手机号码" DataField="Mobile" />
                    <asp:BoundField HeaderText="导入时间" DataField="ImportTime" />
                    <asp:BoundField HeaderText="签到时间" DataField="SignupTime" />
                    <asp:BoundField HeaderText="调查问题回答" DataField="TextAnswers" >
                    <ItemStyle CssClass="text-answers" />
                    </asp:BoundField>
                    <asp:CommandField ButtonType="Image" HeaderText="操作" EditImageUrl="~/images/recover.png" DeleteImageUrl="~/images/delete.png" ShowEditButton="True" ShowDeleteButton="True">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:CommandField>
                </Columns>
                <EmptyDataTemplate>
                    没有记录！
                </EmptyDataTemplate>
                <HeaderStyle BackColor="#99CCFF" />
                <AlternatingRowStyle BackColor="#FFFFCC" />
                <SelectedRowStyle BackColor="Lime" />
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
