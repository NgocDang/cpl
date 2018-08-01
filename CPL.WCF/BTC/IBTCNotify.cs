using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CPL.WCF.BTC
{
    [ServiceContract]
    public interface IBTCNotify
    {
        [OperationContract]
        [WebGet]
        BTCNotifyResult Notify(string txHashId);
    }
}
