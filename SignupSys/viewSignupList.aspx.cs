using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Transaction;
using Entity;

namespace SignupSys
{
    public partial class ViewSignupList : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            Core.checkIfLogin(this);
            try
            {
                List<SignupUserItem> list;
                Core.getSignupList(out list);
                int countSignedUp = list.FindAll((SignupUserItem item) =>
                {
                    return item.SignupSequence.Length > 0;
                }).Count;
                lblRecordInfo.Text = String.Format("导入名单 {0} 人，已签到 {1} 人", list.Count, countSignedUp);
                gridSignupList.DataSource = list;
                gridSignupList.DataBind();
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
            gridSignupList.Sorting += gridSignupList_Sorting;
            gridSignupList.RowEditing += gridSignupList_RowEditing;
            gridSignupList.RowDeleting += gridSignupList_RowDeleting;
            String[] scriptFiles = {"jquery-1.7.1.min.js", "signuplist.js"};
            Array.ForEach<String>(scriptFiles, delegate(String elem)
            {
                ClientScript.RegisterClientScriptInclude(this.GetType(), elem, "Scripts/" + elem);
            });
            ClientScript.RegisterStartupScript(this.GetType(), "","init();", true);
        }

        void gridSignupList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int id = int.Parse(gridSignupList.Rows[e.NewEditIndex].Cells[0].Text);
                Core.recoverSignupUserItem(id);
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, "清空成功。");
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
        }

        void gridSignupList_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<SignupUserItem> list = (List<SignupUserItem>)gridSignupList.DataSource;
            list.Sort((SignupUserItem a, SignupUserItem b) =>
            {
                int seq_a = int.Parse(a.SignupSequence);
                int seq_b = int.Parse(b.SignupSequence);
                if (e.SortDirection == SortDirection.Ascending)
                {
                    if (seq_a > seq_b || a.SignupSequence.Length == 0) return 1;
                    return 0;
                }
                else
                {
                    if (seq_b > seq_a || b.SignupSequence.Length == 0) return 1;
                    return 0;
                }
            });
        }

        protected void gridSignupList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int ret = Core.deleteSignupUserItem((int)e.Values["ID"]);
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, "删除成功。");
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
        }

        protected void btnIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnRecoverAll_Click(object sender, EventArgs e)
        {
            try
            {
                int ret = Core.recoverAllSignupUserItems();
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, "清空成功。");
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                int ret = Core.deleteAllSignupUserItems();
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, "删除成功。");
            }
            catch (Exception ex)
            {
                Misc.doPageJump(this, Request.UrlReferrer.PathAndQuery, ex.Message);
            }
        }

    }
}