using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace DBUtils
{
    /// <summary>
    /// 数据库访问接口类
    /// 关键验证点： 事务 （多个事务同时测试；事务中包含增删改 查 ；每个方法能否正常执行；操作方法考虑各种数据）,
    ///             Null 值的处理 
    /// </summary>
    interface IDbAccess:IDisposable
    {

        bool IsTran { get; set; }

        bool IsOpen { get; set; }

        IDbConnection conn { get; set; }

        IDbTransaction tran { get; set; }

        string ConnectionStr { get; set; }

        bool IsKeepConnect { get; set; }


        #region 连接相关
        void Open();

        void Close();
        #endregion

        #region 事务相关
        void BeginTransaction();

        void Rollback();

        void Commit();
        #endregion

        #region 执行sql语句 返回受影响的行数
        /// <summary>
        /// 执行sql语句 返回受影响的行数
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <returns>受影响的行数</returns>
        int ExecuteSql(string sql);
        /// <summary>
        /// 执行带参数的sql语句  返回受影响的行数
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <returns>受影响的行数</returns>
        int ExecuteSql(string sql, IDataParameter[] paramArr);

        int ExecuteSql(string[] sqlArr);

        int ExecuteSql(string[] sqlArr, IDataParameter[] paramArr);

        #endregion

        #region 获取dataset 或datatable
        DataTable GetDataTable(string sql);

        DataTable GetDataTable(string sql, IDataParameter[] paraArr);

        DataSet GetDataSet(string sql);

        DataSet GetDataSet(string sql, IDataParameter[] paraArr);

        #endregion

        #region
        object GetFirstColumnAndRow(string sql);
        object GetFirstColumnAndRow(string sql, IDataParameter[] paraArr);
        string GetFristColumnAndRowString(string sql, bool isReturnNull = false);
        /// <summary>
        /// 查询获取结果集的首行首列 字符串值
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="paraArr">查询语句中的参数数组</param>
        /// <param name="isReturnNull">是否返回Null</param>
        /// <returns></returns>
        string GetFristColumnAndRowString(string sql, IDataParameter[] paraArr, bool isReturnNull = false);
        #endregion

        #region
        /// <summary>
        /// 向指定表增加一行数据
        /// 需验证 各种数据类型都通过 关键在 date类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">字段列值对</param>
        /// <returns></returns>
        bool AddData(string tableName, Hashtable ht);

        #endregion

        #region 删除
        /// <summary>
        /// 按过滤条件删除行
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filterStr">过滤条件 以and开头</param>
        /// <returns></returns>
        bool DeleteRow(string tableName, string filterStr);
        /// <summary>
        /// 按过滤条件删除行
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filterStr">过滤条件 以and开头</param>
        /// <param name="paraArr">过滤条件中的参数数组</param>
        /// <returns></returns>
        bool DeleteRow(string tableName, string filterStr, IDbDataParameter[] paraArr);
        #endregion

        #region 更新

        bool UpdateData(string tableName, Hashtable ht, string filterStr);
        /// <summary>
        /// 更新指定过滤条件的数据行
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ht">更新的键值对</param>
        /// <param name="filterStr">过滤条件 以and开头</param>
        /// <param name="paraArr">过滤条件中的参数数组</param>
        /// <returns></returns>
        bool UpdateData(string tableName, Hashtable ht, string filterStr, IDataParameter[] paraArr);
        bool UpdateOrAddData(string tableName, Hashtable ht, string filterStr);
        bool UpdateOrAddData(string tableName, Hashtable ht, string filterStr, IDataParameter[] paraArr);
        #endregion
        #region
        /// <summary>
        /// 判断指定表或视图是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool JudgeTableOrViewExist(string tableName);
        #endregion
        #region
        bool GetCount(string tableName, string filterStr);
        bool GetCount(string tableName, string filterStr,IDataParameter[] paraArr);

        DataTable GetDataTableWithPage(string tableName, string filterStr, string orderStr, int pageSize, int pageIndex);

        string GetPageSql(string sql,string orderStr,int pageSize, int pageIndex);
        #endregion
    }
}
