using API.BO;
using Core_App;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace API.BL
{
    public class ApiKey_BL
    {
        private DAL dal = new DAL();
        private Logger logger = LogManager.GetLogger("Write-Log");
        /// <summary>
        /// Gets the best available API key based on usage (prioritizes keys with fewer requests today)
        /// </summary>
        /// <returns>The API key info object or null if no keys available</returns>
        public ApiKeyInfo GetBestAvailableApiKey()
        {
            try
            {
                // Reset daily usage for API keys that were last used before today
                string resetQuery = string.Format(@"
                    UPDATE tbl_Api_Keys
                    SET Total_Request = 0
                    WHERE Latest_Request_On < CAST(GETDATE() AS DATE)");

                dal.ExecuteScalar(resetQuery);

                // Get all API keys ordered by:
                // 1. Keys with Total_Request < 20 for today (or null Total_Request)
                // 2. Keys with older Latest_Request_On (to distribute load)
                // 3. Keys with fewer Total_Request
                string query = @"
                    SELECT TOP 1 Id, Api_Key, Latest_Request_On, Total_Request, Type
                    FROM tbl_Api_Keys
                    WHERE (Type = 1 AND (Total_Request < 20 OR Total_Request IS NULL)) OR Type != 1
                    ORDER BY
                        CASE WHEN Latest_Request_On < CAST(GETDATE() AS DATE) THEN 0 ELSE 1 END,
                        Latest_Request_On ASC,
                        Total_Request ASC";

                return dal.GetListObject<ApiKeyInfo>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Updates the API key usage after a successful request
        /// </summary>
        /// <param name="apiKey">The API key that was used</param>
        /// <returns>True if update was successful</returns>
        public bool UpdateApiKeyUsage(string apiKey)
        {
            try
            {
                // Update the API key usage with inline parameter values
                string updateQuery = string.Format(@"
                    UPDATE tbl_Api_Keys
                    SET
                        Latest_Request_On = '{0}',
                        Total_Request = ISNULL(Total_Request, 0) + 1
                    WHERE Api_Key = '{1}'",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    apiKey.Replace("'", "''")); // Escape single quotes in API key

                dal.ExecuteScalar(updateQuery);
                return true;
            }
            catch (Exception ex)
            {
                // Log error if needed
                return false;
            }
        }

        ///// <summary>
        ///// Gets all API keys for debugging/monitoring purposes
        ///// </summary>
        ///// <returns>List of all API keys with their usage stats</returns>
        //public List<ApiKeyInfo> GetAllApiKeys()
        //{
        //    try
        //    {
        //        string query = "SELECT Id, Api_Key, Latest_Request_On, Total_Request FROM tbl_Api_Keys ORDER BY Id";
        //        return dal.GetListObject<ApiKeyInfo>(query);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error if needed
        //        return new List<ApiKeyInfo>();
        //    }
        //}

    }
}