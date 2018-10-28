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
    public partial class editQA : System.Web.UI.Page
    {
        protected bool questionSelected = false;
        protected bool? questionValid = null;
        protected bool? answerValid = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings.init(Server);
            Core.checkIfLogin(this);
            gridQuestions.SelectedIndexChanged += gridQuestions_SelectedIndexChanged;
            SqlDataSourceQuestion.Inserting += SqlDataSourceQuestion_Inserting;
            SqlDataSourceAnswer.Inserting += SqlDataSourceAnswer_Inserting;
            SqlDataSourceQuestion.InsertCommand = "INSERT INTO Questions (Question) VALUES (@Question)";
            SqlDataSourceQuestion.UpdateCommand = "UPDATE Questions SET Question=@Question WHERE ID=@ID";
            SqlDataSourceQuestion.DeleteCommand = "DELETE FROM Questions WHERE ID=@ID";
            SqlDataSourceAnswer.InsertCommand = "INSERT INTO Answers (QuestionID,Answer) VALUES (@QuestionID,@Answer)";
            SqlDataSourceAnswer.UpdateCommand = "UPDATE Answers SET Answer=@Answer WHERE ID=@ID";
            SqlDataSourceAnswer.DeleteCommand = "DELETE FROM Answers WHERE ID=@ID";
            String[] scriptFiles = { "jquery-1.7.1.min.js", "editqa.js" };
            Array.ForEach<String>(scriptFiles, (String elem) =>
            {
                ClientScript.RegisterClientScriptInclude(this.GetType(), elem, "Scripts/" + elem);
            });
            ClientScript.RegisterStartupScript(this.GetType(), "", "init();", true);
        }

        void gridQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            questionSelected = gridQuestions.SelectedValue != null;
        }

        protected void gridQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "New")
            {
                gridQuestions.DataSourceID = "";
                gridQuestions.DataBind();
            }
        }

        protected void gridAnswers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            questionSelected = gridQuestions.SelectedValue != null;
            if (e.CommandName == "New")
            {
                gridAnswers.DataSourceID = "";
                gridAnswers.DataBind();
            }
        }

        protected void dvwNewQuestion_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                questionValid = ((TextBox)((DetailsView)sender).FindControl("ctl01")).Text.Length != 0;
                if (questionValid == false)
                {
                    gridQuestions.DataBind();
                    return;
                }
            }
            gridQuestions.DataSourceID = "SqlDataSourceQuestion";
            gridQuestions.DataBind();
        }

        protected void dvwNewAnswer_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            questionSelected = gridQuestions.SelectedValue != null;
            if (e.CommandName == "Insert")
            {
                answerValid = ((TextBox)((DetailsView)sender).FindControl("ctl01")).Text.Length != 0;
                if (answerValid == false)
                {
                    gridAnswers.DataBind();
                    return;
                }
                SqlDataSourceAnswer.InsertParameters.Add(SqlDataSourceAnswer.SelectParameters[0]);
            }
            gridAnswers.DataSourceID = "SqlDataSourceAnswer";
            gridAnswers.DataBind();
        }
        
        protected void btnIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        void SqlDataSourceQuestion_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Cancel = questionValid == false;
        }

        void SqlDataSourceAnswer_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Cancel = answerValid == false;
        }

    }
}