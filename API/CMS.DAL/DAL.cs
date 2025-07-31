using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using System.Collections.Specialized;
//using Core_App.BO;

namespace Core_App
{
    ///<summary>
    ///Core App - Data access layer for SQL server
    ///Created and Maintaining by CTS DevTeam
    ///Latest updated on 22 Apr 2016
    ///</summary>
    public class DAL
    {
        //public static string ConnectionString = ConfigurationManager.ConnectionStrings["AppConnString"].ConnectionString;
        public static string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private string mstr_ConnectionString;
        private SqlConnection mobj_SqlConnection;
        private SqlCommand mobj_SqlCommand;
        private int mint_CommandTimeout = 60;
        private string defaultTableName = "DataTable";

        public delegate object CallBackType(string sNameMapped, object oValue, string sColumnName = null);

        public SqlParameter InitParameter(string parameterName, SqlDbType dataType, int dataSize, 
            object parameterValue, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            SqlParameter oParam = new SqlParameter(parameterName, dataType, dataSize);
            if (parameterValue != null && dataType == SqlDbType.UniqueIdentifier)
            {
                oParam.Value = new Guid(parameterValue.ToString());
            }
            else
            {
                oParam.Value = parameterValue;
            }
            oParam.Direction = parameterDirection;
            return oParam;
        }

        

        public void CloseConnection()
        {
            if (mobj_SqlConnection.State != ConnectionState.Closed) mobj_SqlConnection.Close();
        }

        ///<summary>
        ///Check existing data by SQL command
        ///</summary>
        ///<param name="columName">Column name</param>
        ///<param name="tableName">Table name</param> 
        public Boolean CheckExistingData(string columName, string tableName)
        {
            mobj_SqlConnection = new SqlConnection(ConnectionString);
            SqlCommand cmdSelect;
            mobj_SqlConnection.Open();
            string sSelect = "Select " + columName + " From " + tableName;
            cmdSelect = new SqlCommand(sSelect, mobj_SqlConnection);
            SqlDataReader dr = cmdSelect.ExecuteReader();

            if (dr.Read())
            {
                mobj_SqlConnection.Close();
                return false;
            }
            else
            {
                mobj_SqlConnection.Close();
                return true;
            }

        }
    
        ///<summary>
        ///Execute SQL command and return Dataset
        ///</summary>
        ///<param name="sqlCommand">SQL command</param>
        ///<param name="tableName">Table name will be assigned to Dataset. Return [DataTable] if null</param>         
        public DataSet GetDataset(string sqlCommand, string tableName)
        {
            try
            {
                mstr_ConnectionString = ConnectionString;

                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand();
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.Connection = mobj_SqlConnection;

                mobj_SqlCommand.CommandText = sqlCommand;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;

                mobj_SqlConnection.Open();

                SqlDataAdapter adpt = new SqlDataAdapter(mobj_SqlCommand);
                DataSet ds = new DataSet();
                adpt.Fill(ds, tableName != null ? tableName : defaultTableName);
                return ds;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        ///<summary>
        ///Execute stored procedure and return Dataset
        ///</summary>
        ///<param name="procedureName">Stored procedure name</param>
        ///<param name="tableName">Table name will be assigned to Dataset. Return [DataTable] if null</param>
        ///<param name="parameterCollection">SQL parameter list (nullable)</param>        
        public DataSet GetDataset(string procedureName, string tableName, System.Collections.ArrayList parameterCollection)
        {
            try
            {
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand(procedureName, mobj_SqlConnection);
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                if (parameterCollection != null)
                {
                    foreach (object oParameter in parameterCollection)
                    {
                        mobj_SqlCommand.Parameters.Add(oParameter);
                        
                    }
                    parameterCollection.Clear();
                }
                

                mobj_SqlConnection.Open();
                SqlDataAdapter adpt = new SqlDataAdapter(mobj_SqlCommand);
                DataSet ds = new DataSet();
                adpt.Fill(ds, tableName != null ? tableName : defaultTableName);

                return ds;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
      
        ///<summary>
        ///Execute scalar by command
        ///</summary>
        ///<param name="sqlCommand">SQL command</param>   
        public void ExecuteScalar(string sqlCommand)
        {
            try
            {
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand();
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.Connection = mobj_SqlConnection;
                mobj_SqlCommand.CommandText = sqlCommand;

                mobj_SqlConnection.Open();
                mobj_SqlCommand.ExecuteScalar();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        ///<summary>
        ///Execute scalar by stored procedure
        ///</summary>
        ///<param name="procedureName">Stored procedure name</param>
        ///<param name="parameterCollection">SQL parameter list (nullable)</param>        
        public void ExecuteScalar(string procedureName, System.Collections.ArrayList parameterCollection)
        {
            try
            {
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand(procedureName, mobj_SqlConnection);
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                if (parameterCollection != null)
                {
                    foreach (object oParameter in parameterCollection)
                    {
                        mobj_SqlCommand.Parameters.Add(oParameter);
                    }
                    parameterCollection.Clear();
                }

                mobj_SqlConnection.Open();
                mobj_SqlCommand.ExecuteScalar(); 
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }


        ///<summary>
        ///Execute stored procedure and return string value
        ///</summary>
        ///<param name="procedureName">Stored procedure name</param>
        ///<param name="parameterCollection">SQL parameter list (nullable)</param>
        public string ExecuteStoredProcedureAndReturnString(string procedureName, System.Collections.ArrayList parameterCollection)
        {
            try
            {
                string returnVal = string.Empty;
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand(procedureName, mobj_SqlConnection);
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                if (parameterCollection != null)
                {
                    foreach (object oParameter in parameterCollection)
                    {
                        mobj_SqlCommand.Parameters.Add(oParameter);
                    }
                    parameterCollection.Clear();
                }

                mobj_SqlConnection.Open();
                object sqlReturn = mobj_SqlCommand.ExecuteScalar();
                if (sqlReturn != null)
                {
                    returnVal = sqlReturn.ToString();
                }
                return returnVal;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return "";
            }
            finally
            {
                CloseConnection();
            }
        }

        ///<summary>
        ///Execute stored procedure and return int value
        ///</summary>
        ///<param name="procedureName">Stored procedure name</param>
        ///<param name="parameterCollection">SQL parameter list (nullable)</param>
        public int ExecuteStoredProcedure(string procedureName, System.Collections.ArrayList parameterCollection, int defaultValue = 0)
        {
            try
            {
                int returnVal = 0;
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand(procedureName, mobj_SqlConnection);
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                if (parameterCollection != null)
                {
                    foreach (object oParameter in parameterCollection)
                    {
                        mobj_SqlCommand.Parameters.Add(oParameter);
                    }
                    parameterCollection.Clear();
                }

                mobj_SqlConnection.Open();
                object sqlReturn = mobj_SqlCommand.ExecuteScalar();
                if (sqlReturn != null)
                {
                    Int32 iTemp = defaultValue;
                    if (!Int32.TryParse(sqlReturn.ToString(), out iTemp))
                    {
                        iTemp = defaultValue;
                    }
                    returnVal = iTemp;
                }
                return returnVal;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }

        ///<summary>
        ///Get list object by SQL command
        ///</summary>
        ///<param name="objectName">Object name</param>
        ///<param name="sqlCommand">SQL command</param>
        public List<T> GetListObject<T>(string sqlCommand)
        {
            List<T> list = new List<T>();
            T obj = default(T);

            mobj_SqlConnection = new SqlConnection(ConnectionString);
            mobj_SqlConnection.Open();
            mobj_SqlCommand = new SqlCommand(sqlCommand, mobj_SqlConnection);
            SqlDataReader sqlDr = mobj_SqlCommand.ExecuteReader();
            try
            {
                while (sqlDr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo pro_Info in obj.GetType().GetProperties())
                    {
                        pro_Info.SetValue(obj, Convert.ChangeType(sqlDr[pro_Info.Name], pro_Info.PropertyType), null);
                    }
                    list.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlDr.Close();
                CloseConnection();
            }
            return list;
        }
        
        ///<summary>
        ///Get list object by stored procedure
        ///</summary>
        ///<param name="objectName">Object name</param>
        ///<param name="procedureName">Stored procedure name</param>
        ///<param name="parameterCollection">SQL parameter list (nullable)</param>
        public List<T> GetListObject<T>(string procedureName,
            System.Collections.ArrayList parameterCollection)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            try
            {
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand(procedureName, mobj_SqlConnection);
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                if (parameterCollection != null)
                {
                    foreach (object oParameter in parameterCollection)
                    {
                        mobj_SqlCommand.Parameters.Add(oParameter);
                    }
                    parameterCollection.Clear();
                }

                mobj_SqlConnection.Open();
                SqlDataReader sqlDr = mobj_SqlCommand.ExecuteReader();
                try
                {
                    while (sqlDr.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo pro_Info in obj.GetType().GetProperties())
                        {
                            pro_Info.SetValue(obj, Convert.ChangeType(sqlDr[pro_Info.Name], pro_Info.PropertyType), null);
                        }
                        list.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlDr.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return list;
        }

        ///<summary>
        ///Get list object by stored procedure and map based on NameValueCollection
        ///</summary>
        ///<param name="objectName">Object name</param>
        ///<param name="procedureName">Stored procedure name</param>
        ///<param name="parameterCollection">SQL parameter list (nullable)</param>
        public List<object> GetListObject(Type objType, string procedureName, System.Collections.ArrayList parameterCollection,
            NameValueCollection mapField = null, CallBackType fnCallback = null)
        {
            if (objType == null || !objType.IsClass)
            {
                return null;
            }
            List<object> oListResult = null;

            try
            {
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand(procedureName, mobj_SqlConnection);
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                if (parameterCollection != null)
                {
                    foreach (object oParameter in parameterCollection)
                    {
                        mobj_SqlCommand.Parameters.Add(oParameter);
                    }
                    parameterCollection.Clear();
                }

                mobj_SqlConnection.Open();
                SqlDataReader sqlDr = mobj_SqlCommand.ExecuteReader();
                List<string> oListField = new List<string>();
                try
                {
                    string sField;
                    object oData;
                    Boolean bMappedField = false;

                    oListResult = new List<object>();
                    while (sqlDr.Read())
                    {
                        // Add all column into List to detect columns
                        if (oListField.Count == 0)
                        {
                            for (int i = 0; i < sqlDr.FieldCount; i++)
                            {
                                oListField.Add(sqlDr.GetName(i));
                            }
                        }

                        object oResult = Activator.CreateInstance(objType);

                        foreach (System.Reflection.PropertyInfo pro_Info in oResult.GetType().GetProperties())
                        {
                            bMappedField = false;
                            sField = pro_Info.Name;
                            if (mapField != null && !string.IsNullOrWhiteSpace(mapField[sField]))
                            {
                                sField = mapField[sField];
                                bMappedField = true;
                            }

                            if (oListField.Contains(sField, StringComparer.InvariantCultureIgnoreCase))
                            {
                                // Call function format
                                if (fnCallback != null)
                                {
                                    oData = fnCallback(pro_Info.Name, sqlDr[sField], (bMappedField == true) ? sField : null);
                                }
                                else
                                {
                                    oData = sqlDr[sField];
                                }

                                pro_Info.SetValue(oResult, Convert.ChangeType(oData.ToString(), pro_Info.PropertyType), null);
                            }
                            else
                            {
                                pro_Info.SetValue(oResult, null, null);
                            }
                        }

                        oListResult.Add(oResult);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlDr.Close();
                    mobj_SqlCommand.Clone();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return oListResult;
        }

        ///<summary>
        ///Get list CODynamic by stored procedure and map based on NameValueCollection
        ///</summary>
        ///<param name="objectName">Object name</param>
        ///<param name="procedureName">Stored procedure name</param>
        ///<param name="parameterCollection">SQL parameter list (nullable)</param>
        public List<CODynamic> GetListDynamicObject(string procedureName,
          System.Collections.ArrayList parameterCollection,
          NameValueCollection mapField, CallBackType fnCallback = null)
        {
            List<CODynamic> oListResult = null;

            try
            {
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand(procedureName, mobj_SqlConnection);
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.CommandType = CommandType.StoredProcedure;

                if (parameterCollection != null)
                {
                    foreach (object oParameter in parameterCollection)
                    {
                        mobj_SqlCommand.Parameters.Add(oParameter);
                    }
                    parameterCollection.Clear();
                }

                mobj_SqlConnection.Open();
                System.Data.SqlClient.SqlDataReader sqlDr = mobj_SqlCommand.ExecuteReader();
                string sField;
                object oData;
                Boolean bMappedField = false;
                CODynamic oObject = null;
                List<string> oListField = new List<string>();
                try
                {
                    oListResult = new List<CODynamic>();
                    while (sqlDr.Read())
                    {
                        // Add all column into List to detect columns
                        if (oListField.Count == 0)
                        {
                            for (int i = 0; i < sqlDr.FieldCount; i++)
                            {
                                oListField.Add(sqlDr.GetName(i));
                            }
                        }

                        oObject = new CODynamic();
                        foreach (string sMapField in mapField.Keys)
                        {
                            bMappedField = false;
                            sField = sMapField;
                            if (mapField[sField] != null && !string.IsNullOrWhiteSpace(mapField[sField]))
                            {
                                sField = mapField[sField];
                                bMappedField = true;
                            }

                            if (oListField.Contains(sField, StringComparer.InvariantCultureIgnoreCase))
                            {
                                // Call function format
                                if (fnCallback != null)
                                {
                                    oData = fnCallback(sMapField, sqlDr[sField], (bMappedField == true) ? sField : null);
                                }
                                else
                                {
                                    oData = sqlDr[sField];
                                }
                                oObject.SetData(sMapField, oData);
                            }
                            else
                            {
                                oObject.SetData(sMapField, null);
                            }
                        }
                        oListResult.Add(oObject);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlDr.Close();
                    mobj_SqlCommand.Clone();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return oListResult;

        }

        //public int ExectuteTransaction(List<Transaction> list_trans)
        //{
        //    int iRollBack = 0, iResult = 0;

        //    mobj_SqlConnection = new SqlConnection(ConnectionString);
        //    try
        //    {
        //        //var newList = list.FindAll(s => s.Equals("match"));

        //        IEnumerable<Transaction> list_parents = from tran in list_trans
        //                                                where tran.parentID == 0
        //                                                select tran;
        //        mobj_SqlConnection.Open();
        //        SqlTransaction mobj_SqlTransaction = mobj_SqlConnection.BeginTransaction();
        //        foreach (Transaction list_parent in list_parents)
        //        {
        //            SqlCommand mobj_SqlCommand = new SqlCommand();
        //            mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
        //            mobj_SqlCommand.CommandText = list_parent.storeName;
        //            mobj_SqlCommand.CommandType = CommandType.StoredProcedure;
        //            foreach (object obj_Param in list_parent.listParam)
        //            {
        //                SqlParameter pSource = (SqlParameter)obj_Param;
        //                SqlParameter sql_Param = new SqlParameter(pSource.ParameterName, pSource.SqlDbType, pSource.Size);
        //                sql_Param.Value = pSource.SourceColumn;
        //                mobj_SqlCommand.Parameters.Add(sql_Param);
        //            }
        //            list_parent.listParam.Clear();
        //            mobj_SqlCommand.Connection = mobj_SqlConnection;
        //            mobj_SqlCommand.Transaction = mobj_SqlTransaction;
        //            try
        //            {
        //                object oResult = mobj_SqlCommand.ExecuteScalar();
        //                if (oResult != null)
        //                {
        //                    iResult = (Int32)oResult;
        //                    if (iResult > 0)
        //                    {
        //                        ExectuteTransactionChild(iResult, list_trans, list_parent.ID, mobj_SqlTransaction, mobj_SqlConnection, ref iRollBack);
        //                        if (iRollBack == -1)
        //                            break;
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                mobj_SqlTransaction.Rollback();
        //                iRollBack = -1;
        //                break;
        //            }
        //        }
        //        if (iRollBack == 0)
        //        {
        //            mobj_SqlTransaction.Commit();
        //            iRollBack = 1;
        //        }
        //        mobj_SqlTransaction.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //        iRollBack = -1;
        //    }
        //    finally
        //    {
        //        mobj_SqlConnection.Close();
        //    }
        //    return iRollBack;
        //}

        //public void ExectuteTransactionChild(int iParent, List<Transaction> list_trans, int parent_ID, SqlTransaction mobj_sqlTransaction, SqlConnection mobj_Connection, ref int iRollBack)
        //{
        //    // iParent is result return from Store Procedure of Parent Table
        //    string str_child = string.Empty;
        //    int mint_CommandTimeout = 30;
        //    IEnumerable<Transaction> tran_childs = from tran in list_trans
        //                                           where tran.parentID == parent_ID
        //                                           select tran;
        //    foreach (Transaction tran_child in tran_childs)
        //    {
        //        try
        //        {
        //            SqlCommand mobj_sqlCommand = new SqlCommand();
        //            mobj_sqlCommand.CommandTimeout = mint_CommandTimeout;
        //            mobj_sqlCommand.CommandText = tran_child.storeName;
        //            mobj_sqlCommand.CommandType = CommandType.StoredProcedure;
        //            mobj_sqlCommand.Connection = mobj_Connection;
        //            foreach (object obj_Param in tran_child.listParam)
        //            {
        //                SqlParameter pSource = (SqlParameter)obj_Param;
        //                SqlParameter sql_Param = new SqlParameter(pSource.ParameterName, pSource.SqlDbType, pSource.Size);
        //                if (tran_child.foreginKey == pSource.ParameterName)
        //                {
        //                    sql_Param.Value = iParent;
        //                }
        //                else
        //                {
        //                    sql_Param.Value = pSource.SourceColumn;
        //                }
        //                mobj_sqlCommand.Parameters.Add(sql_Param);
        //            }
        //            tran_child.listParam.Clear();
        //            mobj_sqlCommand.Connection = mobj_Connection;
        //            mobj_sqlCommand.Transaction = mobj_sqlTransaction;
        //            try
        //            {
        //                object oResult = mobj_sqlCommand.ExecuteScalar();
        //                if (oResult != null)
        //                {
        //                    int iResult = (int)oResult;
        //                    if (iResult > 0)
        //                    {
        //                        ExectuteTransactionChild(iResult, list_trans, tran_child.ID, mobj_sqlTransaction, mobj_Connection, ref iRollBack);
        //                        if (iRollBack == -1) break;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                mobj_sqlTransaction.Rollback();
        //                str_child = ex.ToString();
        //                iRollBack = -1;
        //                break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            mobj_sqlTransaction.Rollback();
        //            str_child = ex.ToString();
        //            iRollBack = -1;
        //            break;
        //        }
        //    }
        //}

        public class CODynamic : DynamicObject
        {
            private IDictionary<string, object> _values;
            // This property returns the number of elements
            // in the inner dictionary.
            public int Count
            {
                get
                {
                    return _values.Count;
                }
            }

            public CODynamic(Dictionary<string, object> _dictionary = null)
            {
                _values = _dictionary ?? new Dictionary<string, object>();
            }
            public void SetData(string sKey, object oValue)
            {
                _values[sKey] = oValue;
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _values.Keys;
            }

            // If you try to get a value of a property 
            // not defined in the class, this method is called.
            public override bool TryGetMember(
                GetMemberBinder binder, out object result)
            {
                // If the property name is found in a dictionary,
                // set the result parameter to the property value and return true.
                // Otherwise, return false.
                if (_values.ContainsKey(binder.Name))
                {
                    result = _values[binder.Name];
                    return true;
                }
                result = null;
                return false;
            }

            // If you try to set a value of a property that is
            // not defined in the class, this method is called.
            public override bool TrySetMember(
                SetMemberBinder binder, object value)
            {
                // Converting the property name to lowercase
                // so that property names become case-insensitive.
                _values[binder.Name] = value;

                // You can always add a value to a dictionary,
                // so this method always returns true.
                return true;
            }
        }

        public string GetDataByCommand(string sqlCommand, string fieldName)
        {
            try
            {
                mstr_ConnectionString = ConnectionString;

                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand();
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.Connection = mobj_SqlConnection;

                mobj_SqlCommand.CommandText = sqlCommand;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;

                mobj_SqlConnection.Open();

                SqlDataReader reader = mobj_SqlCommand.ExecuteReader();
                string svalue = "";
                if (reader.Read())
                {
                    svalue = reader[fieldName].ToString();
                }
                else
                {
                    svalue = null;
                }
                return svalue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public string GetDataWithMultiColumnByCommand(string sqlCommand, string fieldCollection)
        {
            try
            {
                mstr_ConnectionString = ConnectionString;
                mobj_SqlConnection = new SqlConnection(mstr_ConnectionString);
                mobj_SqlCommand = new SqlCommand();
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlCommand.Connection = mobj_SqlConnection;
                mobj_SqlCommand.CommandText = sqlCommand;
                mobj_SqlCommand.CommandTimeout = mint_CommandTimeout;
                mobj_SqlConnection.Open();

                SqlDataReader reader = mobj_SqlCommand.ExecuteReader();
                string svalue = "";
                string[] aColumnName = fieldCollection.Split(";".ToCharArray());

                if (reader.Read())
                {
                    for (int i = 0; i < aColumnName.Length; i++)
                    {
                        if (reader[aColumnName[i]] != System.DBNull.Value)
                        {
                            svalue += reader[aColumnName[i]].ToString() + ";";
                        }
                        else
                        {
                            svalue += "0" + ";";
                        }
                    }
                }
                else
                {
                    svalue = "0;0";
                }
                return svalue.Substring(0, svalue.Length - 1);
            }
            catch (SqlException)
            {
                return "0;0";
            }
            finally
            {
                CloseConnection();
            }
        }


        public static DataTable CreateDataTable(DataColumn[] typeColumns, string dataTableName = "tbl_Temp")
        {
            if (string.IsNullOrWhiteSpace(dataTableName))
            {
                dataTableName = "tbl_Temp";
            }

            DataTable dt = new DataTable(dataTableName);

            for (int i = 0; i < typeColumns.Length; i++)
            {
                dt.Columns.Add(typeColumns[i]);
            }

            return dt;
        }

        public static DataTable CreateDataTable(IDictionary<string, Type> typeColumns, string dataTableName = "tbl_Temp")
        {
            if (string.IsNullOrWhiteSpace(dataTableName))
            {
                dataTableName = "tbl_Temp";
            }

            DataTable dt = new DataTable(dataTableName);

            foreach (var typeColumn in typeColumns)
            {
                dt.Columns.Add(typeColumn.Key, typeColumn.Value);
            }

            return dt;
        }
    }
}