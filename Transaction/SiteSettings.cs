using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Configuration;

namespace Transaction
{
    public static class SiteSettings
    {
        // 站点虚拟目录
        public const String DIR_UPLOAD_FILES = "uploadFiles";
        public const String DIR_TEMPLATE = "template";
        public const String DIR_GENERATED = "gen";
        public const String DIR_IMAGES = "images";
        public const String DIR_EXPORT_EXCEL = "gen/excel";
        public const String PAGE_MAIN_ASP = "/admin/home.asp";
        public const String PAGE_ERR = "err.aspx";
        public const String PAGE_NOTICE_TEMPLATE = "notice.html";
        public const String CSS_SIGNUP_PAGE = "App_Themes/user/page.css";
        public const String CSS_NOTICE_TEMPLATE = "notice.css";
        public static String SITE_DOMAIN;
        public static String SITE_ROOT;
        public static String SITE_PHYSICAL_PATH;
        public static String PAGE_SIGNUP;
        public static String SCRIPT_SIGNUP;
        public static String IMAGE_QRCODE;
        public static void init(HttpServerUtility server)
        {
            NameValueCollection settings = WebConfigurationManager.AppSettings;
            SITE_DOMAIN = settings["SiteDomain"].ToString();
            SITE_ROOT = settings["SiteRoot"].ToString();
            SITE_PHYSICAL_PATH = server.MapPath(Misc.url(SITE_ROOT));
            PAGE_SIGNUP = settings["SignupPage"].ToString();
            SCRIPT_SIGNUP = settings["SignupScript"].ToString();
            IMAGE_QRCODE = settings["QRCodeImagePath"].ToString();
        }
    }
}