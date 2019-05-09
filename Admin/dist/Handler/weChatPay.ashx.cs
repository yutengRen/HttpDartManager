using DAL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Data;
using System.Configuration;
using System.Xml;

namespace Admin.dist.Handler
{
    /// <summary>
    /// weChatPay 的摘要说明
    /// </summary>
    public class weChatPay : IHttpHandler
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
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
                case "pay":
                    result = pay(context);
                    break;
                case "getOpenid":
                    result = getOpenid(context);
                    break;
                case "refund":
                    result = refund(context);
                    break;
                case "testRefund":
                    result = testRefund(context);
                    break;
                case "adminRefund":
                    result = adminRefund(context);
                    break;
                default:
                    result = "没有内容";
                    break;
            }

            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();
        }

        private string getOpenid(HttpContext context)
        {
            try
            {
                string code = context.Request["code"];

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("appid", "wxd6cf371dbe9f5906");
                dic.Add("secret", "564716aeabb480c0162725dfac0f4196");
                dic.Add("js_code", code);
                dic.Add("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/jscode2session";
                string json = Helper.Post(url, dic);
                if (string.IsNullOrEmpty(json))
                {
                    return "没有请求到内容";
                }
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                if (jo["openid"] == null)
                {
                    return "获取不到openid,异常信息：" + json;
                }
                string openid = jo["openid"].ToString();
                return openid;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }

        private string pay(HttpContext context)
        {
            try
            {
                string openid = context.Request["openid"];
                string ids = context.Request["ids"];
                string quantity = context.Request["quantity"];

                string macNum = ms.getMacNum(ids);
                int biaoNum = ms.getMacBiao(macNum);

                if (string.IsNullOrEmpty(ids) || string.IsNullOrEmpty(quantity))
                {
                    return "必要参数为空";
                }

                string json = "{ids:\"" + ids + "\",quantity:\"" + quantity + "\"}";

                int price = 1;
                
                DataTable dt = ms.getDeposit();
                if (dt != null)
                {
                    price = Convert.ToInt32(dt.Rows[0]["biaoMoney"]) * biaoNum + Convert.ToInt32(dt.Rows[0]["gameMoney"]) * Convert.ToInt32(quantity);
                }

                ms.makeLog("pay()方法：biaoNum," + biaoNum + "macNum," + macNum);

                Random rand = new Random();
                int num = rand.Next(1000, 9999);
                Parameteres param = new Parameteres();
                param.Total_fee = price.ToString();     //总金额有问题
                string orderno = DateTime.Now.ToString("yyyyMMddHHmmss") + num;
                param.Out_trade_no = orderno;
                param.Attach = json;
                string canshu = GetUnifiedOrderParam(openid, param);
                string payResXML = Helper.ordinaryPost("https://api.mch.weixin.qq.com/pay/unifiedorder", canshu);
                var payRes = XDocument.Parse(payResXML);
                var root = payRes.Element("xml");

                //序列化相应参数返回给小程序
                var res = GetPayRequestParam(root, param.Appid, param.Key);
                string strJson = JsonConvert.SerializeObject(res);
                return strJson;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }

        private string refund(HttpContext context)
        {
            try
            {
                //string alluserid = context.Request["alluserid"];
                string biaoNum = context.Request["biaoNum"];
                string payNo = context.Request["orderno"];
                ms.makeLog("进来了退款的方法:" + "，还镖数量：" + biaoNum + "，付款单号：" + payNo);

                //注意这里，这里有异常
                refundInfo info = ms.getRefundInfo(biaoNum,payNo);     //这里计算出需要退款的信息
                payNo = info.orderno;

                Random rand = new Random();
                int num = rand.Next(1000, 9999);
                Parameteres param = new Parameteres();
                string orderno = DateTime.Now.ToString("yyyyMMddHHmmss") + num;

                Refund refund = new Refund();
                refund.appid = "wxd6cf371dbe9f5906";
                refund.key = "F56S456F4SD5F4S65F4AS65D4FS6D54F";
                refund.mch_id = "1519238551";
                refund.nonce_str = "5K8264ILTKCH16CQ2502SI8ZNMTM67VS";  //随机字符串
                refund.out_refund_no = orderno;  //退款单号随机生成
                refund.out_trade_no = payNo;             //支付单号
                refund.refund_fee = info.refundPrice;
                refund.total_fee = info.totalPrice;
                refund.notify_url = "https://feibiao.ty-gz.com:8137/dist/Handler/refundNotify.ashx";

                string strParam = getRefundParam(refund);
                string payResXML = Helper.PostWebRequest("https://api.mch.weixin.qq.com/secapi/pay/refund", strParam);
                ms.makeLog("退款方法:" + payResXML + ",refund.refund_fee:" + refund.refund_fee + ",refund.total_fee:" + refund.total_fee);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(payResXML);

                XmlNode rootNode = xmlDoc.SelectSingleNode("xml");

                XmlNode nodeScuecc = rootNode.SelectSingleNode("result_code");
                if (nodeScuecc == null)
                {
                    ms.makeLog("微信退款：result_code节点为空");
                    return "";
                }

                XmlNode refund_idNode = rootNode.SelectSingleNode("refund_id");
                if (refund_idNode != null)
                {
                    string refund_id = refund_idNode.InnerText;
                    refund_id = refund_id.Replace("<![CDATA[", "").Replace("]]", "");

                    XmlNode refund_feeNode = rootNode.SelectSingleNode("cash_refund_fee");
                    string refundfee = refund_feeNode.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                    XmlNode out_trade_noNode = rootNode.SelectSingleNode("out_trade_no");
                    string out_trade_no = out_trade_noNode.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                    XmlNode out_refund_noNode = rootNode.SelectSingleNode("out_refund_no");
                    string out_refund_no = out_refund_noNode.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                    int count = ms.selectRefundCount(refund_id);
                    if (count > 0)
                    {
                        return "";
                    }

                    int aa = ms.addRefund(out_trade_no, out_refund_no, refund_id, refundfee);

                    int changeState = ms.updateRefundState(out_trade_no);

                }
                return "成功退押金：" + info.refundPrice + "分";

            }
            catch (Exception ex)
            {
                ms.makeLog("退款方法异常:" + ex.ToString());
                return ex.ToString();
            }
        }

        private string testRefund(HttpContext context)
        {
            try
            {
                string payNo = context.Request["orderno"];
                Random rand = new Random();
                int num = rand.Next(1000, 9999);
                Parameteres param = new Parameteres();
                string orderno = DateTime.Now.ToString("yyyyMMddHHmmss") + num;

                Refund refund = new Refund();
                refund.appid = "wxd6cf371dbe9f5906";
                refund.key = "F56S456F4SD5F4S65F4AS65D4FS6D54F";
                refund.mch_id = "1519238551";
                refund.nonce_str = "5K8264ILTKCH16CQ2502SI8ZNMTM67VS";  //随机字符串
                refund.out_refund_no = orderno;  //退款单号随机生成
                refund.out_trade_no = payNo;             //支付单号
                DataTable dt = ms.getDeposit();
                int refundfee = 0;
                if (dt != null)
                {
                    refundfee = Convert.ToInt32(dt.Rows[0]["deposit"]);
                }
                int totalfee = ms.selectPayFee(payNo);
                refund.refund_fee = totalfee.ToString();
                refund.total_fee = totalfee.ToString();
                refund.notify_url = "https://feibiao.ty-gz.com:8137/dist/Handler/refundNotify.ashx";

                string strParam = getRefundParam(refund);
                string payResXML = Helper.PostWebRequest("https://api.mch.weixin.qq.com/secapi/pay/refund", strParam);
                //string certificatePath = ConfigurationManager.AppSettings["certificatePath"];
                //return strParam;
                var payRes = XDocument.Parse(payResXML);
                var root = payRes.Element("xml");

                //序列化相应参数返回给小程序
                var res = GetPayRequestParam(root, refund.appid, refund.key);
                string strJson = JsonConvert.SerializeObject(res);
                return strJson;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private string adminRefund(HttpContext context)
        {
             try
            {
                string payNo = context.Request["orderno"];
                string payMoney = context.Request["payPrice"];
                string refundMoney = context.Request["refundMoney"];

                Random rand = new Random();
                int num = rand.Next(1000, 9999);
                Parameteres param = new Parameteres();
                string orderno = DateTime.Now.ToString("yyyyMMddHHmmss") + num;

                Refund refund = new Refund();
                refund.appid = "wxd6cf371dbe9f5906";
                refund.key = "F56S456F4SD5F4S65F4AS65D4FS6D54F";
                refund.mch_id = "1519238551";
                refund.nonce_str = "5K8264ILTKCH16CQ2502SI8ZNMTM67VS";  //随机字符串
                refund.out_refund_no = orderno;  //退款单号随机生成
                refund.out_trade_no = payNo;             //支付单号
                refund.refund_fee = refundMoney;
                refund.total_fee = payMoney;
                refund.notify_url = "https://feibiao.ty-gz.com:8137/dist/Handler/refundNotify.ashx";

                string strParam = getRefundParam(refund);
                string payResXML = Helper.PostWebRequest("https://api.mch.weixin.qq.com/secapi/pay/refund", strParam);
                //ms.makeLog("退款方法:" + payResXML);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(payResXML);

                XmlNode rootNode = xmlDoc.SelectSingleNode("xml");
                Json jsonResult = new Json();
                XmlNode nodeScuecc = rootNode.SelectSingleNode("result_code");
                if (nodeScuecc == null)
                {
                    ms.makeLog("微信退款：result_code节点为空");
                    jsonResult.Success = false;
                    jsonResult.Msg = "退款失败";
                }
                else
                {
                    XmlNode refund_idNode = rootNode.SelectSingleNode("refund_id");
                    if (refund_idNode != null)
                    {
                        string refund_id = refund_idNode.InnerText;
                        refund_id = refund_id.Replace("<![CDATA[", "").Replace("]]", "");

                        XmlNode refund_feeNode = rootNode.SelectSingleNode("cash_refund_fee");
                        string refundfee = refund_feeNode.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                        XmlNode out_trade_noNode = rootNode.SelectSingleNode("out_trade_no");
                        string out_trade_no = out_trade_noNode.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                        XmlNode out_refund_noNode = rootNode.SelectSingleNode("out_refund_no");
                        string out_refund_no = out_refund_noNode.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                        int count = ms.selectRefundCount(refund_id);
                        if (count > 0)
                        {
                            jsonResult.Success = false;
                            jsonResult.Msg = "退款失败";
                        }
                        else
                        {
                            int aa = ms.addRefund(out_trade_no, out_refund_no, refund_id, refundfee);
                            int changeState = ms.updateRefundState(out_trade_no);

                            jsonResult.Success = true;
                            jsonResult.Msg = "退款成功";
                        }
                    }
                    else
                    {
                        jsonResult.Success = false;
                        jsonResult.Msg = "退款失败";
                    }
                }

                string strJson = JsonConvert.SerializeObject(jsonResult);
                return strJson;
            }
            catch (Exception ex)
            {
                ms.makeLog("管理员退款方法异常:" + ex.ToString());
                Json jsonResult = new Json();
                jsonResult.Success = false;
                jsonResult.Msg = "退款异常" + ex.ToString();
                string strJson = JsonConvert.SerializeObject(jsonResult);
                return strJson;
            }
        }

        /// <summary>
        /// 获取签名和参数
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetUnifiedOrderParam(string openid, Parameteres param)
        {
            //参与统一下单签名的参数，除最后的key外，已经按参数名ASCII码从小到大排序
            //var unifiedorderSignParam = string.Format("appid={0}&body={1}&mch_id={2}&nonce_str={3}&notify_url={4}&openid={5}&out_trade_no={6}&spbill_create_ip={7}&total_fee={8}&trade_type={9}&key={10}"
            //    , param.appid, param.body, param.mch_id, param.nonce, param.notify_url
            //    , openid, param.out_trade_no, param.spbill_create_ip, param.total_fee, param.trade_type, param.key);

            var unifiedorderSignParam = string.Format("appid={0}&attach={11}&body={1}&mch_id={2}&nonce_str={3}&notify_url={4}&openid={5}&out_trade_no={6}&spbill_create_ip={7}&total_fee={8}&trade_type={9}&key={10}"
                , param.Appid, param.Body, param.Mch_id, param.Nonce, param.Notify_url
                , openid, param.Out_trade_no, param.Spbill_create_ip, param.Total_fee, param.Trade_type, param.Key, param.Attach);
            //MD5
            var unifiedorderSign = MD5Encrypt(unifiedorderSignParam, new UTF8Encoding()).ToUpper();

            //构造统一下单的请求参数
            return string.Format(@"<xml>
                                <appid>{0}</appid>
                                <attach>{11}</attach>                                              
                                <body>{1}</body>
                                <mch_id>{2}</mch_id>   
                                <nonce_str>{3}</nonce_str>
                                <notify_url>{4}</notify_url>
                                <openid>{5}</openid>
                                <out_trade_no>{6}</out_trade_no>
                                <spbill_create_ip>{7}</spbill_create_ip>
                               <total_fee>{8}</total_fee>
                                <trade_type>{9}</trade_type>

                                <sign>{10}</sign>
                               </xml>", param.Appid, param.Body, param.Mch_id, param.Nonce, param.Notify_url, openid
                              , param.Out_trade_no, param.Spbill_create_ip, param.Total_fee, param.Trade_type, unifiedorderSign, param.Attach);

        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string MD5Encrypt(string input, Encoding encode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encode.GetBytes(input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }

        /// <summary>
        /// 获取返回给小程序的支付参数
        /// </summary>
        /// <param name="root"></param>
        /// <param name="appid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private PayRequesEntity GetPayRequestParam(XElement root, string appid, string key)
        {
            //当return_code 和result_code都为SUCCESS时才有我们要的prepay_id
            if (root.Element("return_code").Value == "SUCCESS" && root.Element("result_code").Value == "SUCCESS")
            {
                var package = "prepay_id=" + root.Element("prepay_id").Value;
                var nonceStr = GetNoncestr();
                var signType = "MD5";
                var timeStamp = Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

                var paySignParam = string.Format("appId={0}&nonceStr={1}&package={2}&signType={3}&timeStamp={4}&key={5}",
                     appid, nonceStr, package, signType, timeStamp, key);

                var paySign = MD5Encrypt(paySignParam, new UTF8Encoding()).ToUpper();

                var payEntity = new PayRequesEntity
                {
                    package = package,
                    nonceStr = nonceStr,
                    paySign = paySign,
                    signType = signType,
                    timeStamp = timeStamp
                };
                return payEntity;
            }

            return new PayRequesEntity();
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="strPwChar">传入生成的随机字符串可以使用哪些字符</param>
        /// <param name="intlen">传入生成的随机字符串的长度</param>
        public string GetNoncestr()
        {
            string strPwChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int intlen = 32;
            string strRe = "";
            int iRandNum;
            Random rnd = new Random();
            for (int i = 0; i < intlen; i++)
            {
                iRandNum = rnd.Next(strPwChar.Length);
                strRe += strPwChar[iRandNum];
            }
            return strRe;
        }

        public string getRefundParam(Refund refund)
        {
            string signParam = string.Format("appid={0}&mch_id={1}&nonce_str={2}&notify_url={8}&out_refund_no={4}&out_trade_no={3}&refund_fee={5}&total_fee={6}&key={7}", refund.appid, refund.mch_id, refund.nonce_str, refund.out_trade_no, refund.out_refund_no, refund.refund_fee, refund.total_fee, refund.key, refund.notify_url);
            string sign = MD5Encrypt(signParam, new UTF8Encoding()).ToUpper();

            //构造统一下单的请求参数
            string result = string.Format(@"<xml>
                                   <appid>{0}</appid>
                                   <mch_id>{1}</mch_id>
                                   <nonce_str>{2}</nonce_str> 
                                   <notify_url>{8}</notify_url>
                                   <out_refund_no>{3}</out_refund_no>
                                   <out_trade_no>{4}</out_trade_no>
                                   <refund_fee>{5}</refund_fee>
                                   <total_fee>{6}</total_fee>
                                   <sign>{7}</sign>
                                   </xml>
", refund.appid, refund.mch_id, refund.nonce_str, refund.out_refund_no, refund.out_trade_no, refund.refund_fee, refund.total_fee, sign, refund.notify_url);
            return result;
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