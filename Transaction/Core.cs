using DbExec;
using Entity;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.Web.UI;

namespace Transaction
{
    public static class Core
    {
        public const int OK = 0;
        public const int FAILED = 1;
        public const int CANCELLED = 2;
        public static bool isLogon = false;
        public static String uid;
        public static DateTime loginTime;

        private static int readSessionInfo(HttpSessionState session)
        {
            if (session["LoginTime"] == null)    // 未登录
                return FAILED;
            isLogon = true;
            uid = session["UserID"].ToString();
            loginTime = DateTime.Parse(session["LoginTime"].ToString());
            return OK;
        }
        public static int loginSystem(String uid, HttpSessionState session)
        {
            if (uid == null) throw new ArgumentNullException("用户名");
            try
            {
                int ret = DbSelect.checkUserPrivilege(uid);
                if (ret == FAILED)
                {
                    throw new Exception("登录系统时出错：您没有权限进入本系统后台！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("登录系统时出错：{0}", ex.Message));
            }
            session["UserID"] = uid;
            session["LoginTime"] = DateTime.Now.ToString();
            readSessionInfo(session);
            return OK;
        }
        public static int checkIfLogin(Page page)
        {
            readSessionInfo(page.Session);
            if (!isLogon)
            {
                Misc.showError(page, "您没有登录，请从“院务系统-校友管理-活动签到系统”模块登录！");
                return FAILED;
            }
            return OK;
        }
        public static int logout(Page page)
        {
            page.Session.Abandon();
            page.Response.Redirect(SiteSettings.PAGE_MAIN_ASP);
            return OK;
        }

        public static int importSignupList(String excelFile, out int importedCount)
        {
            return DbExcel.importSignupUserList(excelFile, out importedCount);
        }
        public static int getSignupList(out List<SignupUserItem> list, Dictionary<String, String[]> conditions = null)
        {
            return DbSelect.getSignupList(out list, conditions);
        }
        public static int exportSignupList(out String excelFile, Dictionary<String, String[]> conditions = null)
        {
            excelFile = null;
            String destPath = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.DIR_EXPORT_EXCEL);
            String filename = Misc.generateFilename("签到名单", "xls");
            Misc.verifyDirectory(destPath);
            excelFile = Misc.url(SiteSettings.SITE_ROOT, SiteSettings.DIR_EXPORT_EXCEL, filename);
            return DbExcel.exportSignupList(Misc.path(destPath, filename), conditions);
        }
        public static int addSignupUserItem(ref SignupUserItem item)
        {
            item.Name = item.Name.Replace(" ", "").Trim();
            item.CardID = item.CardID.Replace(" ", "").Trim();
            item.Mobile = item.Mobile.Replace(" ", "").Trim();
            if (item.Name.Length == 0 || item.CardID.Length == 0 || item.Mobile.Length == 0)
            {
                throw new Exception("请填写您的姓名、身份证号码和手机号码！");
            }
            else if (item.CardID.Length > 0 && !Regex.Match(item.CardID, @"^\d{17}(\d|X)$", RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("身份证号码格式不正确，请检查是否有误！");
            }
            else if (item.Mobile.Length > 0 && !Regex.Match(item.Mobile, @"^\d{11}$").Success)
            {
                throw new Exception("手机号码格式不正确，请检查是否有误！");
            }
            
            List<AnswerItem> answers = item.Answers;
            List<QuestionItem> ques;
            DbSelect.getQuestionList(out ques, null);
            if (!ques.TrueForAll((QuestionItem q) => answers.Exists((AnswerItem ans) => ans.Question.ID == q.ID)))
            {
                throw new Exception("请确认是否已回答所有问题！");
            }

            List<SignupUserItem> list;
            Dictionary<String, String[]> conditions = new Dictionary<String, String[]>();
            conditions.Add("Name", new String[] { new Operator(OperatorTypeEnum.Equal), item.Name });
            conditions.Add("CardID", new String[] { new Operator(OperatorTypeEnum.Equal), item.CardID });
            conditions.Add("Mobile", new String[] { new Operator(OperatorTypeEnum.Equal), item.Mobile });
            int ret = DbSelect.getSignupList(out list, conditions);
            if (ret != OK) return FAILED;
            if (list.Count == 0)  // 无匹配用户
            {
                throw new Exception("考生信息不符，无法查询！");
            }
            else
            {
                item = list[0];
                item.Answers = answers;
                if (list[0].SignupSequence.Length != 0) // 已签到
                {
                    return CANCELLED;
                }
            }
            return DbEdit.updateSignupUserItem(ref item);
        }
        public static int recoverSignupUserItem(int ID)
        {
            return DbEdit.recoverSignupUserItem(ID);
        }
        public static int recoverAllSignupUserItems()
        {
            return DbEdit.recoverAllSignupUserItems();
        }
        public static int deleteSignupUserItem(int ID)
        {
            return DbEdit.deleteSignupUserItem(ID);
        }
        public static int deleteAllSignupUserItems()
        {
            return DbEdit.deleteAllSignupUserItems();
        }
    }
}