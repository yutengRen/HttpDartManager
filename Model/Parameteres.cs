using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Parameteres
    {
        string appid = "wxd6cf371dbe9f5906";
        public string Appid { get { return appid; } set { appid = value; } }
        string attach;
        public string Attach { get { return attach; } set { attach = value; } }
        string mch_id = "1519238551";
        public string Mch_id { get { return mch_id; } set { mch_id = value; } }
        string nonce_str = "5K8264ILTKCH16CQ2502SI8ZNMTM67VS";
        public string Nonce_str { get { return nonce_str; } set { nonce_str = value; } }
        string sign;
        public string Sign { get { return sign; } set { sign = value; } }
        string body = "pay";
        public string Body { get { return body; } set { body = value; } }
        string out_trade_no = "";     //订单号
        public string Out_trade_no { get { return out_trade_no; } set { out_trade_no = value; } }
        string total_fee = "1";
        public string Total_fee { get { return total_fee; } set { total_fee = value; } }
        string spbill_create_ip = "192.168.1.1";
        public string Spbill_create_ip { get { return spbill_create_ip; } set { spbill_create_ip = value; } }
        string notify_url = "https://feibiao.ty-gz.com:8137/dist/Handler/notify.ashx";
        public string Notify_url { get { return notify_url; } set { notify_url = value; } }
        string trade_type = "JSAPI";
        public string Trade_type { get { return trade_type; } set { trade_type = value; } }
        string nonce = "5K8264ILTKCH16CQ2502SI8ZNMTM67VS";
        public string Nonce { get { return nonce; } set { nonce = value; } }
        string key = "F56S456F4SD5F4S65F4AS65D4FS6D54F";
        public string Key { get { return key; } set { key = value; } }
    }
}
