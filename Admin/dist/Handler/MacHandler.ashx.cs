using DAL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Admin.dist.Handler
{
    /// <summary>
    /// MacHandler 的摘要说明
    /// </summary>
    public class MacHandler : IHttpHandler
    {
        ManagerServer ms = new ManagerServer();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = "";
            action = context.Request["action"];
            DoAction(action, context);
        }

        private void DoAction(string action, HttpContext context)
        {
            string result = string.Empty;
            switch (action)
            {
                case "updateMacInfo":
                    result = updateMacInfo(context);
                    break;
                case "macDebug":
                    result = macDebug(context);
                    break;
                case "getDebugInfo":
                    result = getDebugInfo(context);
                    break;
                case "updateDebugInfoState":
                    result = updateDebugInfoState(context);
                    break;
                default:
                    result = "没有接收到任何对应参数";
                    break;
            }
            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();
        }

        private string updateMacInfo(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            string state = context.Request["state"];
            string macNum = context.Request["machineNum"];
            string info = context.Request["info"];
            string biaoNum = context.Request["biaoNum"];
            string address = context.Request["address"];

            int result = ms.updateMacStateById(state, macNum, info, id,address,biaoNum);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "操作成功";
            }
            else if (result == -1)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "发生异常";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "未知错误";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string macDebug(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            string command = context.Request["command"];
            int result = ms.insertDebugInfo(id, command);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "操作成功";
            }
            else if (result == -1)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "发生异常";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "未知错误";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string getDebugInfo(HttpContext context)
        {
            Json jsonResult = new Json();
            string macNum = context.Request["macNum"];
            DataTable result = ms.getDebugInfo(macNum);
            if (result == null || result.Rows.Count==0)
            {
                jsonResult.Success = false;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.Result = result;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string updateDebugInfoState(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            int result = ms.updateDebugInfoState(id);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "操作成功";
            }
            else if (result == -1)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "发生异常";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "未知错误";
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