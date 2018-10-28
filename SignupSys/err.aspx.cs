using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignupSys
{
    public partial class err : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrSignup.Text = Session["ErrorMsg"].ToString();
        }

        protected void btnIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }
}