using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ManagerServer
    {
        public DataTable dtType()
        {
            string sql = string.Format("select * from `type`");
            DataTable count = MySqlHelper.GetDataTable(CommandType.Text, sql);
            return count;
        }


        public DataTable dtTypebyid(string id)
        {
            string sql = string.Format("select * from `gamename` where gameType = " + id);
            DataTable count = MySqlHelper.GetDataTable(CommandType.Text, sql);
            return count;
        }

        public int addUserChooseGames(string gameid, string username, string time, string flag, string machine, string jieBiaoNum)
        {
            if (string.IsNullOrEmpty(jieBiaoNum))
            {
                jieBiaoNum = "0";
            }
            string selectSql = string.Format("SELECT COUNT(*) from userchoosegame choose INNER JOIN users on users.id=choose.userid  where users.username='{0}' and (choose.flag=0 or choose.flag=1)", username);
            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, selectSql));
            if (count != 0)
            {
                return -2;
            }

            string macStateSql = string.Format("SELECT IFNULL(state,1) state  FROM machine WHERE id='{0}'", machine);
            int countMac = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, macStateSql));
            if (countMac != 0)
            {
                return -3;
            }

            string sql = string.Format("INSERT into userchoosegame(gameid,userid,datetime,flag,machineid,jiebiaoNum)VALUES({0},(SELECT IFNULL(id,0) from users where username='{1}'),'{2}',{3},{4},{5})", gameid, username, time, flag, machine, jieBiaoNum);
            int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            return result;
        }

        public int addMatchInfo(string gameid, string userid, string score, string juid, string flag, string beinum, string realscore, string lunnum, string biaonum,string biaopos)
        {
            try
            {
                string sql = string.Format("INSERT INTO matchinfo(gameid,userid,score,flag,datetime,juid,beinum,realscore,lunNum,biaoNum,biaopos)VALUES({0},{1},{2},{3},'{4}',{5},{6},{7},{8},{9},{10})", gameid, userid, score, flag, DateTime.Now, juid, beinum, realscore, lunnum, biaonum, biaopos);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable select01GameScore(string gameid, string juid)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT IFNULL(SUM(score),0) score,userid FROM `matchInfo` where gameid={0} and juid={1} and flag=0 GROUP BY userid", gameid, juid);
                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public DataTable selectOnce01GameScore(string gameid, string juid)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT id,userid,score,beinum,realscore,lunNum,biaoNum FROM `matchInfo` where gameid={0} and juid={1} and flag=0 LIMIT 1", gameid, juid);
                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    string id = dt.Rows[0]["id"].ToString();
                    sql = string.Format("update matchinfo set flag=1 where id={0}", id);
                    int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public DataTable selectOnceMiGameScore(string gameid, string juid)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT id,userid,score,beinum,realscore,lunNum,biaoNum,biaopos FROM `matchInfo` where gameid={0} and juid={1} and flag=0 LIMIT 1", gameid, juid);
                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    string id = dt.Rows[0]["id"].ToString();
                    sql = string.Format("update matchinfo set flag=1 where id={0}", id);
                    int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public DataTable selectBiaoNum(string gameid, string juid)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT userid,biaoNum from matchinfo where gameid={0} and juid={1} ORDER BY id desc limit 1", gameid, juid);
                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public string selectMouseGameScore(string gameid, string juid)
        {
            try
            {
                string sql = string.Format("select group_concat(num) from (SELECT IFNULL(SUM(score),0) num FROM `matchInfo` where gameid={0} and juid={1} and flag=1 union ALL SELECT IFNULL(SUM(score),0) FROM `matchInfo` where gameid={0} and juid={1} and flag=2 union ALL SELECT IFNULL(SUM(score),0) FROM `matchInfo` where gameid={0} and juid={1} and flag=3 union ALL SELECT IFNULL(SUM(score),0) FROM `matchInfo` where gameid={0} and juid={1} and flag=4 union ALL SELECT IFNULL(SUM(score),0) FROM `matchInfo` where gameid={0} and juid={1} and flag=5 union ALL SELECT IFNULL(SUM(score),0) FROM `matchInfo` where gameid={0} and juid={1} and flag=6)  as aa", gameid, juid);
                object obj = MySqlHelper.ExecuteScalar(CommandType.Text, sql);
                if (obj != null && obj != DBNull.Value)
                {
                    return obj.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        //public int updateGameState(string juid,string state)
        //{
        //    try
        //    {
        //        string sql = string.Format("update gameresult set flag={0} where juid={1}", state, juid);
        //        int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
        //        return result;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }
        //}

        public int updateGameState(string winId, string ortherId, string orderno)
        {
            try
            {
                //if (string.IsNullOrEmpty(ortherId))
                //{
                //    string sql = string.Format("update userchoosegame set flag=2,gameResult=0 where userid={0} and flag=1 and orderno='{1}'", winId, orderno);
                //    int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                //    makeLog("updateGameState:" + sql);
                //    return result;
                //}
                //else
                //{
                //    string sql = string.Format("update userchoosegame set flag=2,gameResult=1 where userid in ({0}) and flag=1 and orderno='{2}';update userchoosegame set flag=2,gameResult=0 where userid={1} and flag=1 and orderno='{2}'", ortherId, winId, orderno);
                //    int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                //    makeLog("updateGameState:" + sql);
                //    return result;    
                //}

                if (!string.IsNullOrEmpty(ortherId))
                {
                    string sql = string.Format("update userchoosegame set flag=2,gameResult=1 where userid in ({0}) and flag=1 and orderno='{2}';", ortherId, winId, orderno);
                    makeLog("updateGameState:" + sql);
                    int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    
                    //return result;   
                }

                if (!string.IsNullOrEmpty(winId))
                {
                    string sql = string.Format("update userchoosegame set flag=2,gameResult=0 where userid={0} and flag=1 and orderno='{1}'", winId, orderno);
                    makeLog("updateGameState:" + sql);
                    int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                    
                    //return result;
                }

                return 1;
            }
            catch(Exception ex)
            {
                makeLog("updateGameState:" + ex.ToString());
                return -1;
            }
        }

        public int setSatrState(string userId)
        {
            try
            {
                string sql = string.Format("update userchoosegame set flag=1 where userid in ({0}) and flag=0;", userId);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable getChooseList(string gamenameid, string flag, string username, string page, string limit)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT cg.id,userid,gameName,cg.flag,users.username,users.nickname,date_format(cg.datetime, '%Y-%m-%d %H:%i:%s')time,mac.machineNum,mac.address,IFNULL(cg.gameResult,2) gameResult FROM `userchoosegame` cg inner JOIN gamename gn on cg.gameid=gn.id INNER JOIN users on cg.userid=users.id INNER JOIN machine mac on mac.id=cg.machineid where 1=1 ");
                if (!string.IsNullOrEmpty(gamenameid))
                {
                    sql += string.Format(" AND cg.gameid={0}", gamenameid);
                }

                if (!string.IsNullOrEmpty(flag))
                {
                    sql += string.Format(" AND cg.flag={0}", flag);
                }

                if (!string.IsNullOrEmpty(username))
                {
                    sql += string.Format(" AND users.username='{0}'", username);
                }

                sql += " ORDER BY cg.id DESC";

                int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                sql += string.Format(" limit {0},{1}", offset, limit);

                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                return dt;
            }
            catch
            {
                return dt;

            }
        }

        public int getChooseListNum(string gamenameid, string flag, string username)
        {
            try
            {
                string sql = string.Format("SELECT count(*) FROM `userchoosegame` cg inner JOIN gamename gn on cg.gameid=gn.id INNER JOIN users on cg.userid=users.id INNER JOIN machine mac on mac.id=cg.machineid where 1=1 ");
                if (!string.IsNullOrEmpty(gamenameid))
                {
                    sql += string.Format(" AND cg.gameid={0}", gamenameid);
                }

                if (!string.IsNullOrEmpty(flag))
                {
                    sql += string.Format(" AND cg.flag={0}", flag);
                }

                if (!string.IsNullOrEmpty(username))
                {
                    sql += string.Format(" AND users.username='{0}'", username);
                }

                sql += " ORDER BY cg.id DESC";

                //int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                //sql += string.Format(" limit {0},{1}", offset, limit);

                int result = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return result;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 开始游戏的方法
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int starGame(string ids,string openid)
        {
            try
            {
                //检查用户选择的游戏是否是同一种游戏，并且必须选择了自己
                //string selGameTypeSql = string.Format("SELECT gameid FROM `userchoosegame` WHERE id in({0}) and flag=0 GROUP BY gameid ", userIds);
                string selGameTypeSql = string.Format("SELECT gameid,gn.peopleNum num FROM `userchoosegame` game INNER JOIN users u on game.userid=u.id INNER JOIN gamename gn ON game.gameid=gn.id  WHERE game.id in({0}) and game.flag=0 and u.username='{1}' GROUP BY game.gameid ", ids, openid);
                DataTable dtType = MySqlHelper.GetDataTable(CommandType.Text, selGameTypeSql);
                if (dtType.Rows.Count != 1)
                {
                    return -2;
                }

                int chooseCount = Convert.ToInt32(dtType.Rows[0]["num"]);

                string sqlCount = string.Format("select count(*) from userchoosegame where id in({0})", ids);
                int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sqlCount));

                if (chooseCount != count)
                {
                    return -2;
                }

                //用户选择的机器只能在一台或两台机上
                string selectSql = string.Format("SELECT gameid FROM `userchoosegame` WHERE id in({0}) and flag=0 GROUP BY gameid,machineid ", ids);
                DataTable dt = MySqlHelper.GetDataTable(CommandType.Text, selectSql);
                if (dt.Rows.Count != 1 && dt.Rows.Count != 2)
                {
                    return -2;
                }
                
                //如果是联网类的游戏，必须是两台机
                string gameid = dt.Rows[0][0].ToString();
                if (gameid != "6" && gameid != "7" && gameid != "8" && gameid != "9")
                {
                    if (dt.Rows.Count == 2)
                    {
                        return -2;
                    }
                }
                string sql = string.Format("UPDATE userchoosegame set isadmin=0 where id in({2}) and userid=(SELECT id from users where username='{3}');update userchoosegame set flag=0 where id in({2});INSERT into gameresult(gameid,juid,flag,datetime) SELECT {0},MAX(juid)+1,0,'{1}' from gameresult", gameid, DateTime.Now, ids, openid);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int deleteChooseGame(string userIds)
        {
            try
            {
                if (string.IsNullOrEmpty(userIds))
                {
                    return -2;
                }
                string sql = string.Format("DELETE from userchoosegame where flag=0 and id in({0})", userIds);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable selectStarInfo(string machineNum)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT gameid,IFNULL(jiebiaoNum,0) jiebiao,playTimes,orderno from userchoosegame choose INNER JOIN machine ma on choose.machineid=ma.id where choose.flag=1 and ma.machineNum='{0}' and payState=0 LIMIT 1", machineNum);
                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);

                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public DataTable getMachine(string macNum,string address,string limit,string page)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT id,machineNum,address,date_format(createtime, '%Y-%m-%d %H:%i:%s')time,state,description,biaoNum from machine where 1=1 ");
                if (!string.IsNullOrEmpty(macNum))
                {
                    sql += string.Format(" machineNum='{0}'", macNum);
                }

                if (!string.IsNullOrEmpty(address))
                {
                    sql += string.Format(" and address like '%{0}%'", macNum);
                }

                if (!string.IsNullOrEmpty(limit) && !string.IsNullOrEmpty(page))
                {
                    int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                    sql += string.Format(" limit {0},{1}", offset, limit);
                }

                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public int getMachineNum(string macNum, string address)
        {
            try
            {
                string sql = string.Format("SELECT count(*) from machine where 1=1 ");
                if (!string.IsNullOrEmpty(macNum))
                {
                    sql += string.Format(" machineNum='{0}'", macNum);
                }

                if (!string.IsNullOrEmpty(address))
                {
                    sql += string.Format(" and address like '%{0}%'", macNum);
                }

                int result = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public int insertUsers(string username,string nickname,string phone,string sex,string remark,string email)
        {
            try
            {
                string sql = string.Format("INSERT INTO users(username,nickname,phoneNum,sex,remark,email,datetime)VALUES('{0}','{1}','{2}',{3},'{4}','{5}','{6}')", username, nickname, phone, sex, remark, email, DateTime.Now);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable getUserList(string id, string username, string limit, string page)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT id,username,date_format(datetime, '%Y-%m-%d %H:%i:%s')time,flag FROM `users` where 1=1 ");
                if (!string.IsNullOrEmpty(id))
                {
                    sql += string.Format(" and id={0}", id);
                }
                if (!string.IsNullOrEmpty(username))
                {
                    sql += string.Format(" and username='{0}'", username);
                }

                int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                sql += string.Format(" limit {0},{1}", offset, limit);

                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public int getUserListNum(string id, string username)
        {
            try
            {
                string sql = string.Format("SELECT count(*) FROM `users` where 1=1 ");
                if (!string.IsNullOrEmpty(id))
                {
                    sql += string.Format(" and id={0}", id);
                }
                if (!string.IsNullOrEmpty(username))
                {
                    sql += string.Format(" and username='{0}'", username);
                }

                int result = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public DataTable getGameNameList(string gameName, string typeName, string limit,string page)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT gamename.id,gameName,typeName from gamename INNER JOIN type on gamename.gameType=type.ID where 1=1 ");
                if (!string.IsNullOrEmpty(gameName))
                {
                    sql += string.Format(" and gameName='{0}'", gameName);
                }
                if (!string.IsNullOrEmpty(typeName))
                {
                    sql += string.Format(" and typeName='{0}'", typeName);
                }

                int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                sql += string.Format(" limit {0},{1}", offset, limit);

                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public int getGameNameListNum(string gameName, string typeName)
        {
            try
            {
                string sql = string.Format("SELECT count(*) from gamename INNER JOIN type on gamename.gameType=type.ID where 1=1 ");
                if (!string.IsNullOrEmpty(gameName))
                {
                    sql += string.Format(" and gameName='{0}'", gameName);
                }
                if (!string.IsNullOrEmpty(typeName))
                {
                    sql += string.Format(" and typeName='{0}'", typeName);
                }

                int result = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public DataTable getGameTypeList(string typeName,string limit,string page)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format("SELECT * FROM type where 1=1 ");
                if (!string.IsNullOrEmpty(typeName))
                {
                    sql += string.Format(" and typeName='{0}'", typeName);
                }

                int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
                sql += string.Format(" limit {0},{1}", offset, limit);

                dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                return dt;
            }
            catch
            {
                return dt;
            }
        }

        public int getGameTypeListNum(string typeName)
        {
            try
            {
                string sql = string.Format("SELECT count(*) FROM type where 1=1 ");
                if (!string.IsNullOrEmpty(typeName))
                {
                    sql += string.Format(" and typeName='{0}'", typeName);
                }

                int result = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public int deleteGameType(string id)
        {
            try
            {
                string sql = string.Format("DELETE FROM type where ID in({0})", id);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int deleteGameName(string id)
        {
            try
            {
                string sql = string.Format("DELETE FROM gamename where id in ({0})", id);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int deleteMac(string id)
        {
            try
            {
                string sql = string.Format("DELETE FROM machine where id in ({0})", id);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backBiaoNum"></param>
        /// <param name="payno"></param>
        /// <returns></returns>
        public refundInfo getRefundInfo(string backBiaoNum,string payno)
        {
            try
            {
                //在这个地方处理退款
                string selectNo = string.Format("SELECT borrowBiaoNum,choose.orderno,result.price from userchoosegame choose INNER JOIN pay_result result on result.order_no=choose.orderno where choose.orderno='{0}' GROUP BY choose.orderno", payno);
                makeLog("getRefundInfo:" + selectNo);
                DataTable dt = MySqlHelper.GetDataTable(CommandType.Text, selectNo);
                string orderno = "";
                string totalPrice = "";
                int borrowNum = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    orderno = dt.Rows[0]["orderno"].ToString();
                    totalPrice = dt.Rows[0]["price"].ToString();
                    if (dt.Rows[0]["borrowBiaoNum"] != DBNull.Value)
                    {
                        borrowNum = Convert.ToInt32(dt.Rows[0]["borrowBiaoNum"]);
                    }
                }
                if (borrowNum == 0)
                {
                    borrowNum = Convert.ToInt32(backBiaoNum);
                }
                if (borrowNum == 0)
                {
                    borrowNum = 1;
                    backBiaoNum = "1";
                }
                //计算退款的金额
                DataTable dtMoney = getDeposit();
                int sinlgeBiaoMoney = Convert.ToInt32(dtMoney.Rows[0]["biaoMoney"]);
                //int money = sinlgeBiaoMoney * (borrowNum - Convert.ToInt32(backBiaoNum));      //押金按比例退
                //if (money > sinlgeBiaoMoney * borrowNum)
                //{
                //    money = sinlgeBiaoMoney * borrowNum;
                //}

                int money = sinlgeBiaoMoney * (Convert.ToInt32(backBiaoNum));      //押金按比例退
                if (money > sinlgeBiaoMoney * borrowNum)
                {
                    money = sinlgeBiaoMoney * borrowNum;
                }
                money = (money > 0) ? money : 0;
                //money = deposit * Convert.ToInt32(backBiaoNum) / borrowNum;
                makeLog("订单号：" + orderno + "总额：" + totalPrice + "还了：" + backBiaoNum + "借了：" + borrowNum + "退还金额：" + money);
                if (!string.IsNullOrEmpty(orderno))
                {
                    refundInfo info = new refundInfo();
                    info.orderno = orderno;
                    info.totalPrice = totalPrice;
                    info.refundPrice = money.ToString();
                    return info;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                makeLog("getRefundInfo()异常：" + ex.ToString());
                return null;
            }
        }

        public int setHuanBiaoState(string orderno, string state,string biaoNum,string macNum)
        {
            try
            {
                if (string.IsNullOrEmpty(biaoNum) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(orderno))
                {
                    return -2;
                }

                string sql = string.Format("UPDATE userchoosegame set biaoState={0},backBiaoNum={2} where flag=2 and orderno='{1}';", state, orderno, biaoNum);
                sql += string.Format("UPDATE machine set biaoNum={0} where machineNum='{1}'", biaoNum, macNum);
                makeLog("setHuanBiaoState:" + sql);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int addGameName(string typeId, string gameName)
        {
            try
            {
                string sql = string.Format("INSERT INTO gamename (gamename,gameType)VALUES('{0}',{1})", gameName, typeId);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int addGameType(string typeName)
        {
            try
            {
                string sql = string.Format("INSERT into type (typeName)VALUES('{0}')", typeName);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int addMac(string machineNum, string address)
        {
            try
            {
                string sql = string.Format("INSERT INTO machine(machineNum,address,createtime)VALUES('{0}','{1}','{2}')", machineNum,address,DateTime.Now);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int checkUser(string username, string nickname)
        {
            string sql = "";
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    username = "";
                }

                if (string.IsNullOrEmpty(nickname))
                {
                    nickname = "";
                }

                sql = string.Format("SELECT count(*) FROM `users` where username='{0}'", username);
                int result = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));

                if (result == 0)
                {
                    sql = string.Format("INSERT INTO users(username,datetime,nickname,flag)VALUES('{0}','{1}','{2}',0)", username, DateTime.Now.ToString(), nickname);
                    result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                }
                return result;
            }
            catch(Exception ex)
            {
                makeLog(ex.ToString() + sql);
                return -1;
            }
        }

        public int makeLog(string info)
        {
            try
            {
                string sql = string.Format("INSERT into log(info,datetime)VALUES('{0}','{1}')", info, DateTime.Now);
                int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);

                return result;
            }
            catch
            {
                return -1;
            }
        }

        public int addNotify(string openid,string price,string orderno)
        {
            string sql = string.Format("insert into pay_result(username,datetime,price,order_no)VALUES('{0}','{1}',{2},'{3}')", openid, DateTime.Now, price, orderno);
            int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            return aa;
        }

        public int selectPayCount(string orderno)
        {
            string sql = string.Format("SELECT count(*) FROM `pay_result` where order_no='{0}'", orderno);
            int aa = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
            return aa;
        }

        public int selectRefundCount(string orderno)
        {
            string sql = string.Format("SELECT count(*) FROM `refund_result` where refund_id='{0}'", orderno);
            int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            return aa;
        }

        public int addRefund(string tradeno, string refundno, string refundid,string refundFee)
        {
            string sql = string.Format("INSERT into refund_result (out_trade_no,out_refund_no,refund_id,refund_fee,datetime)VALUES('{0}','{1}','{2}',{3},'{4}')", tradeno, refundno, refundid, refundFee, DateTime.Now);
            int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            return aa;
        }

        public int addgameSetting(string biaoMoney, string gameMoney)
        {
            string sql = string.Format("INSERT into game_setting(biaoMoney,gameMoney)VALUES({0},{1})", biaoMoney, gameMoney); 
            int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            return aa;
        }

        public DataTable getDeposit()
        {
            string sql = string.Format("SELECT biaoMoney,gameMoney from game_setting ORDER BY id desc LIMIT 1");
            DataTable dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public int getMacBiao(string macNum)
        {
            try
            {
                string selBiaoNumSql = string.Format("SELECT biaoNum from machine where machineNum='{0}'", macNum);
                int biaoNum = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, selBiaoNumSql));
                return biaoNum;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改付款状态和支付状态，必须是没有开局的状态才能修改，否则不能修改
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="quantity"></param>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public int updatePayState(string ids, string quantity,string orderno)
        {
            string sql = string.Format("UPDATE userchoosegame set flag=1,payState=0,playTimes='{0}',orderno='{2}' where flag=0 and id in({1})", quantity, ids, orderno);
            int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            return aa;
        }

        public int selectPayFee(string orderno)
        {
            try
            {
                string sql = string.Format("SELECT IFNULL(price,0)price from pay_result where order_no='{0}' LIMIT 1", orderno);
                int aa = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return aa;
            }
            catch
            {
                return 0;
            }
        }

        public DataTable getUserRefundState(string username)
        {
            string sql = string.Format("SELECT IFNULL(payState,-1) payState,IFNULL(refundState,-1) refundState,choose.flag,IFNULL(backBiaoNum,0)backBiaoNum,IFNULL(biaoState,0)biaoState from userchoosegame choose INNER JOIN users u on choose.userid=u.id  where (IFNULL(payState,-1)<>0 or IFNULL(refundState,-1)<>0 ) and username='{0}' ORDER BY choose.id desc", username);
            DataTable dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public int updateRefundState(string orderno)
        {
            try
            {
                string sql = string.Format("update userchoosegame set refundState=0 and flag=2 where orderno='{0}'", orderno);
                makeLog("updateRefundState:" + sql);
                int aa = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public int updateBorrowBiaoNum(string orderno, string macNum)
        {
            try
            {
                string sql = string.Format("UPDATE userchoosegame set borrowBiaoNum=(SELECT biaoNum from machine where machineNum='{0}') WHERE orderno='{1}'",macNum, orderno);
                int aa = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public int updateGameStateByOrderno(string orderno)
        {
            try
            {
                string sql = string.Format("UPDATE userchoosegame set flag=2 WHERE orderno='{0}'", orderno);
                int aa = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public int updateMacState(string state,string macNum,string info)
        {
            try
            {
                string sql = string.Format("update machine set state='{0}',description='{1}' where machineNum='{2}'", state, info, macNum);
                int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable getGameInfo(string orderno)
        {
            try
            {
                string sql = string.Format("SELECT borrowBiaoNum from userchoosegame where orderno='{0}'", orderno);
                DataTable dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helper.makeLog(ex.ToString());
                return null;
            }
            
        }

        public int updateMacStateById(string state, string macNum, string info, string id,string address,string biaoNum)
        {
            try
            {
                string sql = string.Format("update machine set state='{0}',description='{1}',machineNum='{2}',address='{3}',biaoNum='{4}' where id ={5}", state, info, macNum, address, biaoNum, id);
                int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public int insertDebugInfo(string id,string command)
        {
            try
            {
                string sql = string.Format("INSERT INTO debug_command(command,flag,macid,datetime)VALUES('{0}',0,{1},'{2}')", command, id, DateTime.Now);
                int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable getDebugInfo(string macNum)
        {
            string sql = string.Format("SELECT de.command,de.id from debug_command de INNER JOIN machine mac on de.macid=mac.id where de.flag=0 and mac.machineNum='{0}'", macNum);
            DataTable dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public int updateDebugInfoState(string id)
        {
            try
            {
                string sql = string.Format("UPDATE debug_command set flag=1 where id={0}", id);
                int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public int toEndGame(string username)
        {
            try
            {
                //string sql = string.Format("UPDATE userchoosegame set flag=2 where orderno='{0}'", orderno);
                string sql = string.Format("UPDATE userchoosegame set flag=2 where userid=(SELECT id from users where username='{0}') and flag=1", username);
                int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public string getGameState(string orderno)
        {
            try
            {
                string sql = string.Format("SELECT flag FROM `userchoosegame` where orderno='{0}';", orderno);
                string aa = MySqlHelper.ExecuteScalar(CommandType.Text, sql).ToString();
                return aa;
            }
            catch
            {
                return "0";
            }
        }

        public int adminDeleteChooseGame(string id)
        {
            try
            {
                string sql = string.Format("DELETE from userchoosegame WHERE id in({0})", id);
                int aa = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
                return aa;
            }
            catch
            {
                return -1;
            }
        }

        public DataTable getPayList(string nickname, string limit, string page)
        {
            string sql = string.Format("SELECT pay.id,date_format(pay.datetime, '%Y-%m-%d %H:%i:%s')time,pay.price,choose.orderno,u.nickname,gn.gameName,IFNULL(choose.refundState,1)refundState from pay_result pay INNER JOIN users u on pay.username=u.username INNER JOIN userchoosegame choose on pay.order_no=choose.orderno INNER JOIN gamename gn on choose.gameid=gn.id GROUP BY pay.order_no");

            if (!string.IsNullOrEmpty(nickname))
            {
                sql += string.Format(" and u.nickname='{0}'", nickname);
            }

            int offset = (Convert.ToInt32(page) - 1) * Convert.ToInt32(limit);
            sql += string.Format(" limit {0},{1}", offset, limit);

            DataTable dt = MySqlHelper.GetDataTable(CommandType.Text, sql);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public int getPayListNum(string nickname)
        {
            string sql = string.Format("SELECT count(*) from(SELECT count(*) from pay_result pay INNER JOIN users u on pay.username=u.username INNER JOIN userchoosegame choose on pay.order_no=choose.orderno INNER JOIN gamename gn on choose.gameid=gn.id GROUP BY pay.order_no) as aa where 1=1");

            if (!string.IsNullOrEmpty(nickname))
            {
                sql += string.Format(" and aa.nickname='{0}'", nickname);
            }

            int result = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
            return result;
        }

        public string getMacNum(string id)
        {
            string selMacNumSql = string.Format("SELECT machineNum from userchoosegame choose INNER JOIN machine on choose.machineid=machine.id  where choose.id in ({0}) and choose.flag=0 LIMIT 1", id); 
            string macNum = MySqlHelper.ExecuteScalar(CommandType.Text, selMacNumSql).ToString();
            return macNum;
        }

        public int updateMacBiaoNum(string macNum,string biaoNum)
        {
            string sql = string.Format("UPDATE machine set biaoNum={0} where machineNum='{1}'", biaoNum, macNum);
            int result = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            return result;
        }
    }
}
