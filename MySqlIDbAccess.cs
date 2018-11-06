
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DBUtil
{
    class MySqlIDbAccess : IDbAccess
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
                if (conn.State != ConnectionState.Open)
                    Open();
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
                 
                MySqlCommand comm = new MySqlCommand(sql, this.conn as MySqlConnection);
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
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
                MySqlCommand comm = new MySqlCommand(sql, this.conn as MySqlConnection);
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
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
                MySqlCommand comm = new MySqlCommand();
                comm.Connection = (MySqlConnection)this.conn;
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
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

        public int ExecuteStoreProcedure(string procedureName, IDataParameter[] paramArr)
        {
            try
            {
                Open();
                MySqlCommand cmd = new MySqlCommand(procedureName,(MySqlConnection)this.conn);
                if (IsTran) cmd.Transaction = (MySqlTransaction) this.tran;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(paramArr);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally {
                if (!IsTran && !IsKeepConnect)
                {
                    Close();
                }
            }
        }

        public IDataReader GetDataReader(string sql)
        {
            try
            {
                Open();
                MySqlCommand comm = (MySqlCommand)this.conn.CreateCommand();
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
                comm.CommandText = sql;
                return comm.ExecuteReader();
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if(!IsTran && !IsKeepConnect)
                { 
                    Close();
                }
            }
        }

        public IDataReader GetDataReader(string sql, IDataParameter[] paraArr)
        {
            try
            {
                Open();
                MySqlCommand comm = (MySqlCommand)this.conn.CreateCommand();
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
                comm.CommandText = sql;
                comm.Parameters.AddRange(paraArr);
                return comm.ExecuteReader();
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
        #region
        public DataTable GetDataTable(string sql)
        {
            DataSet ds = GetDataSet(sql);
            if ( ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        public DataTable GetDataTable(string sql, IDataParameter[] paraArr)
        {
            DataSet ds = GetDataSet(sql,paraArr);
            if ( ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }

        public DataSet GetDataSet(string sql)
        {
            try
            {
                Open();
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, (MySqlConnection)this.conn);
                if (IsTran) adapter.SelectCommand.Transaction = (MySqlTransaction)this.tran;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
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

        public DataSet GetDataSet(string sql, IDataParameter[] paraArr)
        {
            try
            {
                Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, (MySqlConnection)this.conn);
                if (IsTran) adapter.SelectCommand.Transaction = (MySqlTransaction)this.tran;
                adapter.SelectCommand.Parameters.AddRange(paraArr);

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
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

        /// <summary>调用存储过程
        /// 存储过程例子
        ///     定义
        /// CREATE OR REPLACE PROCEDURE GetTableInfo (
        ///  EMPS OUT sys_refcursor) AS
        ///BEGIN
        ///  OPEN EMPS for SELECT TABLE_NAME from user_tables ;
        ///END GetTableInfo;
        ///     调用
        ///declare 
        ///    ee sys_refcursor;
        ///    tab_name user_tables.table_name%Type;
        ///begin
        ///      GetTableInfo(ee);    
        ///      /*open ee;*/
        ///      loop
        ///           exit when ee%notfound;
        ///           fetch ee into tab_name;
        ///           DBMS_OUTPUT.PUT_LINE(表名：||tab_name);
        ///      end loop;
        ///      close ee;
        ///end;
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public DataSet GetDatasetByStoreProcedure(string procedureName, IDataParameter[] paramArr)
        {
            try
            {
                Open();
                MySqlCommand cmd = new MySqlCommand(procedureName, (MySqlConnection)this.conn);

                if (IsTran) cmd.Transaction = (MySqlTransaction)this.tran;
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddRange(paramArr);
                DataSet ds = new DataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(ds);
                return ds;
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
        #endregion


        public object GetFirstColumnAndRow(string sql)
        {
            try
            {
                MySqlCommand comm = new MySqlCommand(sql);
                Open();
                comm.Connection = (MySqlConnection)this.conn;
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
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

        public object GetFirstColumnAndRow(string sql, IDataParameter[] paraArr)
        {
            try
            {
                Open();
                MySqlCommand comm = new MySqlCommand(sql);
                comm.Connection = (MySqlConnection)this.conn;
                comm.Parameters.AddRange(paraArr);
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
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

        public string GetFristColumnAndRowString(string sql, bool isReturnNull=false)
        {
            object obj = GetFirstColumnAndRow(sql);
            if(obj==null)
            {
                return isReturnNull ? null : "";
            }
            return obj.ToString();
        }

        public string GetFristColumnAndRowString(string sql,IDataParameter[] paraArr, bool isReturnNull = false)
        {
            object obj = GetFirstColumnAndRow(sql,paraArr);
            if (obj == null)
            {
                return isReturnNull ? null : "";
            }
            return obj.ToString();
        }

        #region

        public bool UpdateData(string tableName, Hashtable ht, string filterStr)
        {
            try
            {
                string sql = "update {0} set {1} where 1=1 {2} ";
                string updateSetSql = "";
                List<OracleParameter> paras = new List<OracleParameter>();
                foreach (DictionaryEntry item in ht)
                {
                    if (item.Value == null)
                    {
                        updateSetSql += " " + item.Key + "=null,";
                    }
                    else
                    {
                        updateSetSql += " " + item.Key + "=" + ":" + item.Key + ",";
                    }
                    paras.Add(new OracleParameter(item.Key.ToString(), item.Value));
                }
                updateSetSql = updateSetSql.TrimEnd(',');
                sql = string.Format(sql, tableName, updateSetSql, filterStr);
                return ExecuteSql(sql, paras.ToArray()) > 0 ? true : false;
            }
            catch (Exception e)
            {
                throw e;

            }
            finally
            {

            }
        }
        public bool UpdateData(string tableName, Hashtable ht, string filterStr, IDataParameter[] paraArr)
        {
            try
            {
                string sql = "update {0} set {1} where 1=1 {2} ";
                string updateSetSql = "";
                List<IDbDataParameter> paras = new List<IDbDataParameter>();
                foreach (DictionaryEntry item in ht)
                {
                    if (item.Value == null)
                    {
                        updateSetSql += " " + item.Key + "=null,";
                    }
                    else
                    {
                        updateSetSql += " " + item.Key + "=" + ":" + item.Key + ",";
                    }
                    paras.Add(new OracleParameter(item.Key.ToString(), item.Value));
                }
                foreach (OracleParameter para in paraArr)
                {
                    if (!IsContiansParameter(paras ,para.ParameterName))
                        paras.Add(new OracleParameter(para.ParameterName, para.Value));
                }
                updateSetSql = updateSetSql.TrimEnd(',');
                sql = string.Format(sql, tableName, updateSetSql, filterStr);
                return ExecuteSql(sql, paras.ToArray()) > 0 ? true : false;
            }
            catch (Exception e)
            {
                throw e;

            }
            finally
            {

            }
        }

        private bool IsContiansParameter(List<IDbDataParameter> paraArr, string parameterName)
        {
            foreach (IDbDataParameter item in paraArr)
            {
                if(item.ParameterName == parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateOrAddData(string tableName, Hashtable ht,string filterStr)
        {
            if(GetFristColumnAndRowString( string.Format("select count(1) from {0} where 1=1 {1}", tableName, filterStr)) =="0")
            {
                return AddData(tableName, ht);
            }
            else
            {
                return UpdateData(tableName, ht, filterStr);
            }
        }

        public bool UpdateOrAddData(string tableName, Hashtable ht, string filterStr, IDataParameter[] paraArr)
        {
            if (GetFristColumnAndRowString(string.Format("select count(1) from {0} where 1=1 {1}", tableName, filterStr), paraArr) == "0")
            {
                return AddData(tableName, ht);
            }
            else
            {
                return UpdateData(tableName, ht, filterStr, paraArr);
            }
        }
        #endregion

        #region

        public bool DeleteRow(string tableName, string filterStr)
        {
            string sql = "delete from {0} where 1=1 {1}";
            sql = string.Format(sql, tableName, filterStr);
            return ExecuteSql(sql) > 0;
        }

        public bool DeleteRow(string tableName, string filterStr, IDbDataParameter[] paraArr)
        {
            string sql = "delete from {0} where 1=1 {1}";
            sql = string.Format(sql, tableName, filterStr);
            return ExecuteSql(sql, paraArr) > 0;
        }
        #endregion

        public bool ImportFromDatatable(DataTable dataTable)
        {
            if (null == dataTable || dataTable.Rows.Count <= 0)
            {
                return false; //"添加失败！DataTable暂无数据！";
            }
            if (string.IsNullOrEmpty(dataTable.TableName))
            {
                return false; //"添加失败！请先设置DataTable的名称！";
            }
            // 构建INSERT语句
            try
            {
                Open();
                MySqlCommand comm = (MySqlCommand)conn.CreateCommand();
                if (IsTran) comm.Transaction = (MySqlTransaction)this.tran;
                StringBuilder nameBuilder = new StringBuilder(string.Format("INSERT INTO {0}(", dataTable.TableName));
                foreach (DataColumn c in dataTable.Columns)
                {
                    nameBuilder.AppendFormat("{0},", c.ColumnName);
                }
                nameBuilder.Remove(nameBuilder.Length - 1, 1);
                nameBuilder.Append(") VALUES(");
                foreach (DataRow row in dataTable.Rows)
                {
                    StringBuilder valueBuilder = new StringBuilder();
                    foreach (DataColumn c in dataTable.Columns)
                    {
                        //if (c.DataType == typeof(DateTime))
                        //{
                        //    string val = ""
                        //}
                        string value = row[c.ColumnName].ToString();
                        valueBuilder.AppendFormat("'{0}',", value);
                    }
                    valueBuilder.Remove(valueBuilder.Length - 1, 1);
                    valueBuilder.Append(")");
                    string sql = string.Format("{0}{1}", nameBuilder.ToString(), valueBuilder.ToString());
                    comm.CommandText = sql;
                    comm.ExecuteNonQuery();
                }
                return true;
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
                DataTable dt = GetDataTable(string.Format(" select DATA_TYPE,COLUMN_NAME from user_tab_cols where table_name='{0}'", tableName.ToUpper()));
                Hashtable ht_pre = new Hashtable();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["DATA_TYPE"].ToString().ToUpper().IndexOf("TIMESTAMP")>-1)
                        {
                            ht_pre.Add(dt.Rows[i]["COLUMN_NAME"].ToString(), dt.Rows[i]["DATA_TYPE"].ToString());
                        }
                    }
                }

                string insertTableOption = "";
                string insertTableValues = "";

                List<OracleParameter> pList = new List<OracleParameter>();
                foreach (DictionaryEntry item in ht)
                {
                    insertTableOption += item.Key.ToString() + ",";
                    if (ht_pre.Contains(item.Key.ToString().ToUpper()))
                    {
                        if (item.Value == null)
                        {
                            insertTableValues += "null,";
                        }
                        else
                        {
                            insertTableValues += "to_date(:" + item.Key.ToString() + ", 'yyyy-MM-dd hh24:mi:ss ff'),";
                        }
                    }
                    else
                    {
                        if (item.Value == null)
                        {
                            insertTableValues += "null,";
                        }
                        else
                        {
                            insertTableValues += ":" + item.Key.ToString() + ",";
                        }
                    }
                    pList.Add(new OracleParameter(item.Key.ToString(), item.Value));
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


        public bool GetCount(string tableName, string filterStr)
        {
            throw new NotImplementedException();
        }

        public bool GetCount(string tableName, string filterStr, IDataParameter[] paraArr)
        {
            throw new NotImplementedException();
        }

        public DataTable GetDataTableWithPage(string tableName,string filterStr, string orderStr,int pageSize, int pageIndex )
        {
            string sql = string.Format("select * from {0} where 1=1 {1} ", tableName,filterStr);
            string pageSql = GetPageSql(sql,"order by "+ orderStr,pageSize,pageIndex);
            return GetDataTable(pageSql);
        }

        public string GetPageSql(string sql, string orderStr, int pageSize, int pageIndex)
        {
            string pageSql = "select * from (select rownum rnum, inner.* from ({0} {1} ) inner ) outter where outter.rnum between {2} and {3}";
            pageSql = string.Format(pageSql, sql, orderStr, pageSize *( pageIndex-1) + 1, pageSize * (pageIndex));
            return pageSql;
        }
        
    }
}
