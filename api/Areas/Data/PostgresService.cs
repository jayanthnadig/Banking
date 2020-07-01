using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Data {
  internal class PostgresService {
    public List<KeyValuePair<string, object>> Parameters;
    private static readonly string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private readonly string requestId = "";

    public PostgresService() {
      Parameters = new List<KeyValuePair<string, object>>();
    }

    public PostgresService(string requestId) : this() {
      this.requestId = requestId;
    }

    internal void AddDateParam(string paramName, DateTime? paramValue) {
      DateTime value = (paramValue == null) ? DateTime.MinValue : (DateTime) paramValue;

      Parameters.Add(new KeyValuePair<string, object>(paramName, value));
    }

    internal void AddDateParamWithNull(string paramName, DateTime? paramValue) {
      if (paramValue == null) {
        Parameters.Add(new KeyValuePair<string, object>(paramName, DBNull.Value));
      }
      else {
        Parameters.Add(new KeyValuePair<string, object>(paramName, ((DateTime) paramValue).GetSqlDate()));
      }
    }

    internal void AddIntParam(string paramName, object paramValue) {
      if (paramValue == null || string.IsNullOrWhiteSpace(paramValue.ToString())) {
        paramValue = System.DBNull.Value;
      }

      AddParam(paramName, paramValue);
    }

    internal void AddLikeParam(string paramName, string paramValue) {
      Parameters.Add(new KeyValuePair<string, object>(paramName, Utility.AddWildCard(paramValue)));
    }

    internal void AddParam(string paramName, object paramValue) {
      Parameters.Add(new KeyValuePair<string, object>(paramName, paramValue));
    }

    internal void ClearParams() {
      Parameters.Clear();
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    internal async Task ExecuteProcedureAsync(string connString, string proc) {
      const string methodName = "ExecuteProcedureAsync";
      LogCall(methodName, proc);
      try {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        using (NpgsqlCommand cmd = conn.CreateCommand()) {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandText = proc;

          foreach (KeyValuePair<string, object> item in Parameters) {
            cmd.Parameters.AddWithValue(item.Key, item.Value);
          }

          conn.Open();

          await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
      }
      catch (Exception ex) {
        LoggerService.LogException(requestId, className, methodName, new NpgsqlException(GetSqlWithParams(proc), ex));
        throw;
      }
      finally {
        ClearParams();
        LoggerService.LogMethodEnd(requestId, className, methodName);
      }
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    internal void ExecuteProcedureNonQuery(string connString, string proc) {
      const string methodName = "ExecuteProcedureNonQuery";
      LogCall(methodName, proc);
      try {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString)) {
          using (NpgsqlCommand cmd = conn.CreateCommand()) {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = proc;

            foreach (KeyValuePair<string, object> item in Parameters) {
              cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            conn.Open();

            cmd.ExecuteNonQuery();
          }
        }
      }
      catch (Exception ex) {
        LoggerService.LogException(requestId, className, methodName, new NpgsqlException(GetSqlWithParams(proc), ex));
        throw;
      }
      finally {
        ClearParams();
        LoggerService.LogMethodEnd(requestId, className, methodName);
      }
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    internal NpgsqlDataReader ExecuteProcedureReturnReader(string connString, string proc) {
      const string methodName = "ExecuteProcedureReturnReader";
      LogCall(methodName, proc);

      NpgsqlConnection conn = new NpgsqlConnection(connString);

      try {
        using (NpgsqlCommand cmd = conn.CreateCommand()) {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandText = proc;

          foreach (KeyValuePair<string, object> item in Parameters) {
            cmd.Parameters.AddWithValue(item.Key, item.Value);
          }

          conn.Open();

          return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
      }
      catch (Exception ex) {
        conn.Close();
        LoggerService.LogException(requestId, className, methodName, new NpgsqlException(GetSqlWithParams(proc), ex));
        throw;
      }
      finally {
        ClearParams();
        LoggerService.LogMethodEnd(requestId, className, methodName);
      }
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    internal object ExecuteProcedureReturnScalar(string connString, string proc) {
      const string methodName = "ExecuteProcedureReturnScalar";
      LogCall(methodName, proc);
      try {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString)) {
          using (NpgsqlCommand cmd = conn.CreateCommand()) {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = proc;

            foreach (KeyValuePair<string, object> item in Parameters) {
              cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            conn.Open();

            return cmd.ExecuteScalar();
          }
        }
      }
      catch (Exception ex) {
        LoggerService.LogException(requestId, className, methodName, new NpgsqlException(GetSqlWithParams(proc), ex));
        throw;
      }
      finally {
        ClearParams();
        LoggerService.LogMethodEnd(requestId, className, methodName);
      }
    }

    internal T ExecuteProcedureReturnScalar<T>(string connString, string proc) {
      object val = ExecuteProcedureReturnScalar(connString, proc);

      if (val == null || val == DBNull.Value) {
        return default(T);
      }
      else {
        return (T) Convert.ChangeType(val, typeof(T));
      }
    }

    internal int ExecuteSqlNonQuery(string sql) {
      return ExecuteSqlNonQuery(Utility.ConnString, sql);
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    internal int ExecuteSqlNonQuery(string connString, string sql) {
      const string methodName = "ExecuteSqlNonQuery";
      LogCall(methodName, sql);
      int recordsAffected = 0;

      try {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString)) {
          using (NpgsqlCommand cmd = conn.CreateCommand()) {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            conn.Open();

            foreach (KeyValuePair<string, object> item in Parameters) {
              cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            recordsAffected = cmd.ExecuteNonQuery();
          }
        }
      }
      catch (Exception ex) {
        LoggerService.LogException(requestId, className, methodName, new NpgsqlException(GetSqlWithParams(sql), ex));
        throw;
      }
      finally {
        ClearParams();
        LoggerService.LogMethodEnd(requestId, className, methodName);
      }
      return recordsAffected;
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    internal NpgsqlDataReader ExecuteSqlReturnReader(string connString, string sql) {
      const string methodName = "ExecuteSqlReturnReader";
      LogCall(methodName, sql);
      NpgsqlConnection conn = new NpgsqlConnection(connString);

      try {
        using (NpgsqlCommand cmd = conn.CreateCommand()) {
          cmd.CommandType = CommandType.Text;
          cmd.CommandText = sql;

          foreach (KeyValuePair<string, object> item in Parameters) {
            cmd.Parameters.AddWithValue(item.Key, item.Value);
          }

          conn.Open();

          return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
      }
      catch (Exception ex) {
        conn.Close();
        LoggerService.LogException(requestId, className, methodName, new NpgsqlException(GetSqlWithParams(sql), ex));
        // Logger.LogException(className, methodName, ex);
        throw;
      }
      finally {
        ClearParams();
        LoggerService.LogMethodEnd(requestId, className, methodName);
      }
    }

    [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    internal object ExecuteSqlReturnScalar(string connString, string sql) {
      const string methodName = "ExecuteSqlReturnScalar";
      LogCall(methodName, sql);

      try {
        using (NpgsqlConnection conn = new NpgsqlConnection(connString)) {
          using (NpgsqlCommand cmd = conn.CreateCommand()) {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            conn.Open();

            foreach (KeyValuePair<string, object> item in Parameters) {
              cmd.Parameters.AddWithValue(item.Key, item.Value);
            }

            return cmd.ExecuteScalar();
          }
        }
      }
      catch (Exception ex) {
        LoggerService.LogException(requestId, className, methodName, new NpgsqlException(GetSqlWithParams(sql), ex));
        throw;
      }
      finally {
        ClearParams();
        LoggerService.LogMethodEnd(requestId, className, methodName);
      }
    }

    internal T ExecuteSqlReturnScalar<T>(string connString, string sql) {
      object val = ExecuteSqlReturnScalar(connString, sql);

      if (val == null || val == DBNull.Value) {
        return default(T);
      }
      else {
        return (T) Convert.ChangeType(val, typeof(T));
      }
    }

    internal DateTime GetDbTime(string connStr) {
      const string methodName = "GetDbTime";
      LoggerService.LogMethodStart(requestId, className, methodName);

      const string sql = "select current_timestamp();";

      DateTime returnValue = DateTime.Parse(ExecuteSqlReturnScalar(connStr, sql).ToString());

      LoggerService.LogMethodEnd(requestId, className, methodName);
      return returnValue;
    }

    internal int GetIntWithDefault(object value) {
      if (int.TryParse(value.ToString(), out int returnValue)) {
        return returnValue;
      }
      else {
        return 0;
      }
    }

    internal string TestConnection() {
      try {
        ExecuteSqlNonQuery("select 1");
        return "Success!";
      }
      catch (Exception ex) {
        return ex.Message;
      }
    }

    private string GetSqlWithParams(string sql) {
      StringBuilder sb = new StringBuilder();

      sb.AppendLine(sql);

      foreach (KeyValuePair<string, object> item in Parameters) {
        sb.Append(item.Key).Append(':').AppendLine((item.Value == null) ? "null" : item.Value.ToString());
      }

      return sb.ToString();
    }

    private void LogCall(string methodName, string sql) {
      LoggerService.LogMethodStart(requestId, className, methodName);

      // Logger.LogInfo(className, methodName, DateTime.Now.Ticks.ToString() + " : " + GetSqlWithParams(sql));
      LoggerService.LogDebug(requestId, className, methodName, GetSqlWithParams(sql));
    }
  }
}
