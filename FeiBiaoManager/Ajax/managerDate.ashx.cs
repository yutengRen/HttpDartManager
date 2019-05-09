using DAL;
//using Framework.Common;
//using Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using Newtonsoft.Json;
using System.Data;

namespace FeiBiaoManager.Ajax
{
    /// <summary>
    /// managerDate 的摘要说明
    /// </summary>
    public class managerDate : IHttpHandler
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
                case "GetType":
                    result = Getppap();
                    break;
                case "GetTypeName":
                    string id = context.Request["id"];
                    result = Getppaptest(id);
                    break;
                case "addGame":
                    result=addGame(context);
                    break;
                case "addGetScore":
                    result = addGetScore(context);
                    break;
                case "get01gameScore":
                    result = select01GameScore(context);
                    break;
                case "getMiGameScore":
                    result = selectMouseGameScore(context);
                    break;
                case "updateGameState":
                    result = updateGameState(context);
                    break;
                case "getChooseList":
                    result = getChooseList(context);
                    break;
                case "starGame":
                    result = starGame(context);
                    break;
                case "getStarInfo":
                    result = getStarInfo(context);
                    break;
                case "GetMachine":
                    result = getMachine(context);
                    break;
                case "regesit":
                    result = userRegesit(context);
                    break;
                case "setSatrState":
                    result = setSatrState(context);
                    break;
                case "selectBiaoNum":
                    result = selectBiaoNum(context);
                    break;
                case "selectOnce01GameScore":
                    result = selectOnce01GameScore(context);
                    break;
                case "selectOnceMiGameScore":
                    result = selectOnceMiGameScore(context);
                    break;
                default:
                    break;
            }
            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();
        }

        private string Getppap()
        {
            Json jsonResult = new Json();

            try
            {
                var list = ms.dtType();
                jsonResult.Success = true;
                jsonResult.Msg = "";
                jsonResult.Result = list;
                jsonResult.PageCount = 10;
                jsonResult.RowCount = 100;
            }
            catch (Exception ex)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "获取用户分页列表异常：" + ex.Message;
            }

            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }


        private string Getppaptest(string gname)
        {
            Json jsonResult = new Json();
            //int pageId = RequestUtil.GetInt("pid", 1);
            //int pageSize = RequestUtil.GetInt("psize", 10);
            //string gname = RequestUtil.GetString("id");
            try
            {
                var list = ms.dtTypebyid(gname);
                jsonResult.Success = true;
                jsonResult.Msg = "";
                jsonResult.Result = list;
                jsonResult.PageCount = 10;
                jsonResult.RowCount = 100;
            }
            catch (Exception ex)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "获取用户分页列表异常：" + ex.Message;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 添加用户选择的游戏
        /// </summary>
        /// <param name="gameid"></param>
        /// <returns></returns>
        private string addGame(HttpContext context)
        {
            string gameid = context.Request["TypeIdss"];
            string username = context.Request["username"];
            string machineid = context.Request["machineids"];
            string jieBiaoNum = context.Request["jieBiaoNum"];
            Json jsonResult = new Json();
            string time = DateTime.Now.ToString();
            int result = ms.addUserChooseGames(gameid, username, time, "0", machineid,jieBiaoNum);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "添加成功";
            }
            else if(result == -2)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "当前还有游戏没有完成，不能添加";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "未知错误";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 添加游戏分数的方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addGetScore(HttpContext context)
        {
            Json jsonResult = new Json();
            string gameid = context.Request["gameid"];
            string userid = context.Request["userid"];
            string juid = context.Request["juid"];
            string score = context.Request["score"];
            string flag = context.Request["flag"];

            string beinum = context.Request["beinum"];
            string realscore = context.Request["realscore"];
            string lunnum = context.Request["lunnum"];
            string biaonum = context.Request["biaonum"];
            string biaopos = context.Request["biaopos"];
            int result = ms.addMatchInfo(gameid, userid, score, juid, flag, beinum, realscore, lunnum, biaonum, biaopos);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "添加成功";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "添加失败";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 查询01系列游戏的得分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string select01GameScore(HttpContext context)
        {
            Json jsonResult = new Json();
            string gameid = context.Request["gameid"];
            string juid = context.Request["juid"];
            DataTable result = ms.select01GameScore(gameid, juid);
            if (result==null)
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

        private string selectOnce01GameScore(HttpContext context)
        {
            Json jsonResult = new Json();
            string gameid = context.Request["gameid"];
            string juid = context.Request["juid"];
            DataTable result = ms.selectOnce01GameScore(gameid, juid);
            if (result == null)
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

        private string selectOnceMiGameScore(HttpContext context)
        {
            Json jsonResult = new Json();
            string gameid = context.Request["gameid"];
            string juid = context.Request["juid"];
            DataTable result = ms.selectOnceMiGameScore(gameid, juid);
            if (result == null)
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

        private string selectBiaoNum(HttpContext context)
        {
            Json jsonResult = new Json();
            string gameid = context.Request["gameid"];
            string juid = context.Request["juid"];
            DataTable result = ms.selectBiaoNum(gameid, juid);
            if (result == null || result.Rows.Count == 0)
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

        /// <summary>
        /// 查询米老鼠系列游戏的得分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string selectMouseGameScore(HttpContext context)
        {
            Json jsonResult = new Json();
            string gameid = context.Request["gameid"];
            string juid = context.Request["juid"];
            string result = ms.selectMouseGameScore(gameid, juid);
            if (string.IsNullOrEmpty(result))
            {
                jsonResult.Success = false;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.Msg = result;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 获取用户已经选择的游戏
        /// </summary>
        /// <returns></returns>
        private string getChooseList(HttpContext context)
        {
            Json jsonResult = new Json();
            string gamenameid = context.Request["gamenameid"];
            string flag = context.Request["flag"];
            DataTable dt = ms.getChooseList(gamenameid, flag);
            if (dt==null)
            {
                jsonResult.Success = false;
                jsonResult.RowCount = dt.Rows.Count;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.Result = dt;
                jsonResult.RowCount = 0;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 修改游戏状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //private string updateGameResult(HttpContext context)
        //{
        //    Json jsonResult = new Json();
        //    string juid = context.Request["juid"];
        //    string state = context.Request["state"];
        //    int result = ms.updateGameState(juid, state);
        //    if (result > 0)
        //    {
        //        jsonResult.Success = true;
        //        jsonResult.Msg = "修改成功";
        //    }
        //    else
        //    {
        //        jsonResult.Success = false;
        //        jsonResult.Msg = "修改失败";
        //    }
        //    string strJson = JsonConvert.SerializeObject(jsonResult);
        //    return strJson;
        //}

        private string updateGameState(HttpContext context)
        {
            Json jsonResult = new Json();
            string winid = context.Request["winid"];
            string ortherid = context.Request["ortherid"];
            int result = ms.updateGameState(winid, ortherid);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "修改成功";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "修改失败";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string setSatrState(HttpContext context)
        {
            Json jsonResult = new Json();
            string allUserId = context.Request["allUserId"];
            int result = ms.setSatrState(allUserId);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "修改成功";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "修改失败";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 向数据库添加开始游戏记录的方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string starGame(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            //string gameid = context.Request["gameid"];
            //gameid = "1";
            int result = ms.starGame(id);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "添加成功";
            }
            else if(result == -2)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "不符合开局条件";
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

        /// <summary>
        /// 查询开局状态的接口方法
        /// </summary>
        /// <returns></returns>
        private string getStarInfo(HttpContext context)
        {
            Json jsonResult = new Json();
            string machineNum = context.Request["machineNum"];
            DataTable dt = ms.selectStarInfo(machineNum);
            if (dt == null)
            {
                jsonResult.Success = false;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.Result = dt;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 获取所有的设备
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getMachine(HttpContext context)
        {
            Json jsonResult = new Json();
            string address = context.Request["address"];
            string machineNum = context.Request["machineNum"];
            DataTable dt = ms.getMachine(machineNum, address);
            if (dt == null)
            {
                jsonResult.Success = false;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.Result = dt;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string userRegesit(HttpContext context)
        {
            Json jsonResult = new Json();
            string username = context.Request["username"];
            string nickname = context.Request["nickname"];
            string sex = context.Request["sex"];
            string phone = context.Request["phone"];
            string remark = context.Request["remark"];
            string email = context.Request["email"];
            int result = ms.insertUsers(username, nickname, phone, sex, remark, email);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "添加成功";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "添加失败";
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