using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;
using Entity;

namespace SignupSys
{
    public partial class ImportSignupList : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            Core.checkIfLogin(this);

            if (!this.IsPostBack && Request.Files.Count > 0) importFile();
        }

        protected void btnIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            importFile();
        }
        private void importFile()
        {
            int importedCount = 0;
            SiteSettings.init(Server);
            try {
                if (Request.Files.Count == 0) throw new Exception("操作无效。");
                String destFile;
                int ret = Misc.saveUploadedFile(out destFile, Request.Files[0]);
                ret = Core.importSignupList(destFile, out importedCount);
                Misc.doPageJump(this, "viewSignupList.aspx", String.Format("导入成功，{0} 条记录已导入。", importedCount));
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
        }
    }
}