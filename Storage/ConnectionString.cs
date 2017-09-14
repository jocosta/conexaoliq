using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Storage
{
    public static class ConnectionString
    {
        static string account = "conexaoliqstorage";
        static string key = "EiR7GRXdBrh6DoXfVZI31ezrVG8lx+WwpdJlXGgY/EMOeCODFGq67DvGhU/Pn33O5AVkzXqgFJobva/NetouFQ==";
        public static CloudStorageAccount GetConnectionString()
        {
            string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", account, key);
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}