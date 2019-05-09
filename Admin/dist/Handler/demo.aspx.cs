using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin.dist.Handler
{
    public partial class demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string str = "<xml><appid><![CDATA[wxd6cf371dbe9f5906]]></appid><bank_type><![CDATA[CFT]]></bank_type><cash_fee><![CDATA[1]]></cash_fee><fee_type><![CDATA[CNY]]></fee_type><is_subscribe><![CDATA[N]]></is_subscribe><mch_id><![CDATA[1519238551]]></mch_id><nonce_str><![CDATA[5K8264ILTKCH16CQ2502SI8ZNMTM67VS]]></nonce_str><openid><![CDATA[o8AoK470J221fnNX85NFKcXDxj6o]]></openid><out_trade_no><![CDATA[201812101329418222]]></out_trade_no><result_code><![CDATA[SUCCESS]]></result_code><return_code><![CDATA[SUCCESS]]></return_code><sign><![CDATA[527338E9676BEE6442D7D2B3100BDBAC]]></sign><time_end><![CDATA[20181210132955]]></time_end><total_fee>1</total_fee><trade_type><![CDATA[JSAPI]]></trade_type><transaction_id><![CDATA[4200000236201812107283618154]]></transaction_id></xml>";
            Post("http://localhost:64823/dist/Handler/notify.ashx", str);
        }

        //Post请求
        public static string Post(string url, string obj)
        {
            string param = (obj);//参数
            byte[] bs = Encoding.Default.GetBytes(param);

            //创建一个新的HttpWebRequest对象。
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

            // 将方法属性设置为“POST”以将数据发布到URI。
            req.Method = "POST";

            //设置contentType属性。
            req.ContentType = "application/json";

            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                HttpWebResponse response2 = (HttpWebResponse)req.GetResponse();
                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);
                string text2 = sr2.ReadToEnd();
                return text2;
            }

        }
    }
}