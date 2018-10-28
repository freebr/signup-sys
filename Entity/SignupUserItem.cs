using System;
using System.Collections.Generic;

namespace Entity
{
    public class SignupUserItem
    {
        private int m_id;
        private int m_signup_seq;               // 签到序号
        private String m_assign_id;             // 分配序号（签到编号）
        private String m_username;              // 签到者姓名
        private String m_card_id;               // 签到者身份证号码
        private String m_signup_mobile;         // 签到者电话号码
        private DateTime m_import_time;         // 导入时间
        private DateTime m_signup_time;         // 签到时间
        private String m_signup_page;           // 签到页面名称
        private List<AnswerItem> m_answers;     // 用户选择答案
        private bool m_valid = true;

        public int ID {
            get { return m_id; }
            set { m_id = value; }
        }
        public String SignupSequence
        {
            get { return m_signup_seq == 0 ? "" : m_signup_seq.ToString(); }
            set { m_signup_seq = value.Length == 0 ? 0 : int.Parse(value); }
        }
        public String AssignID
        {
            get { return m_assign_id; }
            set { m_assign_id = value; }
        }
        public String Name
        {
            get { return m_username; }
            set { m_username = value; }
        }
        public String CardID
        {
            get { return m_card_id; }
            set { m_card_id = value; }
        }
        public String Mobile
        {
            get { return m_signup_mobile; }
            set { m_signup_mobile = value; }
        }
        public String ImportTime
        {
            get { return m_import_time == DateTime.MinValue ? "" : m_import_time.ToString(); }
            set { m_import_time = value.Length == 0 ? DateTime.MinValue : DateTime.Parse(value); }
        }
        public String SignupTime
        {
            get { return m_signup_time == DateTime.MinValue ? "" : m_signup_time.ToString(); }
            set { m_signup_time = value.Length == 0 ? DateTime.MinValue : DateTime.Parse(value); }
        }
        public String SignupPage
        {
            get { return m_signup_page; }
            set { m_signup_page = value; }
        }
        public List<AnswerItem> Answers
        {
            get { return m_answers; }
            set { m_answers = value; }
        }
        public string TextAnswers
        {
            get
            {
                string ret = "";
                int i = 0;
                m_answers.ForEach((AnswerItem ans) =>
                {
                    if (ret.Length > 0) ret += "\n";
                    if (ans.Question == null)
                    {
                        ret += String.Format("{0}.（问题已被删除）", ++i);
                    }
                    else
                    {
                        ret += String.Format("{0}.{1}：{2}", ++i, ans.Question.Question, ans.Answer);
                    }
                });
                return ret;
            }
        }
        public bool Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }
    }
}
