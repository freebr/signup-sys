using Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DbExec
{
    public static class DbSelect
    {
        private const String sql_CheckUserPrivilege =
              @"SELECT * FROM ViewTeacherInfo WHERE TEACHERID=@teacherId AND dbo.hasPrivilege(WRITEPRIVILEGETAGSTRING,'AS2')=1";
        private const String sql_GetSignupList =
              @"SELECT ID,Name,CardID,Mobile,AssignID,SignupSequence,ImportTime,SignupTime,SignupPage FROM SignupUsers {0} ORDER BY SignupSequence DESC";
        private const String sql_GetUserAnswerList =
              @"SELECT UserID,QuestionID,AnswerID FROM UserAnswers {0} ORDER BY QuestionID";
        private const String sql_GetQuestionList =
              @"SELECT ID,Sequence,Question,Valid FROM ViewQuestions {0} ORDER BY Sequence";
        private const String sql_GetAnswerList =
              @"SELECT ID,Sequence,QuestionID,Answer,Valid FROM ViewAnswers {0} ORDER BY Sequence";
        public const int OK = 0;
        public const int FAILED = 1;
        private static List<QuestionItem> listQuestions;

        private static SqlConnection ConnectDb()
        {
            String connstring = ConfigurationManager.ConnectionStrings["SignupSysConnectionString"].ConnectionString;
            try
            {
                SqlConnection conn = new SqlConnection(connstring);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("数据库连接出错：{0}", ex.Message));
            }
        }
        // 查询具有指定序号和权限的用户，用于系统登录
        public static int checkUserPrivilege(String uid)
        {
            bool bValid = false;
            try
            {
                using (SqlConnection conn = ConnectDb())
                {
                    SqlCommand cmd = new SqlCommand(sql_CheckUserPrivilege, conn);
                    cmd.Parameters.AddWithValue("@teacherId", uid);
                    SqlDataReader dr = cmd.ExecuteReader();
                    bValid = dr.HasRows;
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("验证用户信息时出错：{0}", ex.Message));
            }
            if (bValid)
                return OK;
            else
                return FAILED;
        }
        // 查询签到用户信息
        public static int getSignupList(out List<SignupUserItem> list, Dictionary<String, String[]> conditions)
        {
            list = null;
            try
            {
                using (SqlConnection conn = ConnectDb())
                {
                    SqlParameter[] pms;
                    String sql = String.Format(sql_GetSignupList, getSignupUserItemFilter(conditions, out pms));
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (pms != null) cmd.Parameters.AddRange(pms);
                    SqlDataReader dr = cmd.ExecuteReader();
                    list = new List<SignupUserItem>();
                    while (dr.Read())
                    {
                        SignupUserItem item = new SignupUserItem();
                        item.ID = (int)dr["ID"];
                        item.SignupSequence = dr["SignupSequence"].ToString();
                        item.AssignID = dr["AssignID"].ToString();
                        item.Name = dr["Name"].ToString();
                        item.CardID = dr["CardID"].ToString();
                        item.Mobile = dr["Mobile"].ToString();
                        item.ImportTime = dr["ImportTime"].ToString();
                        item.SignupTime = dr["SignupTime"].ToString();
                        item.SignupPage = dr["SignupPage"].ToString();

                        List<AnswerItem> listAnswers;
                        getUserAnswerList(out listAnswers, item.ID);
                        item.Answers = listAnswers;
                        list.Add(item);
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("查询签到用户信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 查询调查问题信息
        public static int getQuestionList(out List<QuestionItem> list, Dictionary<String, String[]> conditions)
        {
            list = null;
            try
            {
                using (SqlConnection conn = ConnectDb())
                {
                    SqlParameter[] pms;
                    String sql = String.Format(sql_GetQuestionList, getQuestionItemFilter(conditions, out pms));
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (pms != null) cmd.Parameters.AddRange(pms);
                    SqlDataReader dr = cmd.ExecuteReader();
                    list = new List<QuestionItem>();
                    while (dr.Read())
                    {
                        QuestionItem item = new QuestionItem();
                        item.ID = Int32.Parse(dr["ID"].ToString());
                        item.Sequence = Int32.Parse(dr["Sequence"].ToString());
                        item.Question = dr["Question"].ToString();
                        item.Valid = (bool)dr["Valid"];
                        // 获取该问题的所有答案
                        List<AnswerItem> answers;
                        Dictionary<String, String[]> cond_answer = new Dictionary<String, String[]>();
                        cond_answer.Add("QuestionID", new String[] { new Operator(OperatorTypeEnum.Equal), item.ID.ToString() });
                        getAnswerList(out answers, cond_answer);
                        item.Answers = answers;
                        item.Answers.ForEach((AnswerItem ans) =>
                        {
                            ans.Question = item;
                        });
                        list.Add(item);
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("查询调查问题信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 查询调查问题答案信息
        public static int getAnswerList(out List<AnswerItem> list, Dictionary<String, String[]> conditions)
        {
            list = null;
            try
            {
                using (SqlConnection conn = ConnectDb())
                {
                    SqlParameter[] pms;
                    String sql = String.Format(sql_GetAnswerList, getAnswerItemFilter(conditions, out pms));
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (pms != null) cmd.Parameters.AddRange(pms);
                    SqlDataReader dr = cmd.ExecuteReader();
                    list = new List<AnswerItem>();
                    while (dr.Read())
                    {
                        AnswerItem item = new AnswerItem();
                        item.ID = Int32.Parse(dr["ID"].ToString());
                        item.Sequence = Int32.Parse(dr["Sequence"].ToString());
                        item.Answer = dr["Answer"].ToString();
                        item.Valid = (bool)dr["Valid"];
                        list.Add(item);
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("查询调查问题答案信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 查询签到用户调查问题回答信息
        public static int getUserAnswerList(out List<AnswerItem> list, int user_id)
        {
            list = null;
            try
            {
                if(listQuestions == null) DbSelect.getQuestionList(out listQuestions, null);
                using (SqlConnection conn = ConnectDb())
                {
                    Dictionary<String, String[]> conditions = new Dictionary<String, String[]>();
                    conditions.Add("UserID", new String[] { new Operator(OperatorTypeEnum.Equal), user_id.ToString() });
                    SqlParameter[] pms;
                    String sql = String.Format(sql_GetUserAnswerList, getUserAnswerItemFilter(conditions, out pms));
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (pms != null) cmd.Parameters.AddRange(pms);
                    SqlDataReader dr = cmd.ExecuteReader();
                    list = new List<AnswerItem>();
                    while (dr.Read())
                    {
                        int q_id = (int)dr["QuestionID"];
                        int ans_id = (int)dr["AnswerID"];
                        QuestionItem ques = listQuestions.Find((QuestionItem q) =>
                        {
                            return q.ID == q_id;
                        });
                        if (ques == null)
                        {
                            AnswerItem answer = new AnswerItem();
                            list.Add(answer);
                        }
                        else
                        {
                            AnswerItem answer = ques.Answers.Find((AnswerItem ans) =>
                            {
                                return ans.ID == ans_id;
                            });
                            list.Add(answer);
                        }
                    }
                    dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("查询签到用户调查问题回答信息时出错：{0}", ex.Message));
            }
            return OK;
        }
        // 通用的查询条件字符串生成函数
        private static String getFilter(String[] propNames, String[] fieldNames, Dictionary<String, String[]> conditions, out SqlParameter[] pms)
        {
            if (conditions == null)
            {
                pms = null;
                return null;
            }
            pms = new SqlParameter[conditions.Count];
            String filter = null;
            int fieldID = 0, i = 0;
            foreach(String key in conditions.Keys) {
                if (filter != null) filter += " AND ";
                fieldID = Array.IndexOf(propNames, key);
                if (fieldID == -1) continue;
                filter += fieldNames[fieldID] + ' ' + conditions[key][0] + "@" + fieldNames[fieldID];
                pms[i++] = new SqlParameter("@" + fieldNames[fieldID], conditions[key][1]);
            }
            if (filter != null) filter = " WHERE " + filter;
            return filter;
        }
        // 生成签到用户信息查询条件字符串
        public static String getSignupUserItemFilter(Dictionary<String, String[]> conditions, out SqlParameter[] pms)
        {
            String s = "ID,Name,CardID,Mobile,ImportTime,SignupTime,SignupSequence,SignupPage";
            String[] propNames = s.Split(',');
            String[] fieldNames = s.Split(',');
            return getFilter(propNames, fieldNames, conditions, out pms);
        }
        // 生成调查问题信息查询条件字符串
        public static String getQuestionItemFilter(Dictionary<String, String[]> conditions, out SqlParameter[] pms)
        {
            String s = "ID,Sequence,Question,Valid";
            String[] propNames = s.Split(',');
            String[] fieldNames = s.Split(',');
            return getFilter(propNames, fieldNames, conditions, out pms);
        }
        // 生成调查问题答案信息查询条件字符串
        public static String getAnswerItemFilter(Dictionary<String, String[]> conditions, out SqlParameter[] pms)
        {
            String s = "ID,Sequence,QuestionID,Answer,Valid";
            String[] propNames = s.Split(',');
            String[] fieldNames = s.Split(',');
            return getFilter(propNames, fieldNames, conditions, out pms);
        }
        // 生成签到用户调查问题回答信息查询条件字符串
        public static String getUserAnswerItemFilter(Dictionary<String, String[]> conditions, out SqlParameter[] pms)
        {
            String s = "UserID,QuestionID,AnswerID";
            String[] propNames = s.Split(',');
            String[] fieldNames = s.Split(',');
            return getFilter(propNames, fieldNames, conditions, out pms);
        }
    }
}
