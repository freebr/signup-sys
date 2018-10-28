using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;
using Entity;

namespace SignupSys
{
    public partial class signUp : System.Web.UI.Page
    {
        private const String PAGE_SIGNUP = "template/Signup.html";
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            try
            {
                String person_name = Request.Form["person_name"];
                String person_card_id = Request.Form["person_card_id"];
                String person_mobile = Request.Form["person_mobile"];
                String url_Signup = null;
                SignupUserItem item = new SignupUserItem();
                Application["lastRequestTime"] = DateTime.Now.Ticks;
                Application.Lock();
                item.Name = person_name;
                item.CardID = person_card_id;
                item.Mobile = person_mobile;
                item.Answers = new List<AnswerItem>();
                foreach (string key in Request.Form.Keys)
                {
                    if (!key.StartsWith("ques")) continue;
                    int q_id = Int32.Parse(key.Substring(4));
                    item.Answers.Add(new AnswerItem() {
                        ID = Int32.Parse(Request.Form[key]),
                        Question = new QuestionItem() { ID=q_id }
                    });
                }
                int ret = Core.addSignupUserItem(ref item);
                Application.UnLock();

                if (ret == Core.OK)
                {
                    NameValueCollection vars = new NameValueCollection();
                    vars.Add("$person_name", item.Name);
                    vars.Add("$person_card_id", item.CardID);
                    vars.Add("$person_mobile", item.Mobile);
                    vars.Add("$assign_id", item.AssignID);
                    vars.Add("$signup_seq", item.SignupSequence);
                    ret = Misc.generateNoticePage(item.SignupPage, vars);
                }
                else if (ret == Core.CANCELLED)
                {
                    url_Signup = item.SignupPage;
                    if (url_Signup.Length == 0)
                    {
                        throw new Exception("签到页面信息丢失！");
                    }
                }
                // 跳转到通知页
                url_Signup = Misc.url(SiteSettings.SITE_ROOT, SiteSettings.DIR_GENERATED, item.SignupPage);
                Response.Redirect(url_Signup, false);
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, "javascript:history.go(-1);", ex.Message);
            }
        }
    }
}