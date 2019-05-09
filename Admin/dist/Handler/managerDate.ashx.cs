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
                    result = addGame(context);
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
                case "deleteChooseGame":
                    result = deleteChooseGame(context);
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
                case "getUserList":
                    result = getUserList(context);
                    break;
                case "getGameNameList":
                    result = getGameNameList(context);
                    break;
                case "getGameTpyeList":
                    result = getGameTpyeList(context);
                    break;
                case "deleteGameType":
                    result = deleteGameType(context);
                    break;
                case "deleteGameName":
                    result = deleteGameName(context);
                    break;
                case "deleteMac":
                    result = deleteMac(context);
                    break;
                case "setHuanBiaoState":
                    result = setHuanBiaoState(context);
                    break;
                case "addGameName":
                    result = addGameName(context);
                    break;
                case "addGameType":
                    result = addGameType(context);
                    break;
                case "addMac":
                    result = addMac(context);
                    break;
                case "addGameSetting":
                    result = addGameSetting(context);
                    break;
                case "getUserRefundState":
                    result = getUserRefundState(context);
                    break;
                case "updateBorrowBiaoNum":
                    result = updateBorrowBiaoNum(context);
                    break;
                case "getDeposit":
                    result = getDeposit(context);
                    break;
                case "updateGameStateByOrderno":
                    result = updateGameStateByOrderno(context);
                    break;
                case "updateMacState":
                    result = updateMacState(context);
                    break;
                case "getGameInfo":
                    result = getGameInfo(context);
                    break;
                case "toEndGame":
                    result = toEndGame(context);
                    break;
                case "adminDelGame":
                    result = adminDelGame(context);
                    break;
                case "getPayList":
                    result = getPayList(context);
                    break;
                case "updateMacBiaoNum":
                    result = updateMacBiaoNum(context);
                    break;
                case "getGameState":
                    result = getGameState(context);
                    break;
                default:
                    result = "没有接收到任何对应参数";
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
                jsonResult.PageCount = 0;
                jsonResult.RowCount = list.Rows.Count;
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
            string username = context.Request["username"];      //这个变量获取openid
            string machineid = context.Request["machineids"];
            string jieBiaoNum = context.Request["jieBiaoNum"];
            string nickname = context.Request["nickname"];      //这个变量获取微信昵称
            Json jsonResult = new Json();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname))
            {
                jsonResult.Success = false;
                jsonResult.Msg = "用户昵称或id不能为空";

                string json = JsonConvert.SerializeObject(jsonResult);
                return json;
            }

            //首先判断是否有这个用户，如果没有，则创建一个
            int count = ms.checkUser(username, nickname);
            if (count <= 0)
            {
                //jsonResult.Success = false;
                //jsonResult.Msg = "有异常";
                //return JsonConvert.SerializeObject(jsonResult);
            }

            
            string time = DateTime.Now.ToString();
            int result = ms.addUserChooseGames(gameid, username, time, "0", machineid,jieBiaoNum);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "添加成功";
            }
            else if (result == -2)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "当前还有游戏没有完成，不能添加";
            }
            else if (result == -3)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "设备异常，请等待维护";
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
            if (string.IsNullOrEmpty(biaopos))
            {
                biaopos = "0";
            }
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
            string username = context.Request["username"];
            string limit = context.Request["limit"];
            string page = context.Request["page"];
            DataTable dt = ms.getChooseList(gamenameid, flag, username, page, limit);
            int count = ms.getChooseListNum(gamenameid, flag, username);
            if (dt == null || dt.Rows.Count == 0)
            {
                jsonResult.Success = false;
                jsonResult.count = count;
                jsonResult.code = 400;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.data = dt;
                jsonResult.count = count;
                jsonResult.code = 0;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string updateGameState(HttpContext context)
        {
            Json jsonResult = new Json();
            string winid = context.Request["winid"];
            string ortherid = context.Request["ortherid"];
            string orderno = context.Request["orderno"];
            int result = ms.updateGameState(winid, ortherid, orderno);
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

        private string setHuanBiaoState(HttpContext context)
        {
            Json jsonResult = new Json();
            string state = context.Request["state"];
            string orderno = context.Request["orderno"];
            string biaoNum = context.Request["biaoNum"];
            string macNum = context.Request["macNum"];
            int result = ms.setHuanBiaoState(orderno, state, biaoNum, macNum);
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
            string data = context.Request["data"];
            string openid = context.Request["username"];

            if (!string.IsNullOrEmpty(data))
            {
                id = "";
                JArray ja = (JArray)JsonConvert.DeserializeObject(data);
                foreach (JObject objid in ja)
                {
                    id += objid["id"] + ","; 
                }
                id = id.Substring(0, id.Length - 1);
            }

            int result = ms.starGame(id, openid);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "操作成功";
            }
            else if (result == -2)
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

        private string deleteChooseGame(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            string data = context.Request["data"];

            if (!string.IsNullOrEmpty(data))
            {
                id = "";
                JArray ja = (JArray)JsonConvert.DeserializeObject(data);
                foreach (JObject objid in ja)
                {
                    id += objid["id"] + ",";
                }
                id = id.Substring(0, id.Length - 1);
            }

            int result = ms.deleteChooseGame(id);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "操作成功";
            }
            else if (result == -2)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "id为空";
            }
            else if (result == -1)
            {
                jsonResult.Success = false;
                jsonResult.Msg = "发生异常";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "只能删除未开局的记录";
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
            if (dt == null || dt.Rows.Count == 0)
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
            string limit = context.Request["limit"];
            string page = context.Request["page"];
            DataTable dt = ms.getMachine(machineNum, address, limit, page);
            int count = ms.getMachineNum(machineNum, address);

            if (dt == null)
            {
                jsonResult.Success = false;
                jsonResult.count = count;
                jsonResult.code = 400;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.data = dt;
                jsonResult.count = count;
                jsonResult.code = 0;
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

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getUserList(HttpContext context)
        {
            Json jsonResult = new Json();
            string username = context.Request["username"];
            string id = context.Request["id"];
            string limit = context.Request["limit"];
            string page = context.Request["page"];
            DataTable dt = ms.getUserList(id, username, limit, page);
            int count = ms.getUserListNum(id, username);
            if (dt == null)
            {
                jsonResult.Success = false;
                jsonResult.count = count;
                jsonResult.code = 400;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.data = dt;
                jsonResult.count = count;
                jsonResult.code = 0;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        /// <summary>
        /// 获取游戏名列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getGameNameList(HttpContext context)
        {
            Json jsonResult = new Json();
            string gameName = context.Request["gameName"];
            string typeName = context.Request["typeName"];
            string limit = context.Request["limit"];
            string page = context.Request["page"];
            DataTable dt = ms.getGameNameList(gameName, typeName, limit, page);
            int count = ms.getGameNameListNum(gameName, typeName);
            if (dt == null)
            {
                jsonResult.Success = false;
                jsonResult.count = dt.Rows.Count;
                jsonResult.code = 400;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.data = dt;
                jsonResult.count = count;
                jsonResult.code = 0;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }


        private string getGameTpyeList(HttpContext context)
        {
            Json jsonResult = new Json();
            string typeName = context.Request["typeName"];
            string limit = context.Request["limit"];
            string page = context.Request["page"];
            DataTable dt = ms.getGameTypeList(typeName, limit, page);
            int count = ms.getGameTypeListNum(typeName);
            if (dt == null)
            {
                jsonResult.Success = false;
                jsonResult.count = count;
                jsonResult.code = 400;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.data = dt;
                jsonResult.count = count;
                jsonResult.code = 0;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string deleteGameType(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            string data = context.Request["data"];

            if (!string.IsNullOrEmpty(data))
            {
                id = "";
                JArray ja = (JArray)JsonConvert.DeserializeObject(data);
                foreach (JObject objid in ja)
                {
                    id += objid["ID"] + ",";
                }
                id = id.Substring(0, id.Length - 1);
            }

            int result = ms.deleteGameType(id);
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

        private string deleteGameName(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            string data = context.Request["data"];

            if (!string.IsNullOrEmpty(data))
            {
                id = "";
                JArray ja = (JArray)JsonConvert.DeserializeObject(data);
                foreach (JObject objid in ja)
                {
                    id += objid["id"] + ",";
                }
                id = id.Substring(0, id.Length - 1);
            }

            int result = ms.deleteGameName(id);
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

        private string deleteMac(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            string data = context.Request["data"];

            if (!string.IsNullOrEmpty(data))
            {
                id = "";
                JArray ja = (JArray)JsonConvert.DeserializeObject(data);
                foreach (JObject objid in ja)
                {
                    id += objid["id"] + ",";
                }
                id = id.Substring(0, id.Length - 1);
            }

            int result = ms.deleteMac(id);
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

        private string addGameName(HttpContext context)
        {
            Json jsonResult = new Json();
            string typeid = context.Request["GoodsTypeIdss"];
            string gamename = context.Request["gamename"];
            int result = ms.addGameName(typeid, gamename);
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

        private string addGameType(HttpContext context)
        {
            Json jsonResult = new Json();
            string gamename = context.Request["gametypename"];
            int result = ms.addGameType(gamename);
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
        /// 添加设备
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addMac(HttpContext context)
        {
            Json jsonResult = new Json();
            string macNum = context.Request["macNum"];
            string macAddress = context.Request["macAddress"];
            int result = ms.addMac(macNum, macAddress);
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
        /// 添加系统设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addGameSetting(HttpContext context)
        {
            Json jsonResult = new Json();
            string biaoMoney = context.Request["biaoMoney"];
            string gameMoney = context.Request["gameMoney"];
            int result = ms.addgameSetting(biaoMoney, gameMoney);
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

        private string getUserRefundState(HttpContext context)
        {
            userState jsonResult = new userState();
            string openid = context.Request["openid"];
            DataTable dt = ms.getUserRefundState(openid);
            if (dt != null && dt.Rows.Count > 0)
            {
                jsonResult.Success = true;
                //jsonResult.backBiaoNum = dt.Rows[0]["backBiaoNum"].ToString();
                //jsonResult.flag = dt.Rows[0]["flag"].ToString() == "2" ? "已结束" : "未结束";
                //jsonResult.payState = dt.Rows[0]["payState"].ToString() == "0" ? "已支付" : "未支付";
                //jsonResult.refundState = dt.Rows[0]["refundState"].ToString() == "0" ? "已退" : "未退";
                //jsonResult.biaoState = dt.Rows[0]["biaoState"].ToString() == "0" ? "未还" : "已还";

                jsonResult.flag = dt.Rows[0]["flag"].ToString();
                jsonResult.payState = dt.Rows[0]["payState"].ToString();
                jsonResult.refundState = dt.Rows[0]["refundState"].ToString();
                jsonResult.biaoState = dt.Rows[0]["biaoState"].ToString();
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.flag = "2";
                jsonResult.payState = "0";
                jsonResult.refundState = "0";
                jsonResult.biaoState = "1";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string updateBorrowBiaoNum(HttpContext context)
        {
            Json jsonResult = new Json();
            string orderno = context.Request["orderno"];
            string macNum = context.Request["macNum"];
            int result = ms.updateBorrowBiaoNum(orderno, macNum);
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
        /// 返回给微信的押金和游戏金额的接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getDeposit(HttpContext context)
        {
            string macNum = context.Request["macNum"];
            if (string.IsNullOrEmpty(macNum))
            {
                Deposit jsonResult = new Deposit();
                DataTable dt = ms.getDeposit();
                if (dt != null && dt.Rows.Count > 0)
                {
                    double biaoMoney = Convert.ToDouble(dt.Rows[0]["biaoMoney"]) / 100;
                    double gameMoney = Convert.ToDouble(dt.Rows[0]["gameMoney"]) / 100;
                    jsonResult.Success = true;
                    jsonResult.GameMoney = gameMoney.ToString();
                    jsonResult.DepositMoney = biaoMoney.ToString();
                    jsonResult.TotalMoney = (gameMoney + biaoMoney).ToString();
                }
                else
                {
                    jsonResult.Success = false;
                }
                string strJson = JsonConvert.SerializeObject(jsonResult);
                return strJson;
            }
            else
            {
                Deposit jsonResult = new Deposit();
                DataTable dt = ms.getDeposit();
                int biaoNum = ms.getMacBiao(macNum);
                biaoNum = biaoNum > 0 ? biaoNum : 1;
                if (dt != null && dt.Rows.Count > 0)
                {
                    double gameMoney = Convert.ToDouble(dt.Rows[0]["gameMoney"]) / 100;
                    double depositMoney = Convert.ToDouble(dt.Rows[0]["biaoMoney"]) / 100 * biaoNum;
                    jsonResult.Success = true;
                    jsonResult.GameMoney = gameMoney.ToString();            //游戏费用
                    jsonResult.DepositMoney = depositMoney.ToString();      //押金
                    jsonResult.TotalMoney = (gameMoney + depositMoney).ToString();
                }
                else
                {
                    jsonResult.Success = false;
                }
                string strJson = JsonConvert.SerializeObject(jsonResult);
                return strJson;
            }
        }

        private string updateGameStateByOrderno(HttpContext context)
        {
            string orderno = context.Request["orderno"];
            int result = ms.updateGameStateByOrderno(orderno);
            return result.ToString();
        }

        private string updateMacState(HttpContext context)
        {
            string state = context.Request["state"];
            string macNum = context.Request["macNum"];
            string info = context.Request["info"];
            int result = ms.updateMacState(state, macNum, info);
            return result.ToString();
        }

        private string getGameInfo(HttpContext context)
        {
            Json jsonResult = new Json();
            string orderno = context.Request["orderno"];
            DataTable dt = ms.getGameInfo(orderno);
            if (dt != null && dt.Rows.Count > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = dt.Rows[0]["borrowBiaoNum"].ToString();
            }
            else
            {
                jsonResult.Success = true;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string toEndGame(HttpContext context)
        {
            Json jsonResult = new Json();
            string username = context.Request["username"];
            int result = ms.toEndGame(username);
            if (result > 0)
            {
                jsonResult.Success = true;
            }
            else
            {
                jsonResult.Success = false;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string getGameState(HttpContext context)
        {
            string orderno = context.Request["orderno"];
            string result = ms.getGameState(orderno);
            return result;
        }

        private string adminDelGame(HttpContext context)
        {
            Json jsonResult = new Json();
            string id = context.Request["id"];
            string data = context.Request["data"];
            if (!string.IsNullOrEmpty(data))
            {
                id = "";
                JArray ja = (JArray)JsonConvert.DeserializeObject(data);
                foreach (JObject objid in ja)
                {
                    id += objid["id"] + ",";
                }
                id = id.Substring(0, id.Length - 1);
            }
            int result = ms.adminDeleteChooseGame(id);
            if (result > 0)
            {
                jsonResult.Success = true;
                jsonResult.Msg = "操作成功";
            }
            else
            {
                jsonResult.Success = false;
                jsonResult.Msg = "操作失败";
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string getPayList(HttpContext context)
        {
            Json jsonResult = new Json();
            string nickname = context.Request["nickname"];
            string limit = context.Request["limit"];
            string page = context.Request["page"];
            DataTable dt = ms.getPayList(nickname, limit, page);
            int count = ms.getPayListNum(nickname);
            if (dt == null)
            {
                jsonResult.Success = false;
                jsonResult.count = count;
                jsonResult.code = 400;
            }
            else
            {
                jsonResult.Success = true;
                jsonResult.data = dt;
                jsonResult.count = count;
                jsonResult.code = 0;
            }
            string strJson = JsonConvert.SerializeObject(jsonResult);
            return strJson;
        }

        private string updateMacBiaoNum(HttpContext context)
        {
            string macNum = context.Request["macNum"];
            string biaoNum = context.Request["biaoNum"];
            int result = ms.updateMacBiaoNum(macNum, biaoNum);
            return result.ToString();
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