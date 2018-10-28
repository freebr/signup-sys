<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editQA.aspx.cs" Inherits="SignupSys.editQA" Theme="default" ValidateRequest="false" %>

<%@ Register assembly="FreeTextBox" namespace="FreeTextBoxControls" tagprefix="FTB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        table.qa td {
            vertical-align: top;
        }
        td.question-list {
            width: 50%;
        }
        td.answer-list {
            width: 50%;
        }
        input[type="text"] {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="sprite_name">
        <span>编辑调查问题和答案</span>
        </div>
        <div class="top_panel"><p></p>
        <p><asp:Button ID="btnIndex" runat="server" OnClick="btnIndex_Click" Text="返回首页" OnClientClick="this.form.target='';" /></p>
        </div>
        <table class="qa" style="width:100%">
            <tr>
                <td class="question-list">
                    <asp:GridView ID="gridQuestions" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSourceQuestion" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnRowCommand="gridQuestions_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Sequence" HeaderText="#" InsertVisible="False" ReadOnly="True" SortExpression="ID" ItemStyle-Width="50px" >
<ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Question" HeaderText="问题文本" SortExpression="Question" />
                            <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" ItemStyle-Width="200px" ShowEditButton="True" ShowInsertButton="True" >
<ItemStyle Width="200px"></ItemStyle>
                            </asp:CommandField>
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <EmptyDataTemplate>
                            <asp:DetailsView ID="dvwNewQuestion" runat="server" AutoGenerateRows="False" AutoGenerateInsertButton="True" DataKeyNames="ID" DataSourceID="SqlDataSourceQuestion" DefaultMode="Insert" CellPadding="4" GridLines="None" Width="100%" ForeColor="#333333" OnItemCommand="dvwNewQuestion_ItemCommand">
                                <AlternatingRowStyle BackColor="White" />
                                <CommandRowStyle BackColor="#C5BBAF" Font-Bold="True" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <FieldHeaderStyle BackColor="#D0D0D0" Font-Bold="True" />
                                <Fields>
                                    <asp:BoundField DataField="Question" HeaderText="问题文本" SortExpression="Question" >
                                    <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                </Fields>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                            </asp:DetailsView>
                            <asp:Literal runat="server" Visible="<%# questionValid==false %>">
                                <p class="validation">请输入问题文本！</p>
                            </asp:Literal>
                        </EmptyDataTemplate>
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                </td>
                <td class="answer-list">
                    <asp:GridView ID="gridAnswers" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSourceAnswer" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnRowCommand="gridAnswers_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Sequence" HeaderText="#" InsertVisible="False" ReadOnly="True" SortExpression="Sequence" ItemStyle-Width="20px" >
<ItemStyle Width="30px" HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Answer" HeaderText="答案文本" SortExpression="Answer" />
                            <asp:CommandField ShowDeleteButton="True" ItemStyle-Width="200px" ShowEditButton="True" ShowInsertButton="True">
                                <ItemStyle Width="150px"></ItemStyle>
                            </asp:CommandField>
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <EmptyDataTemplate>
                            <asp:Literal ID="ltrNoQuestion" runat="server" Visible="<%#!questionSelected%>">请在左边选择一个问题项。</asp:Literal>
                            <asp:DetailsView ID="dvwNewAnswer" runat="server" Visible="<%# questionSelected %>" AutoGenerateRows="False" AutoGenerateInsertButton="True" DataKeyNames="ID" DataSourceID="SqlDataSourceAnswer" DefaultMode="Insert" CellPadding="4" GridLines="None" Width="100%" ForeColor="#333333" OnItemCommand="dvwNewAnswer_ItemCommand">
                                <AlternatingRowStyle BackColor="White" />
                                <CommandRowStyle BackColor="#C5BBAF" Font-Bold="True" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <FieldHeaderStyle BackColor="#D0D0D0" Font-Bold="True" />
                                <Fields>
                                    <asp:BoundField DataField="Answer" HeaderText="答案文本" SortExpression="Answer">
                                    <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                </Fields>
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                            </asp:DetailsView>
                            <asp:Literal runat="server" Visible="<%# answerValid==false %>">
                                <p class="validation">请输入答案文本！</p>
                            </asp:Literal>
                        </EmptyDataTemplate>
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
            
        <asp:SqlDataSource ID="SqlDataSourceQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:SignupSysConnectionString %>" SelectCommand="SELECT * FROM [ViewQuestions]">
        </asp:SqlDataSource>
            
        <asp:SqlDataSource ID="SqlDataSourceAnswer" runat="server" ConnectionString="<%$ ConnectionStrings:SignupSysConnectionString %>" SelectCommand="SELECT * FROM [ViewAnswers] WHERE ([QuestionID] = @QuestionID)">
            <SelectParameters>
                <asp:ControlParameter ControlID="gridQuestions" Name="QuestionID" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
