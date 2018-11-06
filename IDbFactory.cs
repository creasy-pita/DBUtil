using DBUtils;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DBUtil
{
    /// <summary>
    /// 数据库访问类工厂
    /// </summary>
    public class IDbFactory
    {
        public static IDbAccess CreateIDb(string connStr, string dbType){
            IDbAccess dbAccess = null;
            if (dbType == "oracle")
            {
                OracleConnection conn = new OracleConnection(connStr);
                dbAccess = new OracleIDbAccess()
                {
                    conn = conn,
                    ConnectionStr = connStr,
                };
            }
            else if (dbType == "mysql")
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                dbAccess = new MySqlIDbAccess()
                {
                    conn = conn,
                    ConnectionStr = connStr,
                };
            }
            return dbAccess;
        }
    }
}
