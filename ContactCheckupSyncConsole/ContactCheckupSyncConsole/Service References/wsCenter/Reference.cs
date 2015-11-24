﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContactCheckupSyncConsole.wsCenter {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="wsCenter.ServiceSoap")]
    public interface ServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InsertLogApplication", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void InsertLogApplication(string strAppName, string strUser, string strIp, string strComName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InsertLogApplicationBySite", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void InsertLogApplicationBySite(string strAppName, string strAppName_Sub, string strSite, string strUser, string strIp, string strComName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Usage_Log_Insert", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void Usage_Log_Insert(string appname, string usern);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/checkAppAuthorize", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        bool checkAppAuthorize(string usern, string dept_id, string app_id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/LoginChecker", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string LoginChecker(string usern, string pwd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getDept", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet getDept();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceSoapChannel : ContactCheckupSyncConsole.wsCenter.ServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceSoapClient : System.ServiceModel.ClientBase<ContactCheckupSyncConsole.wsCenter.ServiceSoap>, ContactCheckupSyncConsole.wsCenter.ServiceSoap {
        
        public ServiceSoapClient() {
        }
        
        public ServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void InsertLogApplication(string strAppName, string strUser, string strIp, string strComName) {
            base.Channel.InsertLogApplication(strAppName, strUser, strIp, strComName);
        }
        
        public void InsertLogApplicationBySite(string strAppName, string strAppName_Sub, string strSite, string strUser, string strIp, string strComName) {
            base.Channel.InsertLogApplicationBySite(strAppName, strAppName_Sub, strSite, strUser, strIp, strComName);
        }
        
        public void Usage_Log_Insert(string appname, string usern) {
            base.Channel.Usage_Log_Insert(appname, usern);
        }
        
        public bool checkAppAuthorize(string usern, string dept_id, string app_id) {
            return base.Channel.checkAppAuthorize(usern, dept_id, app_id);
        }
        
        public string LoginChecker(string usern, string pwd) {
            return base.Channel.LoginChecker(usern, pwd);
        }
        
        public System.Data.DataSet getDept() {
            return base.Channel.getDept();
        }
    }
}
