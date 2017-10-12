
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
            try
            {
                OracleCommand comm = new OracleCommand(sql, this.conn as OracleConnection);
                if (IsTran) comm.Transaction = (OracleTransaction)this.tran;
                Open();
                comm.Parameters.AddRange(paramArr);
                return comm.ExecuteNonQuery();
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

        public int ExecuteSql(string[] sqlArr)
        {
            try
            {
                OracleCommand comm = new OracleCommand();
                comm.Connection = (OracleConnection)this.conn;
                if (IsTran) comm.Transaction = (OracleTransaction)this.tran;
                Open();
                int r = -1;
                foreach (string sql in sqlArr)
                {
                    comm.CommandText = sql;
                    r = comm.ExecuteNonQuery();
                }
                return r;
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

        public int ExecuteSql(string[] sqlArr, System.Data.IDataParameter[] paramArr)
        {
            throw new NotImplementedException();
        }
        
        public object GetFirstColumnAndRow(string sql)
        {
            try
            {
                OracleCommand comm = new OracleCommand(sql);
                Open();
                comm.Connection = (OracleConnection)this.conn;
                if (IsTran) comm.Transaction = (OracleTransaction)this.tran;
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
                //DataTable dt = GetDataTable(string.Format(" select DATA_TYPE,COLUMN_NAME from user_tab_cols where table_name='{0}'", tableName.ToUpper()));
                //Hashtable ht_pre = new Hashtable();
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        if (dt.Rows[i]["DATA_TYPE"].ToString().ToUpper() == "DATE")
                //        {
                //            ht_pre.Add(dt.Rows[i]["COLUMN_NAME"].ToString(), dt.Rows[i]["DATA_TYPE"].ToString());
                //        }
                //    }
                //}
                string insertTableOption = "";
                string insertTableValues = "";

                List<OracleParameter> pList = new List<OracleParameter>();
                foreach (DictionaryEntry item in ht)
                {
                    insertTableOption += item.Key.ToString() + ",";
                    insertTableValues += ":" + item.Key.ToString() + ",";
                    pList.Add(new OracleParameter(":" + item.Key, item.Value));
                }
                insertTableOption = insertTableOption.TrimEnd(',');
                insertTableValues = insertTableValues.TrimEnd(',');
                string insertSql = "insert into {0} ({1}) values ({2}) ";
                insertSql = string.Format(insertSql, tableName, insertTableOption, insertTableValues);
                return ExecuteSql(insertSql, pList.ToArray()) > -1 ? true : false;
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
