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
    public partial class editPageNotice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            if (!IsPostBack) {
                try
                {
                    String htmlPageNotice, content;
                    Misc.getNoticePage(out htmlPageNotice);
                    // 获取模板页内容
                    Match match = Regex.Match(htmlPageNotice, "(?<=<body>).*(?=</body>)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (match.Success)
                        content = match.Value;
                    else
                        content = htmlPageNotice;
                    ftbPage.DesignModeCss = Misc.url(SiteSettings.SITE_ROOT, SiteSettings.DIR_TEMPLATE, SiteSettings.CSS_NOTICE_TEMPLATE);
                    ftbPage.Text = content;
                    // 获取模板页标题
                    match = Regex.Match(htmlPageNotice, "(?<=<title>).*(?=</title>)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (match.Success) txtTitle.Text = match.Value;
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
                Misc.setNoticePage(ftbPage.Text, txtTitle.Text);
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, "保存模板成功。");
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            String htmlPage = Misc.generateNoticePagePreview(ftbPage.Text, txtTitle.Text);
            Response.Write(htmlPage);
            Response.End();
        }
    }
}