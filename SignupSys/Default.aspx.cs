using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;

namespace SignupSys
{
    public partial class _Default : System.Web.UI.Page
    {
        private String strItemNames = "签到名单管理:  查看签到名单|viewSignupList.aspx|tbl,导入待签到用户名单|importSignupList.aspx|imp,导出签到名单|exportSignupList.aspx|exp;" +
                                      "页面内容管理:  编辑签到页模板|editPageSignup.aspx|,编辑通知页模板|editPageNotice.aspx|,编辑调查问题和答案|editQA.aspx|sur;" +
                                      "登录与其他:    查看签到二维码|viewQRCode.aspx|,关于本系统|about.aspx|,注销|login.aspx?logout|exit";
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            Core.checkIfLogin(this);
            
            SpriteList.PreRender += SpriteList_PreRender;
        }
        private void SpriteList_PreRender(object sender, EventArgs e)
        {
            if (Request.QueryString.ToString() == "debug")
            {
                SpriteList.InnerHtml = "<p align=\"center\">系统调试中，暂停使用！</p>";
                return;
            }

            String[] arrTmp = strItemNames.Split(';');
            String[] arrTmp2;
            String codeSpriteList = "";
            String sprite_class;
            for (int i = 0; i < arrTmp.Length; i++)
            {
                arrTmp2 = arrTmp[i].Split(':', ',', '|');
                // 写功能组名
                codeSpriteList += String.Format("<div class=\"sprite_group_header\"><span class=\"sprite_group_name\" onclick=\"\">{0}</span></div>",
                    arrTmp2[0]);
                codeSpriteList += "<div class=\"sprite_group_content\">";
                for (int j = 1; j < arrTmp2.Length - 1; j += 3)
                {
                    sprite_class = arrTmp2[j + 2].Trim();
                    if (sprite_class.Length == 0) sprite_class = "def";
                    codeSpriteList += String.Format("<p class=\"{0}\"><a href=\"{1}\" class=\"menu\">{2}</a></p>", sprite_class, arrTmp2[j + 1].Trim(), arrTmp2[j].Trim());
                }
                codeSpriteList += "</div>";
                SpriteList.InnerHtml = codeSpriteList;
            }
        }
    }
}