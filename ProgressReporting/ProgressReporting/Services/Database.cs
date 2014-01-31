using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgressReporting.Services
{
    public class Database
    {
        public static readonly Database Instance = new Database();

        ConcurrentDictionary<string, object> _values = new ConcurrentDictionary<string, object>();

        public string GetGlobalJobId()
        {
            object result;
            return _values.TryGetValue("GlobalJobId", out result) ? (string)result : "";
        }

        public void StoreGlobalJobId(string jobId)
        {
            _values["GlobalJobId"] = jobId;
        }
    }
}