using DBUtils;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace DBUtil
{
    class IDbFactory
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
            return dbAccess;
        }
    }
}
