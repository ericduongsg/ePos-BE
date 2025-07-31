using Core_App.BL;
using Newtonsoft.Json.Linq;
using NLog;
using System;
//using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.BL
{


    public class pub_Function
    {
        Core_App.DAL dal = new Core_App.DAL();
        Utilities until = new Utilities();

        #region Get email in site config
        public void getEmailConfig(ref string sUserName, ref string sPassword, ref string sSMTP, ref int iPortNo, ref string sBCC, ref string SMTP_User_Name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select Value from tbl_SiteConfig ");
            sb.Append("Where Name = '1_Sender' OR Name = '2_Serder_Pass' OR Name = '3_SMTP' ");
            sb.Append("OR Name = '4_Port' OR Name = '5_Bcc' OR Name = '6_SMTP_User_Name' ");
            sb.Append("Order By Name ");

            DataSet dsInfor = dal.GetDataset(sb.ToString(), "Infor");
            sb.Clear();
            dsInfor.Dispose();

            if (dsInfor.Tables["Infor"].Rows.Count == 6)
            {
                sUserName = dsInfor.Tables["Infor"].Rows[0]["value"].ToString().Trim();
                sPassword = Core_App.BL.Utilities.decryptRijndael(dsInfor.Tables["Infor"].Rows[1]["value"].ToString().Trim(), Core_App.BL.Constants.RijndaelHashType.PUBLIC);
                //   sPassword = dsInfor.Tables["Infor"].Rows[1]["value"].ToString().Trim();
                sSMTP = dsInfor.Tables["Infor"].Rows[2]["value"].ToString().Trim();
                try
                {
                    iPortNo = int.Parse(dsInfor.Tables["Infor"].Rows[3]["value"].ToString());
                }
                catch
                {
                    iPortNo = 25;
                }
                sBCC += ";" + dsInfor.Tables["Infor"].Rows[4]["value"].ToString().Trim();
                sBCC = sBCC.Trim(';');
                SMTP_User_Name = dsInfor.Tables["Infor"].Rows[5]["value"].ToString().Trim();
            }
            else if (dsInfor.Tables["Infor"].Rows.Count == 5)
            {
                sUserName = dsInfor.Tables["Infor"].Rows[0]["value"].ToString().Trim();
                sPassword = Core_App.BL.Utilities.decryptRijndael(dsInfor.Tables["Infor"].Rows[1]["value"].ToString().Trim(), Core_App.BL.Constants.RijndaelHashType.PUBLIC);
                //   sPassword = dsInfor.Tables["Infor"].Rows[1]["value"].ToString().Trim();
                sSMTP = dsInfor.Tables["Infor"].Rows[2]["value"].ToString().Trim();
                try
                {
                    iPortNo = int.Parse(dsInfor.Tables["Infor"].Rows[3]["value"].ToString());
                }
                catch
                {
                    iPortNo = 587;
                }
                sBCC += ";" + dsInfor.Tables["Infor"].Rows[4]["value"].ToString().Trim();
                sBCC = sBCC.Trim(';');
            }
            dsInfor.Dispose();
        }
        #endregion

        public DataSet getImageUrl(string account_id, string role_name, string image_id, float image_type)
        {
            ArrayList paramArrayList = new ArrayList();
            paramArrayList.Add(dal.InitParameter("@PRM_account_id", SqlDbType.VarChar, 50, account_id));
            paramArrayList.Add(dal.InitParameter("@PRM_role_name", SqlDbType.VarChar, 20, role_name));
            paramArrayList.Add(dal.InitParameter("@PRM_image_id", SqlDbType.VarChar, 50, image_id));
            paramArrayList.Add(dal.InitParameter("@PRM_image_type", SqlDbType.VarChar, 5, image_type.ToString()));
            DataSet dsInfo = dal.GetDataset("API_GetImageUrl", "dsInfo", paramArrayList);
            paramArrayList.Clear();
            dsInfo.Dispose();

            return dsInfo;

        }
        public DataSet getImagePublicUrl(string account_id, string image_id, float image_type)
        {
            ArrayList paramArrayList = new ArrayList();
            paramArrayList.Add(dal.InitParameter("@PRM_account_id", SqlDbType.VarChar, 50, account_id));
            paramArrayList.Add(dal.InitParameter("@PRM_role_name", SqlDbType.VarChar, 20, ""));
            paramArrayList.Add(dal.InitParameter("@PRM_image_id", SqlDbType.VarChar, 50, image_id));
            paramArrayList.Add(dal.InitParameter("@PRM_image_type", SqlDbType.VarChar, 5, image_type.ToString()));
            DataSet dsInfo = dal.GetDataset("API_GetImagePublicUrl", "dsInfo", paramArrayList);
            paramArrayList.Clear();
            dsInfo.Dispose();

            return dsInfo;

        }
        public int getBookingTypeNum(string booking_type)
        {
            if (booking_type == "my_self")
            {
                return 1;
            }
            else if (booking_type == "a_child")
            {
                return 2;
            }
            else if (booking_type == "refill_medication")
            {
                return 3;
            }
            else if (booking_type == "travel_medication")
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }

        //public static float GetStandardFee()
        //{
        //    Core_App.DAL DAL = new Core_App.DAL();
        //    float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_StandardFee]() As Value", "Value"));
        //    return value;
        //}

        public static float GetSurchargeFeeConsultation()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_SurchargeFeeConsultation]() As Value", "Value"));
            return value;
        }

        public static float GetAdditionalTimeConsultation()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_TimeOutCosulation]() As Value", "Value"));
            return value;
        }

        public static bool GetConfig_RefillMedicationAllow(string member_id)
        {
            Core_App.DAL DAL = new Core_App.DAL();
            bool value = bool.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetRefillMedicationAllow]('" + member_id + "') As Value", "Value"));
            return value;
        }

        public static float GetConfig_RefillMedicationFee(string member_id)
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_RefillMedicationFee]('" + member_id + "') As Value", "Value"));
            return value;
        }

        public static float GetConfig_TimeOutSkipBooking()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_TimeOutSkipBooking]() As Value", "Value"));
            return value;
        }

        public static float GetConfig_TimeOutCancelBooking()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_TimeOutCancelBooking]() As Value", "Value"));
            return value;
        }

        public static bool GetForChildThatTurned21(string member_id)
        {
            bool value = false;
            Core_App.DAL DAL = new Core_App.DAL();
            int total_child = int.Parse(DAL.GetDataByCommand("Select [dbo].[fn_for_child_that_turned_21]('" + member_id + "') As Value", "Value"));
            if (total_child > 0)
            {
                value = true;
            }
            return value;
        }

        //public static void SendRecoveryEmail(string sEmail, string sFull_Name, string sRecovery_ID, string sRecovery_Code, string sPath, int account_type)
        //{
        //    System.IO.StreamReader objStreamReader = null;
        //    objStreamReader = System.IO.File.OpenText(sPath);
        //    string sContent = objStreamReader.ReadToEnd().ToString();
        //    objStreamReader.Close();
        //    string sRecoveryURL = Utilities.getAppURL().TrimEnd('/') + "/v1/ResetPassword?recovery=" + QueryStringEncryption.Encrypt(sRecovery_ID) + "&code=" + QueryStringEncryption.Encrypt(sRecovery_Code) + "&type=" + QueryStringEncryption.Encrypt(account_type.ToString());
        //    string sApp_Name = Utilities.getAppName();
        //    string sSubject = "[" + sApp_Name + "] Account recovery";

        //    sContent = sContent.Replace("{0}", sApp_Name);
        //    sContent = sContent.Replace("{1}", sFull_Name);
        //    sContent = sContent.Replace("{2}", sEmail);
        //    sContent = sContent.Replace("{3}", sRecovery_Code);
        //    sContent = sContent.Replace("{4}", sRecoveryURL);
        //    sContent = sContent.Replace("{99}", Utilities.getLogoUrl());


        //    System.Threading.Thread threadSendMails;
        //    threadSendMails = new System.Threading.Thread(delegate()
        //    {
        //        if (sEmail != "")
        //        {
        //            eMail_Helper.GMAIL_SMTP_Sender(sApp_Name, sEmail, "", sSubject, sContent.ToString(), true, "");
        //        }
        //    });
        //    threadSendMails.IsBackground = true;
        //    threadSendMails.Start();
        //}

        public static void SendRecoveryEmail(string sEmail, string sFull_Name, string sRecovery_ID, string sRecovery_Code, string sPath, int account_type)
        {
            System.IO.StreamReader objStreamReader = null;
            objStreamReader = System.IO.File.OpenText(sPath);
            string sContent = objStreamReader.ReadToEnd().ToString();
            objStreamReader.Close();
            string sRecoveryURL = Utilities.getAppURL().TrimEnd('/') + "/v1/ResetPassword?recovery=" + QueryStringEncryption.Encrypt(sRecovery_ID) + "&code=" + QueryStringEncryption.Encrypt(sRecovery_Code) + "&type=" + QueryStringEncryption.Encrypt(account_type.ToString());

            if ((AccountType)account_type == AccountType.Driver ||
                (AccountType)account_type == AccountType.Pharmacy)
            {
                sRecoveryURL = Utilities.getAppURL().TrimEnd('/') + "/v1/ResetPasswords?recovery=" + QueryStringEncryption.Encrypt(sRecovery_ID) + "&code=" + QueryStringEncryption.Encrypt(sRecovery_Code) + "&type=" + QueryStringEncryption.Encrypt(account_type.ToString());
            }
            string sApp_Name = Utilities.getAppName();
            string sSubject =  sApp_Name + " - Forgotten Your Password?";

            sContent = sContent.Replace("{0}", sApp_Name);
            sContent = sContent.Replace("{1}", sFull_Name);
            sContent = sContent.Replace("{2}", sEmail);
            sContent = sContent.Replace("{3}", sRecovery_Code);
            sContent = sContent.Replace("{4}", sRecoveryURL);
            sContent = sContent.Replace("{99}", Utilities.getLogoUrl());


            System.Threading.Thread threadSendMails;
            threadSendMails = new System.Threading.Thread(delegate()
            {
                if (sEmail != "")
                {
                    eMail_Helper.GMAIL_SMTP_Sender(sApp_Name, sEmail, "", sSubject, sContent.ToString(), true, "");
                }
            });
            threadSendMails.IsBackground = true;
            threadSendMails.Start();
        }

        public static string getImageUrlAPI(string account_id, string refresh_token, string account_type, string image_id, string image_type, string latest_update)
        {
            StringBuilder sbImageUrl = new StringBuilder();
            if (account_type == Constants.API_ROLE_NAME.MEMBER ||
                account_type == Constants.API_ROLE_NAME.DOCTOR ||
                account_type == Constants.API_ROLE_NAME.DRIVER ||
                account_type == Constants.API_ROLE_NAME.DEV ||
                account_type == Constants.API_ROLE_NAME.PHARMACY)
            {

                sbImageUrl.Append(Utilities.getAPIURL() + "/pub/Component/ViewImage?");
                //sbImageUrl.Append("role=" + QueryStringEncryption.Encrypt(account_type));
                if (account_id != "")
                {
                    sbImageUrl.Append("&acc_type=" + QueryStringEncryption.Encrypt(account_type));
                    sbImageUrl.Append("&acc_id=" + QueryStringEncryption.Encrypt(account_id));
                }
                if (image_id != null && image_id != "" && image_id != "0")
                {
                    sbImageUrl.Append("&img_id=" + QueryStringEncryption.Encrypt(image_id.ToString()));
                }
                sbImageUrl.Append("&img_type=" + QueryStringEncryption.Encrypt(image_type.ToString()));
                if (latest_update != "")
                {
                    sbImageUrl.Append("&latest_update=" + DateTime.Parse(latest_update).Ticks);
                }
                else
                {
                    sbImageUrl.Append("&latest_update=" + DateTime.Now.Ticks);
                }
            }
            return sbImageUrl.ToString();
        }

        //public static string GetAccessToken(string account_id, string refresh_token, bool request_update_contact_info, bool request_activate_account, bool for_child_that_turned_21, string type, string role_name)
        //{
        //    string API_URL = Core_App.BL.Utilities.getMainAppURL();

        //    var oAuthIdentity = new ClaimsIdentity(Constants.AUTHENTICATION.TYPE);//"JWT"
        //    oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, role_name));
        //    oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Utilities.encryptRijndael(account_id, Constants.RijndaelHashType.PRIVATE)));
        //    oAuthIdentity.AddClaim(new Claim("rfr-tkn", refresh_token));

        //    if (role_name == Constants.API_ROLE_NAME.MEMBER)
        //    {
        //        oAuthIdentity.AddClaim(new Claim("request_update_contact_info", request_update_contact_info.ToString()));
        //        oAuthIdentity.AddClaim(new Claim("request_activate_account", request_activate_account.ToString()));
        //        oAuthIdentity.AddClaim(new Claim("for_child_that_turned_21", for_child_that_turned_21.ToString()));
        //    }
        //    else if (role_name == Constants.API_ROLE_NAME.PHARMACY)
        //    {
        //        oAuthIdentity.AddClaim(new Claim("type", type.ToString()));
        //    }

        //    var ticket = new Microsoft.Owin.Security.AuthenticationTicket(oAuthIdentity, null);
        //    ticket.Properties.IssuedUtc = DateTime.Now;
        //    ticket.Properties.ExpiresUtc = Utilities.getExpireDateAccessToken();

        //    CustomJwtFormat cjwtf = new CustomJwtFormat(API_URL);
        //    string access_token = cjwtf.Protect(ticket);

        //    return access_token;
        //}

        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            //age = DateTime.Now.Year - dateOfBirth.Year;
            
            //if (DateTime.Now.DayOfYear - dateOfBirth.DayOfYear > 0)
            //{
            //    age = DateTime.Now.DayOfYear - dateOfBirth.DayOfYear;
            //}

            //int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            {
                age = age - 1;
            }
            return age;
        }

        public static string sendSMS(string phoneNumber, string smsContent)
        {
            string message = "";
            if (phoneNumber.Length > 2)
            {
                //string resultString = "{\"result\":{\"status\":\"NOK\",\"error\":\"Account does not exist or has expired.\",\"testmode\":false},\"content\":{\"value\":\"Your password is oN7J8lJmw39sgujQi1A Og==\",\"encoding\":null,\"chars\":0,\"parts\":0},\"receivers\":[],\"credit\":{\"balance\":0,\"required\":0}}";

                phoneNumber = phoneNumber.Substring(0, 2) == Constants.MOBILE_CODE_SINGAPORE ? phoneNumber : Constants.MOBILE_CODE_SINGAPORE + phoneNumber;
                var requestUri = new StringBuilder("http://www.smsdome.com/api/http/sendsms.aspx?");
                requestUri.AppendFormat("appid={0}&appsecret={1}", ConfigurationManager.AppSettings["App_ID"], ConfigurationManager.AppSettings["App_Secret"]);
                requestUri.AppendFormat("&receivers={0}", phoneNumber);
                requestUri.AppendFormat("&content={0}&responseformat=JSON", smsContent);

                var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
                request.Method = WebRequestMethods.Http.Get;

                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var resultString = reader.ReadToEnd();
                    try
                    {
                        dynamic resultData = JObject.Parse(resultString);
                        JObject routes = resultData["result"];
                        if (routes != null)
                        {
                            if ((string)routes.SelectToken("status") == "OK")
                            {
                                //logger.Info("Activate Code has been successfully sent to " + phoneNumber);
                                message = "Password has been successfully sent to " + phoneNumber;
                            }
                            else
                            {
                                message = (string)routes.SelectToken("error");
                            }
                        }
                        else
                        {
                            message = "Could not connect to SMS services";
                        }
                    }
                    catch (Exception)
                    {
                        return "Sorry. An unexpected error has occured. Please contact your system administrator.";
                    }
                }
            }
            return message;
        }

        public static void clearLog()
        {
            System.Threading.Thread thread;
            thread = new System.Threading.Thread(delegate()
            {
                try
                {
                    Core_App.DAL dal = new Core_App.DAL();

                    AmazonUploader amazon_uploader = new AmazonUploader();

                    #region Delete Logs folder Of Services
                    //string Log_Services_Path = System.Configuration.ConfigurationManager.AppSettings["Log_Services_Path"].ToString();
                    //string[] dirs_2 = Directory.GetDirectories(Log_Services_Path);
                    //foreach (string dir in dirs_2)
                    //{
                    //    DirectoryInfo d = new DirectoryInfo(dir);
                    //    if (d.CreationTime < DateTime.Now.AddDays(-Day_Delete_Folder_Log))
                    //    {
                    //        d.Delete(true);
                    //    }
                    //} 
                    #endregion

                    #region Delete images in folder temps of S3
                    float Day_Delete_Image_In_S3 = 3;
                    float.TryParse(System.Configuration.ConfigurationManager.AppSettings["Day_Delete_Image_In_S3"].ToString(), out Day_Delete_Image_In_S3);
                    amazon_uploader.deleteFileInFolderTemps("Temps", Day_Delete_Image_In_S3);
                    #endregion

                    #region Delete Logs folder Of API
                    float Day_Delete_Folder_Log = 3;
                    float.TryParse(System.Configuration.ConfigurationManager.AppSettings["Day_Delete_Folder_Log"].ToString(), out Day_Delete_Folder_Log);
                    string Log_API_Path = System.Configuration.ConfigurationManager.AppSettings["API_Log_Path"].ToString();
                    string[] dirs = Directory.GetDirectories(Log_API_Path);
                    foreach (string dir in dirs)
                    {
                        DirectoryInfo d = new DirectoryInfo(dir);

                        if (d.CreationTime < DateTime.Now.AddDays(-Day_Delete_Folder_Log))
                        {
                            d.Delete(true);
                        }
                    }
                    #endregion

                    #region Delete Logs folder Of CMS
                    string Log_CMS_Path = System.Configuration.ConfigurationManager.AppSettings["CMS_Log_Path"].ToString();
                    dirs = Directory.GetDirectories(Log_CMS_Path);
                    foreach (string dir in dirs)
                    {
                        DirectoryInfo d = new DirectoryInfo(dir);
                        if (d.CreationTime < DateTime.Now.AddDays(-Day_Delete_Folder_Log))
                        {
                            d.Delete(true);
                        }
                    }
                    #endregion

                    #region Delete data of tbl_API_Log, tbl_SysAppDev_Account_Log_Sub table
                    float Day_Delete_Data_Log = 3;
                    float.TryParse(System.Configuration.ConfigurationManager.AppSettings["Day_Delete_Data_Log"].ToString(), out Day_Delete_Data_Log);

                    float Day_Run_Clear_Log = 7;
                    float.TryParse(System.Configuration.ConfigurationManager.AppSettings["Day_Run_Clear_Log"].ToString(), out Day_Run_Clear_Log);

                    ArrayList paramArrayList = new ArrayList();
                    paramArrayList.Add(dal.InitParameter("@PRM_day", SqlDbType.Float, 10, Day_Delete_Data_Log));
                    paramArrayList.Add(dal.InitParameter("@PRM_day_clear_log", SqlDbType.Float, 10, Day_Run_Clear_Log));
                    string result = dal.ExecuteStoredProcedureAndReturnString("CMS_ClearLog", paramArrayList);
                    paramArrayList.Clear();

                    #endregion

                }
                catch (Exception ex)
                {
                    Logger log = LogManager.GetLogger("Write-Log");
                    log.Error("Error clear log" + ex.ToString());
                }

            });
            thread.IsBackground = true;
            thread.Start();
        }


        #region Payment Gateway

        public static String Sign(IDictionary<string, string> paramsArray)
        {
            string Sectet_Key = System.Configuration.ConfigurationManager.AppSettings["Sectet_Key"].ToString();
            return Sign(buildDataToSign(paramsArray), Sectet_Key);
        }

        private static String Sign(String data, String secretKey)
        {
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(data);
            return Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
        }

        private static String buildDataToSign(IDictionary<string, string> paramsArray)
        {
            String[] signedFieldNames = paramsArray["signed_field_names"].Split(',');
            IList<string> dataToSign = new List<string>();

            foreach (String signedFieldName in signedFieldNames)
            {
                dataToSign.Add(signedFieldName + "=" + paramsArray[signedFieldName]);
            }

            return commaSeparate(dataToSign);
        }

        private static String commaSeparate(IList<string> dataToSign)
        {
            return String.Join(",", dataToSign);
        }

        #endregion
    }
}
