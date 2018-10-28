using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Transaction;

namespace SignupSys
{
    public partial class editPageSignup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            if (!IsPostBack) {
                try
                {
                    String htmlPageSignup, content;
                    Misc.getSignupPage(out htmlPageSignup);
                    // 获取签到页内容
                    Match match = Regex.Match(htmlPageSignup, "(?<=<div class=\"personal-info\">).*?(?=</div>)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (match.Success)
                        content = match.Value;
                    else
                        content = htmlPageSignup;
                    ftbPage.DesignModeCss = Misc.url(SiteSettings.SITE_ROOT, "App_Themes/user/page.css");
                    ftbPage.Text = content;
                    // 获取签到页标题
                    match = Regex.Match(htmlPageSignup, "(?<=<title>).*(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (match.Success) txtTitle.Text = match.Value;
                    ClientScript.RegisterClientScriptInclude(this.GetType(), "", "Scripts/jquery-1.7.1.min.js");
                    ClientScript.RegisterClientScriptInclude(this.GetType(), "", "Scripts/signup.js");
                }
                catch (Exception ex)
                {
                    Misc.showError(this, ex.Message);
                }
            }
        }
        protected void btnIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Misc.setSignupPage(ftbPage.Text, txtTitle.Text);
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, "保存页面成功。");
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            String htmlPage = Misc.generateSignupPagePreview(ftbPage.Text, txtTitle.Text);
            Response.Write(htmlPage);
            Response.End();
        }
    }
}