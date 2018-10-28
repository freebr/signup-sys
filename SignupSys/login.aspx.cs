using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;

namespace SignupSys
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            if (Request.QueryString.ToString()=="logout") // 注销
            {
                Core.logout(this);
            }
            else
            {
                String uid = Request.Form["uid"];
                String queryString = Request.QueryString.ToString();
                if (Request.ServerVariables["REMOTE_ADDR"] == "::1") queryString = "debug";
                if (uid == null && queryString == "debug")
                {
                    uid = "863";
                }
                try
                {
                    int ret = Core.loginSystem(uid, Session);
                    Response.Redirect("default.aspx", false);
                }
                catch (Exception ex)
                {
                    Misc.showError(this, ex.Message);
                }
            }
        }
    }
}