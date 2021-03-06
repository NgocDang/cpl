﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     //
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BTCCurrentPriceService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetBTCCurrentPriceResult", Namespace="http://schemas.datacontract.org/2004/07/CPL.WCF.ExchangeCurrentPrice")]
    public partial class SetBTCCurrentPriceResult : object
    {
        
        private long DateTimeField;
        
        private decimal PriceField;
        
        private BTCCurrentPriceService.Status StatusField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long DateTime
        {
            get
            {
                return this.DateTimeField;
            }
            set
            {
                this.DateTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Price
        {
            get
            {
                return this.PriceField;
            }
            set
            {
                this.PriceField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BTCCurrentPriceService.Status Status
        {
            get
            {
                return this.StatusField;
            }
            set
            {
                this.StatusField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Status", Namespace="http://schemas.datacontract.org/2004/07/CPL.WCF.Misc")]
    public partial class Status : object
    {
        
        private int CodeField;
        
        private string TextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Code
        {
            get
            {
                return this.CodeField;
            }
            set
            {
                this.CodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetBTCCurrentPriceResult", Namespace="http://schemas.datacontract.org/2004/07/CPL.WCF.ExchangeCurrentPrice")]
    public partial class GetBTCCurrentPriceResult : object
    {
        
        private long DateTimeField;
        
        private decimal PriceField;
        
        private BTCCurrentPriceService.Status StatusField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long DateTime
        {
            get
            {
                return this.DateTimeField;
            }
            set
            {
                this.DateTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Price
        {
            get
            {
                return this.PriceField;
            }
            set
            {
                this.PriceField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public BTCCurrentPriceService.Status Status
        {
            get
            {
                return this.StatusField;
            }
            set
            {
                this.StatusField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BTCCurrentPriceService.IBTCCurrentPrice")]
    public interface IBTCCurrentPrice
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBTCCurrentPrice/SetBTCCurrentPrice", ReplyAction="http://tempuri.org/IBTCCurrentPrice/SetBTCCurrentPriceResponse")]
        System.Threading.Tasks.Task<BTCCurrentPriceService.SetBTCCurrentPriceResult> SetBTCCurrentPriceAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBTCCurrentPrice/GetBTCCurrentPrice", ReplyAction="http://tempuri.org/IBTCCurrentPrice/GetBTCCurrentPriceResponse")]
        System.Threading.Tasks.Task<BTCCurrentPriceService.GetBTCCurrentPriceResult> GetBTCCurrentPriceAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface IBTCCurrentPriceChannel : BTCCurrentPriceService.IBTCCurrentPrice, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class BTCCurrentPriceClient : System.ServiceModel.ClientBase<BTCCurrentPriceService.IBTCCurrentPrice>, BTCCurrentPriceService.IBTCCurrentPrice
    {
        
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public BTCCurrentPriceClient() : 
                base(BTCCurrentPriceClient.GetDefaultBinding(), BTCCurrentPriceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IBTCCurrentPrice.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BTCCurrentPriceClient(EndpointConfiguration endpointConfiguration) : 
                base(BTCCurrentPriceClient.GetBindingForEndpoint(endpointConfiguration), BTCCurrentPriceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BTCCurrentPriceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(BTCCurrentPriceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BTCCurrentPriceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(BTCCurrentPriceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BTCCurrentPriceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<BTCCurrentPriceService.SetBTCCurrentPriceResult> SetBTCCurrentPriceAsync()
        {
            return base.Channel.SetBTCCurrentPriceAsync();
        }
        
        public System.Threading.Tasks.Task<BTCCurrentPriceService.GetBTCCurrentPriceResult> GetBTCCurrentPriceAsync()
        {
            return base.Channel.GetBTCCurrentPriceAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IBTCCurrentPrice))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IBTCCurrentPrice))
            {
                return new System.ServiceModel.EndpointAddress("http://202.53.150.20/ExchangeCurrentPrice/BTCCurrentPrice.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return BTCCurrentPriceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IBTCCurrentPrice);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return BTCCurrentPriceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IBTCCurrentPrice);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IBTCCurrentPrice,
        }
    }
}
