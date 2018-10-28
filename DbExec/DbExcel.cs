using Entity;
using Microsoft.Office.Interop.Owc11;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace DbExec
{
    public static class DbExcel
    {
        private const String sql_ImportInterviewIDList_QueryAll = "SELECT * FROM [{0}A:D]";
        public const int OK = 0;
        public const int FAILED = 1;

        private static OleDbConnection ConnectDb(String excelFile)
        {
            String connstring = String.Format(ConfigurationManager.ConnectionStrings["ExcelFileConnectionString"].ConnectionString, excelFile);
            try
            {
                OleDbConnection conn = new OleDbConnection(connstring);
                conn.Open();
                return conn;
            }
            catch (OleDbException ex)
            {
                throw new Exception(String.Format("数据库连接出错：{0}", ex.Message));
            }
        }
        // 导入待签到用户信息到数据库
        public static int importSignupUserList(String excelFile, out int importedCount) {
            List<SignupUserItem> list = new List<SignupUserItem>();
            importedCount = 0;
            try
            {
                using (OleDbConnection conn = ConnectDb(excelFile))
                {
                    // 获取可访问的表名
                    DataTableReader dtr = new DataTableReader(conn.GetSchema("Tables", new string[] { null, null, null, "TABLE" }));
                    String table_used;
                    if (dtr.Read())
                        table_used = dtr["TABLE_NAME"].ToString();
                    else
                        throw new Exception("该Excel文件不包含可用的表格。");
                    // 查询分组编号记录
                    OleDbCommand cmd = new OleDbCommand(String.Format(sql_ImportInterviewIDList_QueryAll, table_used), conn);
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) {
                        SignupUserItem item = new SignupUserItem();
                        item.Name = dr[0].ToString();
                        item.CardID = dr[1].ToString();
                        item.Mobile = dr[2].ToString();
                        item.AssignID = dr[3].ToString();
                        list.Add(item);
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("读取Excel文件时出错：{0}", ex.Message));
            }
            importedCount = list.Count;
            return DbEdit.batchAddSignupUserItems(list);
        }
        // 导出签到名单到Excel文件
        public static int exportSignupList(String excelFile, Dictionary<String, String[]> conditions)
        {
            try
            {
                List<QuestionItem> listQuestions;
                List<SignupUserItem> list;
                DbSelect.getQuestionList(out listQuestions, null);
                // 读取签到名单
                DbSelect.getSignupList(out list, conditions);
                // 生成Excel文件
                SpreadsheetClass factory = new SpreadsheetClass();
                Worksheet sheet = factory.ActiveSheet;
                List<string> columnHeaderNames = new List<string> { "签到编号", "姓名", "身份证号码", "手机号码", "签到时间" };
                List<int> columnHeaderWidth = new List<int> { 10, 15, 12, 15 };
                int columnHeaderQuestionWidth = 42;
                listQuestions.ForEach((QuestionItem q) =>
                {
                    columnHeaderNames.Add(q.Question);
                    columnHeaderWidth.Add(columnHeaderQuestionWidth);
                });
                sheet.Name = String.Format("签到名单({0}.{1})", DateTime.Today.Year, DateTime.Today.Month.ToString("00"));
                // 字段名
                for (int i = 0; i < columnHeaderNames.Count; i++)
                {
                    Range col = sheet.get_Range((char)('A' + i) + "1");
                    sheet.Cells[1, i + 1] = columnHeaderNames[i];
                    col.set_ColumnWidth(columnHeaderWidth[i]);
                    col.set_HorizontalAlignment(XlHAlign.xlHAlignCenter);
                    col.Font.set_Bold(true);
                }
                // 记录
                for (int i = 0, ROW_OFFSET = 2, COL_OFFSET = 1; i < list.Count; i++)
                {
                    sheet.Cells[ROW_OFFSET + i, COL_OFFSET] = list[i].AssignID;
                    sheet.Cells[ROW_OFFSET + i, COL_OFFSET + 1] = list[i].Name;
                    sheet.Cells[ROW_OFFSET + i, COL_OFFSET + 2] = '\'' + list[i].CardID;
                    sheet.Cells[ROW_OFFSET + i, COL_OFFSET + 3] = '\'' + list[i].Mobile;
                    sheet.Cells[ROW_OFFSET + i, COL_OFFSET + 4] = list[i].SignupTime.ToString();
                    if (list[i].Answers.Count == listQuestions.Count)
                    {
                        for (int j = 0; j < listQuestions.Count; j++)
                        {
                            sheet.Cells[ROW_OFFSET + i, COL_OFFSET + j + 4] = list[i].Answers[j].Answer;
                        }
                    }
                }
                factory.Export(excelFile, SheetExportActionEnum.ssExportActionNone, SheetExportFormat.ssExportXMLSpreadsheet);
                return OK;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("导出签到名单时出错：{0}", ex.Message));
            }
        }
    }
}
