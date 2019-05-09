using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Deposit
    {
        public bool Success { get; set; }
        public string GameMoney { get; set; }
        public string DepositMoney { get; set; }
        public string TotalMoney { get; set; }
    }
}
