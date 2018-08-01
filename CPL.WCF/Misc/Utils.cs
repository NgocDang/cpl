using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace CPL.WCF.Misc
{
    public class Utils
    {
        public static string GetClientIpAddress()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string address = string.Empty;
            if (properties.Keys.Contains(HttpRequestMessageProperty.Name))
            {
                var endpointLoadBalancer = properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                if (endpointLoadBalancer != null && endpointLoadBalancer.Headers["X-Forwarded-For"] != null)
                    address = endpointLoadBalancer.Headers["X-Forwarded-For"];
            }
            if (string.IsNullOrEmpty(address))
            {
                address = endpoint.Address;
            }
            return address;
        }

        public static bool IsAuthenticated(string ipAddress)
        {
            if (ConfigurationManager.AppSettings["Authorization:WhiteIps"].ToString().Split(';').Contains(ipAddress))
                return true;
            return false;
        }
    }
}