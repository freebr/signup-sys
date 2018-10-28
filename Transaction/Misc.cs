using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using DbExec;
using Entity;

namespace Transaction
{
    public static class Misc
    {
        private const String HTML_PAGE_SIGNUP = @"<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /><meta name=""viewport"" content=""width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no"" /><title>{0}</title><link rel=""stylesheet"" href=""{1}"" /><script type=""text/javascript"" src=""Scripts/jquery-1.7.1.min.js""></script><script type=""text/javascript"" src=""Scripts/signup.js""></script></head>
<body><div class=""container""><div class=""signupbox""><form name=""fmSignup"" action=""{2}"" method=""post"">
<div class=""personal-info"">{3}</div>{4}
<p><input type=""submit"" name=""btnsubmit"" value=""提     交"" onclick=""this.form.submit();""></p>
</form></div></div></body></html>";
        private const String HTML_SURVEY_DIV = "<div class=\"survey\"><p>以下选项，请根据您的需要选择：</p>{0}</div>";
        private const String HTML_SURVEY_QUESTION = @"<div class=""question"">
<p>{0}.{1}</p>
<p>{2}</p></div>";
        private const String HTML_SURVEY_ANSWER = "<label for=\"ques{0}{1}\"><input type=\"radio\" name=\"ques{0}\" id=\"ques{0}{1}\" value=\"{1}\" />{2}</label>";
        private const String HTML_PAGE_NOTICE = "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\" /><title>{0}</title><link rel=\"stylesheet\" href=\"{1}\" /></head><body>{2}</body></html>";
        public const int OK = 0;
        public const int FAILED = 1;

        public static String generateFilename(String prefix = null, String fileExt = null)
        {
            // yyyy_m_d_h_i_s.ext
            if (prefix != null) prefix += '_';
            return prefix + DateTime.Now.ToShortDateString().Replace('/', '_') + '_' +
                   DateTime.Now.ToLongTimeString().Replace(':', '_') + '.' + fileExt;
        }
        public static int verifyDirectory(String dir)
        {
            try
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return OK;
        }
        public static int saveUploadedFile(out String destFile, HttpPostedFile file)
        {
            destFile = null;
            if (file.FileName.Length == 0)
            {
                throw new Exception("请选择要上传的 Excel 文件！");
            }
            String fileExt = file.FileName;
            fileExt = fileExt.Substring(fileExt.LastIndexOf('.') + 1);
            String destPath = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.DIR_UPLOAD_FILES);
            destFile = destPath + '/' + generateFilename(fileExt: fileExt);
            try
            {
                verifyDirectory(destPath);
                file.SaveAs(destFile);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("保存文件时出错：{0}", ex.Message));
            }
            return OK;
        }
        public static int getSignupPage(out String htmlPage)
        {
            htmlPage = null;
            String signupFile = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.PAGE_SIGNUP);
            try
            {
                if (!File.Exists(signupFile)) throw new FileNotFoundException();
                htmlPage = File.ReadAllText(signupFile);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("读取签到页文件'{0}'时出错：{1}", signupFile, ex.Message));
            }
            return OK;
        }
        public static int getNoticePage(out String htmlPage)
        {
            htmlPage = null;
            String templateFile = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.DIR_TEMPLATE, SiteSettings.PAGE_NOTICE_TEMPLATE);
            try
            {
                if (!File.Exists(templateFile)) throw new FileNotFoundException();
                htmlPage = File.ReadAllText(templateFile);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("读取模板文件'{0}'时出错：{1}", templateFile, ex.Message));
            }
            return OK;
        }
        public static String getSurveyContent()
        {
            String code_survey = "";
            String code_questions = "";

            List<QuestionItem> ques;
            DbSelect.getQuestionList(out ques, null);
            ques.ForEach((QuestionItem q) =>
            {
                String code_answers = "";
                q.Answers.ForEach((AnswerItem ans) =>
                {
                    String code_answer = String.Format(HTML_SURVEY_ANSWER, ans.Question.ID, ans.ID, ans.Answer);
                    code_answers += code_answer;
                });
                String code_question = String.Format(HTML_SURVEY_QUESTION, q.Sequence, q.Question, code_answers);
                code_questions += code_question;
            });
            if (code_questions.Length == 0) return "";
            code_survey = String.Format(HTML_SURVEY_DIV, code_questions);
            return code_survey;
        }
        public static int setSignupPage(String content, String title)
        {
            String signupFile = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.PAGE_SIGNUP);
            
            // 添加标题、HTML样式表、表单内容和调查问题代码
            String htmlPage = String.Format(HTML_PAGE_SIGNUP, title, Misc.url(SiteSettings.SITE_ROOT, SiteSettings.CSS_SIGNUP_PAGE), SiteSettings.SCRIPT_SIGNUP, content, getSurveyContent());
            try
            {
                File.WriteAllText(signupFile, htmlPage);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("保存签到页文件'{0}'时出错：{1}", signupFile, ex.Message));
            }
            return OK;
        }
        public static int setNoticePage(String content, String title)
        {
            String templateFile = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.DIR_TEMPLATE, SiteSettings.PAGE_NOTICE_TEMPLATE);
            // 添加HTML样式表等信息
            String htmlPage = String.Format(HTML_PAGE_NOTICE,
                        title, Misc.url(SiteSettings.SITE_ROOT, SiteSettings.DIR_TEMPLATE, SiteSettings.CSS_NOTICE_TEMPLATE), content);
            try
            {
                File.WriteAllText(templateFile, htmlPage);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("保存模板文件'{0}'时出错：{1}", templateFile, ex.Message));
            }
            return OK;
        }
        // 生成签到页预览
        public static String generateSignupPagePreview(String content, String title)
        {
            String htmlPage = String.Format(HTML_PAGE_SIGNUP, title, Misc.url(SiteSettings.SITE_ROOT, SiteSettings.CSS_SIGNUP_PAGE), "about:blank", content, getSurveyContent());
            return htmlPage;
        }
        // 基于模板生成指定文件名的通知页
        public static int generateNoticePage(String destFile, NameValueCollection vars)
        {
            String destPath = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.DIR_GENERATED);
            String templateFile = Misc.path(SiteSettings.SITE_PHYSICAL_PATH, SiteSettings.DIR_TEMPLATE, SiteSettings.PAGE_NOTICE_TEMPLATE);
            String tmpFile = destPath + '\\' + destFile;
            String htmlPageNotice;
            
            try
            {
                if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);
                if (!File.Exists(templateFile)) throw new FileNotFoundException();
                htmlPageNotice = File.ReadAllText(templateFile);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("读取模板文件'{0}'时出错：{1}", templateFile, ex.Message));
            }
            try
            {
                vars.Add("$template_dir", Misc.url(SiteSettings.SITE_ROOT, SiteSettings.DIR_TEMPLATE));
                // 应用变量值
                foreach (object e in vars.Keys)
                {
                    htmlPageNotice = htmlPageNotice.Replace(e.ToString(), vars[e.ToString()]);
                }
                File.WriteAllText(tmpFile, htmlPageNotice);
                return OK;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("生成通知页时出错：{0}", ex.Message));
            }
        }
        // 生成通知页预览
        public static String generateNoticePagePreview(String content, String title)
        {
            String htmlPage = String.Format(HTML_PAGE_NOTICE,
                        title, Misc.url(SiteSettings.SITE_ROOT, SiteSettings.DIR_TEMPLATE, SiteSettings.CSS_NOTICE_TEMPLATE), content);
            htmlPage = htmlPage.Replace("$template_dir", Misc.url(SiteSettings.SITE_ROOT, SiteSettings.DIR_TEMPLATE));
            return htmlPage;
        }
        public static int doPageJump(Page page, String jumpUrl, String notice = null)
        {
            if (notice == null)
            {
                page.Response.Redirect(jumpUrl, false);
            }
            else
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "pageJump",
                    String.Format("<script type=\"text/javascript\">alert(\"{0}\");location.href=\"{1}\";</script>",
                    notice.Replace(@"\", @"\\").Replace("\"","\\\"").Replace("\r", @"\r").Replace("\n", @"\n"), jumpUrl));
            }
            return OK;
        }
        public static int showError(Page page, String errMsg = null)
        {
            page.Session["ErrorMsg"] = errMsg;
            page.Response.Redirect(url(SiteSettings.SITE_ROOT, SiteSettings.PAGE_ERR), false);
            return OK;
        }
        public static String url(params String[] parts)
        {
            String tmp = "", part;
            for (int i = 0; i < parts.Length; i++)
            {
                part = parts[i].Trim('/');
                if (part.Length > 0) tmp += '/' + part;
            }
            return tmp;
        }
        public static String path(params String[] parts)
        {
            String tmp = "", part;
            for (int i = 0; i < parts.Length; i++)
            {
                part = parts[i].Trim('/', '\\');
                if (part.Length > 0) tmp += part;
                if (i < parts.Length - 1) tmp += '\\';
            }
            return tmp;
        }
    }
}
