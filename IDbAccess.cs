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
    /// ���ݿ���ʽӿ���
    /// �ؼ���֤�㣺 ���� ���������ͬʱ���ԣ������а�����ɾ�� �� ��ÿ�������ܷ�����ִ�У������������Ǹ������ݣ�,
    ///             Null ֵ�Ĵ��� 
    /// </summary>
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

        #region �������
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

        #region ��ȡdataset ��datatable
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
        /// ��ѯ��ȡ��������������� �ַ���ֵ
        /// </summary>
        /// <param name="sql">��ѯ���</param>
        /// <param name="paraArr">��ѯ����еĲ�������</param>
        /// <param name="isReturnNull">�Ƿ񷵻�Null</param>
        /// <returns></returns>
        string GetFristColumnAndRowString(string sql, IDataParameter[] paraArr, bool isReturnNull = false);
        #endregion

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

        #region ɾ��
        /// <summary>
        /// ����������ɾ����
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filterStr">�������� ��and��ͷ</param>
        /// <returns></returns>
        bool DeleteRow(string tableName, string filterStr);
        /// <summary>
        /// ����������ɾ����
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filterStr">�������� ��and��ͷ</param>
        /// <param name="paraArr">���������еĲ�������</param>
        /// <returns></returns>
        bool DeleteRow(string tableName, string filterStr, IDbDataParameter[] paraArr);
        #endregion

        #region ����

        bool UpdateData(string tableName, Hashtable ht, string filterStr);
        /// <summary>
        /// ����ָ������������������
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ht">���µļ�ֵ��</param>
        /// <param name="filterStr">�������� ��and��ͷ</param>
        /// <param name="paraArr">���������еĲ�������</param>
        /// <returns></returns>
        bool UpdateData(string tableName, Hashtable ht, string filterStr, IDataParameter[] paraArr);
        bool UpdateOrAddData(string tableName, Hashtable ht, string filterStr);
        bool UpdateOrAddData(string tableName, Hashtable ht, string filterStr, IDataParameter[] paraArr);
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
        bool GetCount(string tableName, string filterStr);
        bool GetCount(string tableName, string filterStr,IDataParameter[] paraArr);

        DataTable GetDataTableWithPage(string tableName, string filterStr, string orderStr, int pageSize, int pageIndex);

        string GetPageSql(string sql,string orderStr,int pageSize, int pageIndex);
        #endregion
    }
}
