using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            //IDbAccess db = new OracleIDbAcess();
            //db.ConnectionStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            //string sql = "select 32 from dual";
            //Console.WriteLine(db.GetFirstColumnAndRow(sql).ToString());

            
            TestAddTable();
            TestAddData();
            TestdeleteTable();
        }

        public static void TestAddTable()
        {
            string sql = "create table templjqfortest( name varchar2(100))";
            IDbAccess db = new OracleIDbAccess();
            db.ConnectionStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            if (!db.JudgeTableOrViewExist("templjqfortest"))
            {
                Console.WriteLine("插入行:" +( db.ExecuteSql(sql)>0?true:false).ToString());
            }

        }

        public static void TestdeleteTable()
        {
            IDbAccess db = new OracleIDbAccess();
            db.ConnectionStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            if (db.JudgeTableOrViewExist("templjqfortest"))
            {
                string sql = "drop table templjqfortest";
                Console.WriteLine(":" + db.ExecuteSql(sql));
            }
        }

        public static void TestAddData()
        {
            IDbAccess db = new OracleIDbAccess();
            db.ConnectionStr = "Data Source=kfhzbdc200;User ID=sjgj;Password=sjgj";
            string tableName = "templjqfortest";
            Hashtable ht = new Hashtable();
            ht.Add("name", "hello db");
            Console.WriteLine("" + db.AddData(tableName, ht));

        }
    }
}
