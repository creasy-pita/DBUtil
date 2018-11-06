using System;
using System.Data;
using DBUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DBUtilTest
{
    [TestClass]
    public class UnitTest1
    {
        string connStr1 = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
        string dbType1 = "oracle";

        string connStr2 = "Server=192.168.11.83;Database=szf_sjgj;Uid=root;Pwd=root;";
        string dbType2 = "mysql";

        [TestMethod]
        public void TestMethod1()
        {

            IDbAccess oracledb = IDbFactory.CreateIDb(connStr1, dbType1);
            DataTable dt = oracledb.GetDataTable(@"select BDCQZH, QLXZ, YWH, ZSGBH, SYQX, ZSLX, DBSJ, QLBSM, SIGNVALUE, SBH, SZSJ, QLBM, DJJG, QLR, QLQTZK, BDCDYH, MJ, ZL, ZSBSM, YT, QLLX, XZQBM, ZJH, GYQK, FJ, FWQBM, ZSZT, TNSVALUE, SIGNCERT from bdcqz where rownum<9");
            dt.TableName = "bdcqz1";
            IDbAccess mysqldb = IDbFactory.CreateIDb(connStr2, dbType2);
            bool su = mysqldb.ImportFromDatatable(dt);
            if (su)
            {
                Console.Write("sssssss");
            }

        }
    }
}
