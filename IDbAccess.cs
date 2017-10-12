using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace DBUtils
{
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

        #region 
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

        object GetFirstColumnAndRow(string sql);

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


        #region
        /// <summary>
        /// 判断指定表或视图是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool JudgeTableOrViewExist(string tableName);
        #endregion
        #region

        #endregion
    }
}
