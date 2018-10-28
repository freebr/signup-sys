using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;

namespace SignupSys
{
    public partial class about : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            String strAboutTxt = "系统全名：\t华工工管活动签到系统\n" +
                           "系统版本：\t1.4\n" +
                           "开发单位：\tfreebr &copy; 2015～" + DateTime.Now.Year + "\n" +
                           "编译日期：\t2018-5-11\n" +
                           "系统功能简介：\n\n" +
                           "\t本系统针对学院活动签到的需求，提供二维码签到功能，签到人员只需用手机扫一扫二维码，进入签到页面填写简单的个人信息，系统即为其自动分配签到序号，同时记录签到时间等关键信息。\n" +
                           "\t同时，系统面向教务员提供一个管理后台，实现签到名单的管理、导入和导出，以及扫描签到页、签到成功通知页的内容编辑等功能。\n";
            ltrDesc.Text = strAboutTxt.Replace("\n", "<br/>").Replace("\t", "&emsp;&emsp;");
        }
        protected void btnIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }
}