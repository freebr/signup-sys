using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;

namespace SignupSys
{
    public partial class exportSignupList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            try
            {
                String excelFile;
                int ret = Core.exportSignupList(out excelFile);
                ExcelFile.Value = excelFile;
                LastPage.Value = Request.UrlReferrer.PathAndQuery;
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
                script.Visible = false;
            }
        }
    }
}