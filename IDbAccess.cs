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


        #region �������
        void Open();

        void Close();
        #endregion

        #region 
        void BeginTransaction();

        void Rollback();

        void Commit();
        #endregion

        #region ִ��sql��� ������Ӱ�������
        /// <summary>
        /// ִ��sql��� ������Ӱ�������
        /// </summary>
        /// <param name="sql">��Ҫִ�е�sql���</param>
        /// <returns>��Ӱ�������</returns>
        int ExecuteSql(string sql);
        /// <summary>
        /// ִ�д�������sql���  ������Ӱ�������
        /// </summary>
        /// <param name="sql">��Ҫִ�е�sql���</param>
        /// <returns>��Ӱ�������</returns>
        int ExecuteSql(string sql, IDataParameter[] paramArr);

        int ExecuteSql(string[] sqlArr);

        int ExecuteSql(string[] sqlArr, IDataParameter[] paramArr);

        #endregion

        object GetFirstColumnAndRow(string sql);

        #region 
        /// <summary>
        /// ��ָ��������һ������
        /// ����֤ �����������Ͷ�ͨ�� �ؼ��� date����
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="ht">�ֶ���ֵ��</param>
        /// <returns></returns>
        bool AddData(string tableName, Hashtable ht);

        #endregion


        #region
        /// <summary>
        /// �ж�ָ�������ͼ�Ƿ����
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool JudgeTableOrViewExist(string tableName);
        #endregion
        #region

        #endregion
    }
}
