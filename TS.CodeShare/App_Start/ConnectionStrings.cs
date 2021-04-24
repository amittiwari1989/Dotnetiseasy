using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TS.CodeShare.App_Start
{
    public class ConnectionStrings
    {
        public static string getConnectionStrings()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
        public static DBConnection.DBServerType getServerType()
        {
            string constr = "ConnectionString";

            return (ConfigurationManager.ConnectionStrings[constr].ProviderName.Contains("MySql") ? DBConnection.DBServerType.MYSQL :
            (ConfigurationManager.ConnectionStrings[constr].ProviderName.Contains("Oracle") ? DBConnection.DBServerType.ORACLE : DBConnection.DBServerType.MSSQL));
        }
    }
}