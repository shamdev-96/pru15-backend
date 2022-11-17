//using System;
//using System.Data;
//using System.Reflection;

//namespace PRU15_Shared
//{
//    public class DataAccess
//    {
//        #region Properties
//        private string ConnectionString { get; set; }
//        #endregion

//        #region Constant
//        private const string CONST_DEFAULT_DB_CONNECTION_STRING = "Data Source=localhost;Initial Catalog=RR_Backend;Integrated Security=True";
//        #endregion

//        #region Constructor
//        public DataAccess()
//        {
//            this.ConnectionString = CONST_DEFAULT_DB_CONNECTION_STRING;
//        }

//        public DataAccess(string strConnectionString)
//        {
//            this.ConnectionString = strConnectionString;
//        }
//        #endregion

//        #region Accessible Method
//        public long ExecuteSqlQuery(string strSqlQuery, out DataSet dsResult, out string strExceptionMessage, int iTimeOut = 500000)
//        {
//            return ExecuteSqlQueryOrSP(strSqlQuery, out dsResult, out strExceptionMessage, false, null, false, null, iTimeOut);
//        }

//        public long ExecuteStoreProcedure(string strStoreProcName, out DataSet dsResult, out string strExceptionMessage, int iTimeOut = 500000)
//        {
//            return ExecuteSqlQueryOrSP(strStoreProcName, out dsResult, out strExceptionMessage, true, null, false, null, iTimeOut);
//        }
//        public long ExecuteStoreProcedureNonQuery(string strStoreProcName, DataTable dsTVP, out string strExceptionMessage, int iTimeOut = 500000)
//        {
//            DataSet dsResult;
//            dsResult = null;
//            return ExecuteSqlQueryOrSP(strStoreProcName, out dsResult, out strExceptionMessage, true, dsTVP, false, null, iTimeOut);
//        }
//        public long ExecuteStoreProcedureWithParameters(string strStoreProcName, DataTable dtblStoreProcedureParameters, out DataSet dsResult, out string strExceptionMessage, int iTimeOut = 500000)
//        {
//            return ExecuteSqlQueryOrSP(strStoreProcName, out dsResult, out strExceptionMessage, true, dtblStoreProcedureParameters, false, null, iTimeOut);
//        }

//        public long ExecuteStoreProcedureWithTVP(string strStoreProcName, DataSet dsTVP, out DataSet dsResult, out string strExceptionMessage, int iTimeOut = 500000)
//        {
//            return ExecuteSqlQueryOrSP(strStoreProcName, out dsResult, out strExceptionMessage, true, null, true, dsTVP, iTimeOut);
//        }
//        #endregion

//        #region Helpers
//        private long ExecuteSqlQueryOrSP(string strSqlQueryOrSpName, out DataSet dsResult, out string strExceptionMessage, bool blnIsStoreProcedure = false, DataTable dtblStoreProcedureParameters = null, bool blnIsTVP = false, DataSet dsTVP = null, int iTimeOut = 500000)
//        {
//            long lngStatus = 0;

//            dsResult = null;
//            strExceptionMessage = string.Empty;
//            try
//            {
//                #region Data checking
//                if (string.IsNullOrWhiteSpace(strSqlQueryOrSpName))
//                    throw new Exception("SQL Query/Store Procedure is null or empty.");
//                if (string.IsNullOrWhiteSpace(this.ConnectionString))
//                    throw new Exception("Connection string is null or empty.");
//                #endregion

//                dsResult = new DataSet();

//                SqlConnection sqlConn = new SqlConnection(this.ConnectionString);
//                SqlCommand cmd = new SqlCommand(strSqlQueryOrSpName, sqlConn);
//                if (blnIsStoreProcedure)
//                {
//                    cmd = new SqlCommand(strSqlQueryOrSpName, sqlConn) { CommandType = CommandType.StoredProcedure };

//                    if (blnIsTVP)
//                    {
//                        if (dsTVP != null
//                            && dsTVP.Tables != null
//                            && dsTVP.Tables.Count > 0)
//                        {
//                            foreach (DataTable dtbl in dsTVP.Tables)
//                            {
//                                SqlParameter sParameter = new SqlParameter("@" + dtbl.TableName, dtbl);
//                                cmd.Parameters.Add(sParameter);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        if (dtblStoreProcedureParameters != null
//                            && dtblStoreProcedureParameters.Rows != null
//                            && dtblStoreProcedureParameters.Rows.Count > 0)
//                        {
//                            foreach (DataColumn dc in dtblStoreProcedureParameters.Columns)
//                            {
//                                SqlParameter sParameter = new SqlParameter("@" + dc.ColumnName, dtblStoreProcedureParameters.Rows[0][dc.ColumnName]);
//                                cmd.Parameters.Add(sParameter);
//                            }
//                        }
//                    }
//                }

//                using (sqlConn)
//                {
//                    using (cmd)
//                    {
//                        cmd.CommandTimeout = iTimeOut;
//                        SqlDataAdapter da = new SqlDataAdapter(cmd);
//                        da.Fill(dsResult);
//                    }
//                }
//            }
//            catch (SqlException sqlEx)
//            {
//                lngStatus = sqlEx.Number;

//                if (lngStatus == -2)
//                    strExceptionMessage = string.Format("SQL Connection Timeout.");
//                else
//                    strExceptionMessage = string.Format("SQL Exception occurred. Message = {0}", sqlEx.Message);
//            }
//            catch (Exception ex)
//            {
//                lngStatus = -99;
//                strExceptionMessage = string.Format("Exception occurred. Message = {0}", ex.Message);
//            }

//            return lngStatus;
//        }

//        public static DataTable GetDataTableXLSX(string strFilePath, string strSQL)
//        {
//            string curNamespaceAndMethod = MethodInfo.GetCurrentMethod().DeclaringType.FullName + "." + MethodInfo.GetCurrentMethod().Name;
//            DataTable dtOutput = new DataTable();

//            if (string.IsNullOrEmpty(strFilePath))
//            {
//                throw new Exception(string.Format("Error source = {0}, Exception message = {1}", curNamespaceAndMethod, "Excel file path is null."));
//            }
//            if (string.IsNullOrEmpty(strSQL))
//            {
//                throw new Exception(string.Format("Error source = {0}, Exception message = {1}", curNamespaceAndMethod, "SQL query is null."));
//            }
//            string strConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=YES;TypeGuessRows=0;ImportMixedTypes=Text\"";
//            using (var conn = new OleDbConnection(strConnectionString))
//            {
//                conn.Open();
//                using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSQL, conn))
//                {
//                    adapter.Fill(dtOutput);
//                }
//                conn.Close();
//            }
//            return dtOutput;
//        }
//        #endregion
//    }
//}
