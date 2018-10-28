using Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DbExec
{
    public static class DbEdit
    {
        private const String sql_BatchAddSignupUserItem_Exec = @"EXEC spAddSignupInfo @Name, @CardID, @Mobile, @AssignID";
        private const String sql_AddSignupUserItem_Select = @"SELECT ID,Name,Mobile,ImportTime,SignupTime,SignupPage,Valid FROM SignupUsers";
        private const String sql_UpdateSignupUserItem_Exec = @"EXEC spUpdateSignupInfo @Name, @Mobile";
        private const String sql_UpdateSignupUserItem_Exec2 = @"EXEC spAddUserAnswer @UserID, @QuestionID, @AnswerID";
        private const String sql_RecoverSignupUserItem_Update = @"UPDATE SignupUsers SET SignupSequence=NULL,SignupTime=NULL,SignupPage=NULL WHERE ID={0}";
        private const String sql_RecoverSignupUserItem_Delete = @"DELETE FROM UserAnswers WHERE UserID={0}";
        private const String sql_RecoverAllSignupUserItems_Update = @"UPDATE SignupUsers SET SignupSequence=NULL,SignupTime=NULL,SignupPage=NULL";
        private const String sql_RecoverAllSignupUserItems_Delete = @"DELETE FROM UserAnswers";
        private const String sql_DeleteSignupUserItem_Delete = @"DELETE FROM SignupUsers WHERE ID={0}";
        private const String sql_DeleteAllSignupUserItems_Delete = @"DELETE FROM SignupUsers";
        public const int OK = 0;
        public const int FAILED = 1;

        private static SqlConnection ConnectDb()
        {
            String connstring = ConfigurationManager.ConnectionStrings["SignupSysConnectionString"].ConnectionString;
            try
            {
                SqlConnection conn = new SqlConnection(connstring);
                conn.Open();
                return conn;
            }
            catch (SqlException ex)
            {
                throw new Exception(String.Format("数据库连接出错：{0}", ex.Message));
            }
        }
        // 添加待签到用户信息
        public static int batchAddSignupUserItems(List<SignupUserItem> list)
        {
            if (list.Count == 0) return FAILED;
            try
            {
                using (SqlConnection conn = ConnectDb())
                {
                    foreach (SignupUserItem item in list)
                    {
                        // 新增签到记录
                        SqlParameter[] pms = new SqlParameter[]{
                                     new SqlParameter("@Name", item.Name),
                                     new SqlParameter("@CardID", item.CardID),
                                     new SqlParameter("@Mobile", item.Mobile),
                                     new SqlParameter("@AssignID", item.AssignID)};
                        SqlCommand cmd = new SqlCommand(sql_BatchAddSignupUserItem_Exec, conn);
                        cmd.Parameters.AddRange(pms);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("批量添加待签到用户信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 写入用户签到信息并返回签到信息
        public static int updateSignupUserItem(ref SignupUserItem item)
        {
            SqlConnection conn;
            try
            {
                conn = ConnectDb();
                // 更新签到记录
                SqlParameter[] pms = new SqlParameter[]{
                                     new SqlParameter("@Name", item.Name),
                                     new SqlParameter("@Mobile", item.Mobile)};
                SqlCommand cmd = new SqlCommand(sql_UpdateSignupUserItem_Exec, conn);
                cmd.Parameters.AddRange(pms);
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                sdr.Read();
                item.SignupSequence = sdr.GetInt32(0).ToString();
                item.SignupPage = sdr.GetString(1);
                sdr.Close();

                int user_id = item.ID;
                item.Answers.ForEach((AnswerItem ans) =>
                {
                    pms = new SqlParameter[]{
                                     new SqlParameter("@UserID", user_id),
                                     new SqlParameter("@QuestionID", ans.Question.ID),
                                     new SqlParameter("@AnswerID", ans.ID)};
                    cmd = new SqlCommand(sql_UpdateSignupUserItem_Exec2, conn);
                    cmd.Parameters.AddRange(pms);
                    cmd.ExecuteNonQuery();
                });
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("写入签到信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 按签到序号清空指定用户的签到信息
        public static int recoverSignupUserItem(int ID)
        {
            try
            {
                List<SignupUserItem> list;
                Dictionary<String, String[]> conditions = new Dictionary<String, String[]>();
                conditions.Add("ID", new String[] { new Operator(OperatorTypeEnum.Equal), ID.ToString() });
                int ret = DbSelect.getSignupList(out list, conditions);
                if (ret != OK)
                {
                    throw new Exception(String.Format("不存在序号为\"{0}\"的记录。", ID));
                }
                using (SqlConnection conn = ConnectDb())
                {
                    SqlCommand cmd = new SqlCommand(String.Format(sql_RecoverSignupUserItem_Update, ID), conn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(String.Format(sql_RecoverSignupUserItem_Delete, ID), conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("清空指定用户的签到信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 清空全部用户的签到信息
        public static int recoverAllSignupUserItems()
        {
            try
            {
                using (SqlConnection conn = ConnectDb())
                {
                    SqlCommand cmd = new SqlCommand(sql_RecoverAllSignupUserItems_Update, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(sql_RecoverAllSignupUserItems_Delete, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("清空全部用户的签到信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 按签到序号删除指定用户信息
        public static int deleteSignupUserItem(int ID)
        {
            try
            {
                List<SignupUserItem> list;
                Dictionary<String, String[]> conditions = new Dictionary<String, String[]>();
                conditions.Add("ID", new String[] { new Operator(OperatorTypeEnum.Equal), ID.ToString() });
                int ret = DbSelect.getSignupList(out list, conditions);
                if (ret != OK) {
                    throw new Exception(String.Format("不存在序号为\"{0}\"的记录。", ID));
                }
                using (SqlConnection conn = ConnectDb())
                {
                    SqlCommand cmd = new SqlCommand(String.Format(sql_DeleteSignupUserItem_Delete, ID), conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("删除单个用户信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 删除全部用户信息
        public static int deleteAllSignupUserItems()
        {
            try
            {
                using (SqlConnection conn = ConnectDb())
                {
                    SqlCommand cmd = new SqlCommand(sql_DeleteAllSignupUserItems_Delete, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("删除全部用户信息时出错：{0}", ex.Message));
            }
            return OK;
        }
    }
}