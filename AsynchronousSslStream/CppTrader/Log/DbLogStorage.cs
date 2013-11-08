using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.SqlClient;
using System.Data;

namespace Trader.Server.CppTrader.Log
{
    public class DbLogStorage:ILogStorage
    {
        private ILog _Logger = LogManager.GetLogger(typeof(DbLogStorage));
        public bool Save(LogInfo info)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(SettingManager.Default.ConnectionString))
                {
                    SqlCommand sqlCommand;
                    SqlParameter sqlParameter;

                    sqlCommand = new SqlCommand(((info.UserId.Equals(Guid.Empty)) ? "dbo.P_SaveLogForLoginFail" : "dbo.P_SaveLog"), sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (info.UserId.Equals(Guid.Empty))
                    {
                        sqlParameter = sqlCommand.Parameters.Add("@LoginName", SqlDbType.VarChar, 20);
                        sqlParameter.Value = info.LoginName;
                    }
                    else
                    {
                        sqlParameter = sqlCommand.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier);
                        sqlParameter.Value = info.UserId;
                    }

                    sqlParameter = sqlCommand.Parameters.Add("@IP", SqlDbType.NVarChar, 15);
                    sqlParameter.Value = info.Ip;
                    sqlParameter = sqlCommand.Parameters.Add("@Role", SqlDbType.NVarChar, 30);
                    sqlParameter.Value = info.UserType;
                    sqlParameter = sqlCommand.Parameters.Add("@ObjectIDs", SqlDbType.NVarChar, 4000);
                    sqlParameter.Value = string.IsNullOrEmpty(info.Msg) ? (object)DBNull.Value : info.Msg;

                    sqlParameter = sqlCommand.Parameters.Add("@Timestamp", SqlDbType.DateTime);
                    sqlParameter.Value = info.Timestamp;

                    sqlParameter = sqlCommand.Parameters.Add("@Event", SqlDbType.NVarChar, 4000);
                    sqlParameter.Value = info.EventType;

                    if (info.TransactionId != null)
                    {
                        sqlParameter = sqlCommand.Parameters.Add("@TransactionID", SqlDbType.UniqueIdentifier);
                        sqlParameter.Value = info.TransactionId.Equals(Guid.Empty) ? (object)DBNull.Value : info.TransactionId;
                    }
                    if (info.AccountId != null)
                    {
                        sqlParameter = sqlCommand.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier);
                        sqlParameter.Value = info.AccountId.Equals(Guid.Empty) ? (object)DBNull.Value : info.AccountId;
                    }
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex);
                return false;
            }
        }
    }
}
