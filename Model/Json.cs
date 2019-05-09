using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Model
{
    public class Json
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public DataTable Result { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }

        public int code { get; set; }
        public int count { get; set; }
        public DataTable data { get; set; }
        public string msg { get; set; }
    }
}
