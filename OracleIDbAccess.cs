
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
    class OracleIDbAccess:IDbAccess
    {

        public bool IsTran
        {
            get;
            set;
        }

        public bool IsOpen
        {
            get;
            set;
        }

        public IDbConnection conn
        {
            get;
            set;
        }

        public IDbTransaction tran
        {
            get;
            set;
        }

        public string ConnectionStr
        {
            get;
            set;
        }

        public bool IsKeepConnect
        {
            get;
            set;
        }

        public void Open()
        {
            if (this.conn == null)
                this.conn = new OracleConnection(ConnectionStr);
            if (this.conn.State != ConnectionState.Open)
            { 
                this.conn.Open();
                IsOpen = true;
            }
        }

        public void Close()
        {
            IsOpen = false;
            IsTran = false;
            IsKeepConnect = false;
            this.conn.Close();
        }

        public void BeginTransaction()
        {
            if (this.tran == null)
            {
                tran = conn.BeginTransaction();
                IsTran = true;
            }
        }

        public void Rollback()
        {
            if(this.tran !=null)
            {
                tran.Rollback();
                tran = null;
                IsTran = false;
            }
        }

        public void Commit()
        {
            if (this.tran != null)
            {
                tran.Commit();
                tran = null;
                IsTran = false;
            }
        }

        public int ExecuteSql(string sql)
        {
            try
            {
                Open();
                OracleCommand comm = new OracleCommand(sql, this.conn as OracleConnection);
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!IsTran && !IsKeepConnect)
                {
                    Close();
                }
            }

        }

        public int ExecuteSql(string sql, System.Data.IDataParameter[] paramArr)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSql(string[] sqlArr)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSql(string[] sqlArr, System.Data.IDataParameter[] paramArr)
        {
            throw new NotImplementedException();
        }
        
        public object GetFirstColumnAndRow(string sql)
        {
            try
            {
                
                Open();
                OracleCommand comm = new OracleCommand(sql, this.conn as OracleConnection);
                return comm.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (!IsTran && !IsKeepConnect)
                {
                    Close();
                }
            }

        }


        public bool AddData(string tableName, Hashtable ht)
        {
            try
            {
                string insertSql = "insert into {0} ({1}) values ({2}) ";
                string insertTableOption = "";
                string insertTableValues = "";
                OracleCommand comm = new OracleCommand();
                Open();
                comm.Connection = this.conn as OracleConnection;

                foreach (DictionaryEntry item in ht)
                {
                    insertTableOption += item.Key.ToString() + ",";
                    insertTableValues += ":" + item.Key.ToString() + ",";
                    comm.Parameters.Add(new OracleParameter(":" + item.Key, item.Value));
                }
                insertTableOption = insertTableOption.TrimEnd(',');
                insertTableValues = insertTableValues.TrimEnd(',');
                comm.CommandText = string.Format(insertSql, tableName, insertTableOption, insertTableValues);
                return comm.ExecuteNonQuery() > -1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (!IsTran && !IsKeepConnect)
                {
                    Close();
                }
            }


        }

        /// <summary>判断表是否存在,注意 oracle 对象名采用大写判断
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>返回表是否存在</returns>
        public bool JudgeTableOrViewExist(string tableName)
        {
            string sql = string.Format("select count(1) from user_tables t where t.TABLE_NAME ='{0}'", tableName.ToUpper());
            int r = int.Parse(GetFirstColumnAndRow(sql).ToString());
            if (r > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            if(this.conn !=null && this.conn.State !=ConnectionState.Closed )
            {
                Close();
            }
        }
    }
}
