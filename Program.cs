using DBUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtils
{
    class Program
    {
        static string connStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
        static string dbType = "oracle";

        static void Main(string[] args)
        {
            //IDbAccess db = new OracleIDbAcess();
            //db.ConnectionStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            //string sql = "select 32 from dual";
            //Console.WriteLine(db.GetFirstColumnAndRow(sql).ToString());

            
            TestAddTable();
            //TestAddData();
            //TestdeleteTable();
            //TestUpdateData();
            //TestGetDataTable();
            //TestUpdataOrAdd();
            //TestDeleteRow();
            GetDataTableWithPage();
        }

        public static void GetDataTableWithPage()
        {
            IDbAccess db = IDbFactory.CreateIDb(connStr, dbType);
            DataTable dt = db.GetDataTableWithPage("templjqfortest", "", "name", 3, 1);
        }

        public static void TestGetDataTable()
        {
            IDbAccess db = IDbFactory.CreateIDb(connStr, dbType);
            DataTable dt = db.GetDataTable("select * from templjqfortest");

        }

        public static void TestDeleteRow()
        {
            IDbAccess db = IDbFactory.CreateIDb(connStr, dbType);
            string tableName = "templjqfortest";
            string filterStr = "and name >'hello db7'";
            db.DeleteRow(tableName, filterStr);
        }
        public static void TestUpdataOrAdd()
        {
            IDbAccess db = IDbFactory.CreateIDb(connStr, dbType);
            string tableName = "templjqfortest";
            Hashtable ht = new Hashtable();
            ht.Add("name", "hello db8");
            ht.Add("name1", null);
            ht.Add("createtime", new DateTime(2121, 12, 11, 2, 12, 2, 11));
            ht.Add("supertiem", null);
            string filter = "and 1=1 and name=:name";
            IDataParameter[] paraArr = new OracleParameter[1];
            paraArr[0] = new OracleParameter()
            {
                ParameterName = "name",
                Value = "hello db8"
            };
            //filter = "";
            //paraArr = new OracleParameter[0];
            Console.WriteLine("" + db.UpdateOrAddData(tableName, ht, filter, paraArr));
        }
        public static void TestAddTable()
        {
            string sql = "create table templjqfortest( name varchar2(100),name1 varchar2(100),createtime date,supertiem timestamp)";
            string connStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            IDbAccess db = IDbFactory.CreateIDb(connStr, "oracle");
            if (!db.JudgeTableOrViewExist("templjqfortest"))
            {
                Console.WriteLine("插入行:" +( db.ExecuteSql(sql)>0?true:false).ToString());
            }

        }

        public static void TestdeleteTable()
        {
            string connStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            IDbAccess db = IDbFactory.CreateIDb(connStr, "oracle");
            
            if (db.JudgeTableOrViewExist("templjqfortest"))
            {
                string sql = "drop table templjqfortest";
                Console.WriteLine(":" + db.ExecuteSql(sql));
            }
        }

        public static void TestAddData()
        {
            string connStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            IDbAccess db = IDbFactory.CreateIDb(connStr, "oracle");
            string tableName = "templjqfortest";
            Hashtable ht = new Hashtable();
            ht.Add("name", "hello db2");
            ht.Add("createtime", new DateTime(2011, 2, 1, 2, 2, 2, 11));
            ht.Add("supertiem", null);
            
            Console.WriteLine("" + db.AddData(tableName, ht));

        }

        public static void TestUpdateData()
        {
            string connStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            IDbAccess db = IDbFactory.CreateIDb(connStr, "oracle");
            string tableName = "templjqfortest";
            Hashtable ht = new Hashtable();
            //ht.Add("name", "hello db3");
            ht.Add("createtime", new DateTime(2111, 12, 11, 2, 12, 2, 11));
            ht.Add("supertiem", null);
            string filter = "and 1=1 and name=:name";
            IDataParameter[] paraArr = new OracleParameter[1];
            paraArr[0] = new OracleParameter()
            {
                ParameterName = "name",
                Value = "hello db3"
            };
            //filter = "";
            //paraArr = new OracleParameter[0];
            Console.WriteLine("" + db.UpdateData(tableName, ht, filter, paraArr));

        }
    }
}
