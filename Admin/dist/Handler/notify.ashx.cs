using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using DAL;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Admin.dist.Handler
{
    /// <summary>
    /// notify 的摘要说明
    /// </summary>
    public class notify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //接收post参数
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            string result = Encoding.UTF8.GetString(b);
            if (string.IsNullOrEmpty(result))
            {
                context.Response.Write("没有收到回调参数");
                return;
            }

            ManagerServer ms = new ManagerServer();
            //ms.makeLog("notify:" + result);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);

            XmlNode rootNode = xmlDoc.SelectSingleNode("xml");

            XmlNode nodeScuecc = rootNode.SelectSingleNode("result_code");
            if (nodeScuecc == null)
            {
                return;
            }

            XmlNode node = rootNode.SelectSingleNode("openid");
            if (node != null)
            {
                string openid = node.InnerText;
                openid = openid.Replace("<![CDATA[", "").Replace("]]", "");

                XmlNode nodePrice = rootNode.SelectSingleNode("total_fee");
                string price = nodePrice.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                XmlNode nodeOrderno = rootNode.SelectSingleNode("out_trade_no");
                string orderno = nodeOrderno.InnerText.Replace("<![CDATA[", "").Replace("]]", "");

                //获取下单的id和局数
                XmlNode nodeAttach = rootNode.SelectSingleNode("attach");
                if (nodeAttach != null)
                {
                    string attach = nodeAttach.InnerText.Replace("<![CDATA[", "").Replace("]]", "");
                    JObject jo = (JObject)JsonConvert.DeserializeObject(attach);
                    string ids = jo["ids"].ToString();
                    string quantity = jo["quantity"].ToString();
                    int changeResult = ms.updatePayState(ids, quantity, orderno);
                }

                int count = ms.selectPayCount(orderno);
                if (count > 0)
                {
                    return;
                }
                
                int aa = ms.addNotify(openid, price, orderno);
            }
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