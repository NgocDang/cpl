using CPL.Common.Enums;
using CPL.WCF.Misc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CPL.WCF.BTC
{
    public class BTCNotify : IBTCNotify
    {
        /// <summary>
        /// Inserts the tx identifier to BTC transaction.
        /// </summary>
        /// <param name="txHashId">The tx identifier.</param>
        /// <returns></returns>
        public BTCNotifyResult Notify(string txHashId)
        {
            var ipAddress = Utils.GetClientIpAddress();
            if (!Utils.IsAuthenticated(ipAddress))
            {
                return new BTCNotifyResult { Status = new Status { Code = Status.UnAuthenticatedCode, Text = string.Format(Status.UnAuthenticatedText, ipAddress) } };
            }
            else
            {
                if (!string.IsNullOrEmpty(txHashId))
                {
                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CPLConnection"].ConnectionString))
                    {
                        try
                        {
                            SqlCommand command = new SqlCommand("dbo.usp_InsertTxHashIdToBTCTransaction", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@TxHashId", txHashId);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                            return new BTCNotifyResult { Status = new Status { Code = Status.OkCode, Text = Status.OkText } };
                        }
                        catch (Exception ex)
                        {
                            return new BTCNotifyResult { Status = new Status { Code = Status.ExceptionCode, Text = ex.Message } };
                        }
                    }
                }
                else
                {
                    return new BTCNotifyResult { Status = new Status { Code = Status.InvalidTxHashIdCode, Text = Status.InvalidTxHashIdText } };
                }
            }
        }
    }
}
