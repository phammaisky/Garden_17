using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GardenCrm.Helpers
{
    public class DataHandler
    {
        private string _dbConnStr;

        public string DBConnStr
        {
            get
            {
                return this._dbConnStr;
            }
            set
            {
                this._dbConnStr = value;
            }
        }

        public DataHandler()
        {
            this.DBConnStr = ConfigurationManager.ConnectionStrings[DBConnStr].ConnectionString;
        }

        public DataHandler(string keyDbConnStr)
        {
            this.DBConnStr = ConfigurationManager.ConnectionStrings[DBConnStr].ConnectionString;
        }

        public DataHandler(string keyDbConnStr, bool isCrypto, string crytoKey)
        {
            if (isCrypto)
                this.DBConnStr = ConfigurationManager.ConnectionStrings[DBConnStr].ConnectionString;
            else
                this.DBConnStr = ConfigurationManager.ConnectionStrings[DBConnStr].ConnectionString;
        }

        public DataSet GetSpDataSet(
          string procedure,
          List<SqlParameter> sqlParams,
          string RetCode,
          ref int ReturnValue)
        {
            DataSet dataSet = (DataSet)null;
            try
            {
                if (string.IsNullOrEmpty(procedure))
                    throw new ArgumentNullException(nameof(procedure));
                using (SqlConnection selectConnection = new SqlConnection(this.DBConnStr))
                {
                    selectConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(procedure, selectConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParams != null && sqlParams.Count > 0)
                    {
                        foreach (SqlParameter sqlParam in sqlParams)
                            sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
                    }
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);
                    ReturnValue = Convert.ToInt32(sqlDataAdapter.SelectCommand.Parameters[RetCode].Value);
                }
            }
            catch (SystemException ex)
            {
            }
            catch (Exception ex)
            {
            }
            return dataSet;
        }

        public DataTable GetSpDataTable(
          string procedure,
          List<SqlParameter> sqlParams,
          string RetCode,
          ref int ReturnValue)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                if (string.IsNullOrEmpty(procedure))
                    throw new ArgumentNullException(nameof(procedure));
                using (SqlConnection selectConnection = new SqlConnection(this.DBConnStr))
                {
                    selectConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(procedure, selectConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParams != null && sqlParams.Count > 0)
                    {
                        foreach (SqlParameter sqlParam in sqlParams)
                            sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
                    }
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    ReturnValue = Convert.ToInt32(sqlDataAdapter.SelectCommand.Parameters[RetCode].Value);
                }
            }
            catch (SystemException ex)
            {
            }
            catch (Exception ex)
            {
            }
            return dataTable;
        }

        public DataSet GetSpDataSet(string procedure, List<SqlParameter> sqlParams)
        {
            DataSet dataSet = (DataSet)null;
            try
            {
                if (string.IsNullOrEmpty(procedure))
                    throw new ArgumentNullException(nameof(procedure));
                using (SqlConnection selectConnection = new SqlConnection(this.DBConnStr))
                {
                    selectConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(procedure, selectConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParams != null && sqlParams.Count > 0)
                    {
                        foreach (SqlParameter sqlParam in sqlParams)
                            sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
                    }
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);
                }
            }
            catch (SystemException ex)
            {
            }
            catch (Exception ex)
            {
            }
            return dataSet;
        }

        public DataTable GetSpDataTable(string procedure, List<SqlParameter> sqlParams)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                if (string.IsNullOrEmpty(procedure))
                    throw new ArgumentNullException(nameof(procedure));
                using (SqlConnection selectConnection = new SqlConnection(this.DBConnStr))
                {
                    selectConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(procedure, selectConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParams != null && sqlParams.Count > 0)
                    {
                        foreach (SqlParameter sqlParam in sqlParams)
                            sqlDataAdapter.SelectCommand.Parameters.Add(sqlParam);
                    }
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                }
            }
            catch (SystemException ex)
            {
            }
            catch (Exception ex)
            {
            }
            return dataTable;
        }

        public int ExcuteSp(string procedure, List<SqlParameter> sqlParams, string RetCode)
        {
            int num = -1;
            try
            {
                if (string.IsNullOrEmpty(procedure))
                    throw new ArgumentNullException(nameof(procedure));
                using (SqlConnection connection = new SqlConnection(this.DBConnStr))
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand(procedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParams != null && sqlParams.Count > 0)
                    {
                        foreach (SqlParameter sqlParam in sqlParams)
                            sqlCommand.Parameters.Add(sqlParam);
                    }
                    sqlCommand.ExecuteNonQuery();
                    num = Convert.ToInt32(sqlCommand.Parameters[RetCode].Value);
                }
            }
            catch (SystemException ex)
            {
            }
            catch (Exception ex)
            {
            }
            return num;
        }
    }
}