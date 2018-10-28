using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;

namespace SignupSys
{
    public partial class viewQRCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            ltrSignupUrl.Text = SiteSettings.SITE_DOMAIN + Misc.url(SiteSettings.SITE_ROOT, SiteSettings.PAGE_SIGNUP);
            imgQRCode.ImageUrl = SiteSettings.SITE_DOMAIN + Misc.url(SiteSettings.SITE_ROOT, SiteSettings.IMAGE_QRCODE);
        }
        protected void btnIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }
}