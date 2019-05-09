using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Refund
    {
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string nonce_str { get; set; }
        public string notify_url { get; set; }
        public string out_trade_no { get; set; }        //商户订单号
        public string out_refund_no { get; set; }
        public string refund_fee { get; set; }
        public string sign { get; set; }
        public string sign_type { get; set; }
        public string total_fee { get; set; }
        public string transaction_id { get; set; }  //微信订单号
        public string key { get; set; }
    }
}
