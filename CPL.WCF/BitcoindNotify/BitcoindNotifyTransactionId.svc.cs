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

namespace CPL.WCF.BitcoindNotify
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BitcoindNotifyTransactionId" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BitcoindNotifyTransactionId.svc or BitcoindNotifyTransactionId.svc.cs at the Solution Explorer and start debugging.
    public class BitcoindNotifyTransactionId : IBitcoindNotifyTransactionId
    {
        /// <summary>
        /// Inserts the tx identifier to BTC transaction.
        /// </summary>
        /// <param name="txHashId">The tx identifier.</param>
        /// <returns></returns>
        public BitcoindNotifyTransactionIdResult InsertTxHashIdToBTCTransaction(string txHashId)
        {
            if (txHashId != null)
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CPLConnection"].ConnectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("dbo.usp_InsertTxIdToBTCTransaction", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TxHashId", txHashId);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        return new BitcoindNotifyTransactionIdResult { Status = new Status { Code = Status.OkCode, Text = Status.OkText } };
                    }
                    catch (Exception ex)
                    {
                        return new BitcoindNotifyTransactionIdResult { Status = new Status { Code = Status.ExceptionCode, Text = ex.Message } };
                    }
                }
            }
            else
            {
                return new BitcoindNotifyTransactionIdResult { Status = new Status { Code = Status.InvalidIxIdCode, Text = Status.InvalidIxIdText } };
            }
        }
    }
}
