using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Data
{
    internal class SqlServerService
    {
        private static readonly string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        private List<SqlParameter> parameters;
        private string requestId = "";

        public SqlServerService()
        {
            parameters = new List<SqlParameter>();
        }

        public SqlServerService(string _requestId) : this()
        {
            requestId = _requestId;
        }

        public SqlServerService(List<KeyValuePair<string, object>> paramColl) : this()
        {
            parameters = ConvertParameters(paramColl);
        }

        internal static List<SqlParameter> ConvertParameters(List<KeyValuePair<string, object>> paramColl)
        {
            List<SqlParameter> returnValue = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in paramColl)
            {
                returnValue.Add(new SqlParameter { ParameterName = item.Key, Value = item.Value });
            }

            return returnValue;
        }

        internal static SqlParameter GetOutputParam(string paramName, int size)
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                Direction = ParameterDirection.Output,
                Size = size
            };
        }

        internal static object GetOutputParamValue(SqlParameterCollection sqlParameters, string paramName)
        {
            for (int i = 0; i < sqlParameters.Count; i++)
            {
                SqlParameter param = sqlParameters[i];

                if (string.Equals(param.ParameterName, paramName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return param.Value;
                }
            }
            return null;
        }

        internal object GetOutputParamValue(string paramName)
        {
            foreach (var item in parameters)
            {
                if (string.Equals(item.ParameterName, paramName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return item.Value;
                }
            }
            return null;
        }

        internal static SqlParameter GetParam(string paramName, object paramValue, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter
            {
                ParameterName = paramName,
                Value = paramValue,
                Direction = direction
            };
        }

        internal void AddDateParam(string paramName, DateTime? paramValue)
        {
            DateTime value = (paramValue == null) ? DateTime.MinValue : (DateTime) paramValue;

            AddParam(paramName, value);
        }

        internal void AddDateParamWithNull(string paramName, DateTime? paramValue)
        {
            if (paramValue == null)
            {
                AddParam(paramName, DBNull.Value);
            }
            else
            {
                AddParam(paramName, ((DateTime) paramValue).GetSqlDate());
            }
        }

        internal void AddIntParam(string paramName, object paramValue)
        {
            if (paramValue == null || string.IsNullOrWhiteSpace(paramValue.ToString()))
            {
                paramValue = DBNull.Value;
            }

            AddParam(paramName, paramValue);
        }

        internal void AddLikeParam(string paramName, string paramValue)
        {
            AddParam(paramName, Utility.AddWildCard(paramValue));
        }

        internal void AddOutputParam(string paramName, int size)
        {
            parameters.Add(SqlServerService.GetOutputParam(paramName, size));
        }

        internal void AddParam(SqlParameter sqlParameter)
        {
            parameters.Add(sqlParameter);
        }

        internal void AddParam(string paramName, object paramValue)
        {
            AddParam(paramName, paramValue, ParameterDirection.Input);
        }

        internal void AddParam(string paramName, object paramValue, ParameterDirection direction)
        {
            parameters.Add(new SqlParameter
            {
                ParameterName = paramName,
                Value = paramValue,
                Direction = direction
            });
        }

        internal void ClearParams()
        {
            parameters.Clear();
        }

        internal void ExecuteProcedureNonQuery(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureNonQuery";
            LogCall(methodName, proc);
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = proc;
                        AddParameters(cmd.Parameters, parameters);

                        conn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal async Task ExecuteProcedureNonQueryAsync(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureAsynchronously";
            LogCall(methodName, proc);
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc;

                    AddParameters(cmd.Parameters, parameters);

                    conn.Open();

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal DataTable ExecuteProcedureReturnDataTable(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureReturnDataTable";
            LogCall(methodName, proc);

            SqlConnection conn = new SqlConnection(connString);
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc;

                    AddParameters(cmd.Parameters, parameters);

                    conn.Open();

                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
                    {
                        sqlDataAdapter.Fill(dataTable);
                    }

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal DataSet ExecuteProcedureReturnDataSet(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureReturnDataTable";
            LogCall(methodName, proc);

            SqlConnection conn = new SqlConnection(connString);
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc;

                    AddParameters(cmd.Parameters, parameters);

                    conn.Open();

                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
                    {
                        sqlDataAdapter.Fill(dataSet);
                    }

                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal async Task<T> ExecuteProcedureReturnParamValueAsync<T>(string connString, string proc, string outputParamName)
        {
            const string methodName = "ExecuteProcedureReturnParamValue";
            LogCall(methodName, proc);
            object returnValue = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = proc;

                        AddParameters(cmd.Parameters, parameters);

                        conn.Open();

                        await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                        foreach (SqlParameter item in cmd.Parameters)
                        {
                            if (item.Direction == ParameterDirection.Output && string.Equals(item.ParameterName, outputParamName, StringComparison.OrdinalIgnoreCase))
                            {
                                returnValue = item.Value;
                            }
                        }
                    }
                }
                return GetDefault<T>(returnValue);
            }
            catch (Exception ex)
            {
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));

                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal SqlDataReader ExecuteProcedureReturnReader(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureReturnReader";
            LogCall(methodName, proc);

            SqlConnection conn = new SqlConnection(connString);

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc;

                    AddParameters(cmd.Parameters, parameters);

                    conn.Open();

                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal async Task<SqlDataReader> ExecuteProcedureReturnReaderAsync(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureReturnReaderAsync";
            LogCall(methodName, proc);

            SqlConnection conn = new SqlConnection(connString);

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc;

                    AddParameters(cmd.Parameters, parameters);

                    conn.Open();

                    return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal object ExecuteProcedureReturnScalar(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureReturnScalar";
            LogCall(methodName, proc);
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = proc;
                        AddParameters(cmd.Parameters, parameters);

                        conn.Open();

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal T ExecuteProcedureReturnScalar<T>(string connString, string proc)
        {
            object val = ExecuteProcedureReturnScalar(connString, proc);

            return GetDefault<T>(val);
        }

        internal async Task<T> ExecuteProcedureReturnScalarAsync<T>(string connString, string proc)
        {
            object val = await ExecuteProcedureReturnScalarAsync(connString, proc).ConfigureAwait(false);

            return GetDefault<T>(val);
        }

        internal async Task<object> ExecuteProcedureReturnScalarAsync(string connString, string proc)
        {
            const string methodName = "ExecuteProcedureReturnScalar";
            LogCall(methodName, proc);
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = proc;
                    AddParameters(cmd.Parameters, parameters);

                    conn.Open();

                    return await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(proc), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal int ExecuteSqlNonQuery(string sql)
        {
            return ExecuteSqlNonQuery(Utility.ConnString, sql);
        }

        internal int ExecuteSqlNonQuery(string connString, string sql)
        {
            const string methodName = "ExecuteSqlNonQuery";
            LogCall(methodName, sql);
            int recordsAffected = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        AddParameters(cmd.Parameters, parameters);
                        conn.Open();

                        recordsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(sql), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
            return recordsAffected;
        }

        internal SqlDataReader ExecuteSqlReturnReader(string connString, string sql)
        {
            const string methodName = "ExecuteSqlReturnReader";
            LogCall(methodName, sql);
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    AddParameters(cmd.Parameters, parameters);

                    conn.Open();

                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(sql), ex));
                // Logger.LogException(className, methodName, ex);
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal object ExecuteSqlReturnScalar(string connString, string sql)
        {
            const string methodName = "ExecuteSqlReturnScalar";
            LogCall(methodName, sql);

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        conn.Open();
                        AddParameters(cmd.Parameters, parameters);

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogException(requestId, className, methodName, new Exception(GetSqlWithParams(sql), ex));
                throw;
            }
            finally
            {
                ClearParams();
                LoggerService.LogMethodEnd(requestId, className, methodName);
            }
        }

        internal T ExecuteSqlReturnScalar<T>(string connString, string sql)
        {
            object val = ExecuteSqlReturnScalar(connString, sql);

            if (val == null || val == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T) Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
            }
        }

        internal DateTime GetDbTime(string connStr)
        {
            const string methodName = "GetDbTime";
            LoggerService.LogMethodStart(requestId, className, methodName);

            const string sql = "select current_timestamp();";

            DateTime returnValue = ExecuteSqlReturnScalar<DateTime>(connStr, sql);

            LoggerService.LogMethodEnd(requestId, className, methodName);
            return returnValue;
        }

        internal int GetIntWithDefault(object value)
        {
            if (int.TryParse(value.ToString(), out int returnValue))
            {
                return returnValue;
            }
            else
            {
                return 0;
            }
        }

        internal string TestConnection()
        {
            try
            {
                ExecuteSqlNonQuery("select 1");
                return "Success!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static void AddParameters(SqlParameterCollection parameters, List<SqlParameter> paramColl)
        {
            foreach (SqlParameter item in paramColl)
            {
                parameters.Add(item);
            }
        }

        private static T GetDefault<T>(object val)
        {
            if (val == null || val == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T) Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
            }
        }

        private string GetSqlWithParams(string sql)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(sql);

            //foreach (KeyValuePair<string, object> item in Parameters) {
            //  sb.Append(item.Key).Append(':').AppendLine((item.Value == null) ? "null" : item.Value.ToString());
            //}
            foreach (SqlParameter item in parameters)
            {
                sb.Append(item.ParameterName).Append(':').AppendLine((item.Value == null) ? "null" : item.Value.ToString());
            }

            return sb.ToString();
        }

        private void LogCall(string methodName, string sql)
        {
            LoggerService.LogMethodStart(requestId, className, methodName);

            // Logger.LogInfo(className, methodName, DateTime.Now.Ticks.ToString() + " : " + GetSqlWithParams(sql));
            LoggerService.LogDebug(requestId, className, methodName, GetSqlWithParams(sql));
        }
    }
}
