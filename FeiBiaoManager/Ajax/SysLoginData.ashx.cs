using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Model;
using Newtonsoft.Json;

namespace WebSmartSchool.Ajax
{
    /// <summary>
    /// SysLoginData 的摘要说明
    /// </summary>
    public class SysLoginData : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request["action"];
            DoAction(action, context);
        }


        private void DoAction(string action, HttpContext context)
        {
            string result = string.Empty;
            switch (action)
            {
                case "Login":
                    string username = context.Request["Account"];
                    string pwd = context.Request["Pwd"];
                    if (!string.IsNullOrEmpty(username))
                    {
                        result = SysLogin(username, pwd);
                    }
                    break;
            }
            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();
        }

        /// <summary>
        /// 系统登录
        /// </summary>
        /// <returns></returns>
        private string SysLogin(string username,string pwd)
        {
            User jsonResult = new User();
            int result = user.userLogin(username, pwd);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "登录成功";
                jsonResult.Username = username;
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "用户名或密码不匹配";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}