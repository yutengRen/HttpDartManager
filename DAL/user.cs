using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class user
    {
        public static int userLogin(string username,string pwd)
        {
            string sql = string.Format("select COUNT(*) from `users` where username='{0}' and pwd='{1}'", username, pwd);
            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(CommandType.Text, sql));
            return count;
        }


    }
}
