using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core_App.BL
{
    public class Utilities
    {
        private Core_App.DAL dal = new Core_App.DAL();

        public static string encryptRijndael(string sourceValue, Constants.RijndaelHashType hashType)
        {
            if (hashType.ToString() == Constants.RijndaelHashType.PRIVATE.ToString())
            {
                return Rijndael.Encrypt(sourceValue,
                    Constants.RIJNDAEL_PRV_KEY,
                    Constants.RIJNDAEL_PRV_SALT,
                    Constants.RijndaelHashAlgorithm.SHA.ToString(),
                    1024, "#!A$!T4kqxdc-}Vx", 256);
            }
            else
            {
                return Rijndael.Encrypt(sourceValue,
                    Constants.RIJNDAEL_PUB_KEY,
                    Constants.RIJNDAEL_PUB_SALT,
                    Constants.RijndaelHashAlgorithm.SHA256.ToString(),
                    1024, "[Lf,2fKdvQmN3&Qs", 256);
            }
        }
        public static string decryptRijndael(string sourceValue, Constants.RijndaelHashType hashType)
        {
            try
            {
                if (hashType.ToString() == Constants.RijndaelHashType.PRIVATE.ToString())
                {
                    return Rijndael.Decrypt(sourceValue,
                        Constants.RIJNDAEL_PRV_KEY,
                        Constants.RIJNDAEL_PRV_SALT,
                        Constants.RijndaelHashAlgorithm.SHA.ToString(),
                        1024, "#!A$!T4kqxdc-}Vx", 256);
                }
                else
                {
                    return Rijndael.Decrypt(sourceValue,
                        Constants.RIJNDAEL_PUB_KEY,
                        Constants.RIJNDAEL_PUB_SALT,
                        Constants.RijndaelHashAlgorithm.SHA256.ToString(),
                        1024, "[Lf,2fKdvQmN3&Qs", 256);
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Encrypt value by using XOR cipher
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encryptionKey"></param>
        /// <returns>Encrypted value</returns>
        public string encryptXORCipher(string sourceValue, string encryptionKey)
        {
            string sbOut = String.Empty;
            for (int i = 0; i < sourceValue.Length; i++)
            {
                sbOut += String.Format("{0:00}", sourceValue[i] ^ encryptionKey[i % encryptionKey.Length]);
            }

            return sbOut;
        }

        /// <summary>
        /// Descrypt value by using XOR cipher
        /// </summary>
        /// <param name="strIn"></param>
        /// <param name="decryptionKey"></param>
        /// <returns>Decrypted value</returns>
        public string decryptXORCipher(string sourceValue, string decryptionKey)
        {
            string sbOut = String.Empty;
            for (int i = 0; i < sourceValue.Length; i += 2)
            {
                byte code = Convert.ToByte(sourceValue.Substring(i, 2));
                sbOut += (char)(code ^ decryptionKey[(i / 2) % decryptionKey.Length]);
            }
            return sbOut;
        }

        //Set Encrypt
        public string setEncrypt(string sParam)
        {
            sParam = Rijndael.Encrypt(sParam, "pr@se_pwd", "cts@devteam", "MD5", 2, "@1B2c3D4e5F6g7H8", 256);
            return sParam;
        }

        public string getDecrypt(string sParam)
        {
            try
            {
                sParam = Rijndael.Decrypt(sParam, "pr@se_pwd", "cts@devteam", "MD5", 2, "@1B2c3D4e5F6g7H8", 256);
            }
            catch
            {
                System.Web.HttpContext.Current.Response.Redirect("default.aspx?error=get-value-failed", true);
            }
            return sParam;
        }

        public static string generateSalt()
        {
            var buf = new byte[24];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static string generateRandomValue(int lenght, int seekNumber,
            Constants.RandomValueType randomValue)
        {
            string allowedChars = string.Empty;
            if (randomValue == Constants.RandomValueType.INT)
            {
                allowedChars = "0123456789";
            }
            else if (randomValue == Constants.RandomValueType.STRING)
            {
                allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            }
            else if (randomValue == Constants.RandomValueType.PASSWORD)
            {
                allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()?[]";
            }
            else if (randomValue == Constants.RandomValueType.ALL)
            {
                allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            }

            char[] chars = new char[lenght];
            Random rd;
            if (seekNumber > 0)
            {
                rd = new Random(seekNumber);
            }
            else
            {
                rd = new Random();
            }
            for (int i = 0; i < lenght; i++)
            {
                if (i == 0 && randomValue == Constants.RandomValueType.INT)
                {
                    string conChars = "123456789";
                    chars[i] = conChars[rd.Next(0, conChars.Length)];
                }
                else
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
            }

            return new string(chars);
        }

        public static string generateRandomImageName(string prefix, int lenght, int seekNumber, Constants.RandomValueType randomValue, string fileExtension = ".jpg")
        {
            return prefix + "-" + DateTime.Now.ToString("ddMMyyyyhhmmss") + generateRandomValue(lenght, seekNumber, randomValue) + fileExtension;
        }

        public static bool checkValue(string sourceValue, Constants.CheckValueType valueType)
        {
            if (string.IsNullOrWhiteSpace(sourceValue))
                return false;

            Boolean returnVal = false;
            if (valueType == Constants.CheckValueType.EMAIL)
            {
                string pattern = @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$";
                System.Text.RegularExpressions.Regex check = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);

                returnVal = check.IsMatch(sourceValue);
            }
            else if (valueType == Constants.CheckValueType.INT)
            {
                string pattern = @"^\d+$";
                System.Text.RegularExpressions.Regex check = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);

                returnVal = check.IsMatch(sourceValue);
            }
            else if (valueType == Constants.CheckValueType.FLOAT)
            {
                float fOutput = 0;
                returnVal = float.TryParse(sourceValue, out fOutput);
            }

            return returnVal;
        }

        public static string getLuhnCheckDigit(string number)
        {
            var sum = 0;
            var alt = true;
            var digits = number.ToCharArray();
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                var curDigit = (digits[i] - 48);
                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                        curDigit -= 9;
                }
                sum += curDigit;
                alt = !alt;
            }
            if ((sum % 10) == 0)
            {
                return "0";
            }
            return (10 - (sum % 10)).ToString();
        }

        public static bool isLuhnCheckDigit(string sourceValue)
        {
            if (string.IsNullOrEmpty(sourceValue))
            {
                return false;
            }

            //// 1. Starting with the check digit double the value of every other digit
            //// 2. If doubling of a number results in a two digits number, add up the digits to get a single digit number. This will results in eight
            // single digit numbers
            //// 3. Get the sum of the digits
            int sumOfDigits = sourceValue.Where((e) => e >= '0' && e <= '9')
                .Reverse()
                .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                .Sum((e) => e / 10 + e % 10);
            //// If the final sum is divisible by 10, then the credit card number is valid. If it is not divisible by 10, the number is invalid.
            return sumOfDigits % 10 == 0;
        }

        #region Generate code with encrypt
        public string generateCodeWithEncrypt(string columnName, string tableName, string condition, string EncryptionKeys, int length, string prefix = "")
        {
            int loop = 0;
            Boolean flag = false;
            string code = string.Empty;

            while (!flag)
            {
                string randomInt = generateRandomValue((length - 1), Guid.NewGuid().GetHashCode() + loop, Constants.RandomValueType.INT);
                code = prefix + DateTime.Now.ToString("yyMM") + "-" + randomInt + getLuhnCheckDigit(randomInt);
                flag = dal.CheckExistingData(columnName, " " + tableName + " Where " + columnName + " = '" + code + "'" + condition);
                loop++;
            }
            return code;
        }

        public string generateCodeWithEncryptNotCheckExistingData(string EncryptionKeys, int length, string prefix = "")
        {
            int loop = 0;
            string code = string.Empty;

            string randomInt = generateRandomValue((length - 1), Guid.NewGuid().GetHashCode() + loop, Constants.RandomValueType.INT);
            code = prefix + encryptXORCipher(DateTime.Now.ToString("MMdd"), EncryptionKeys) + randomInt + getLuhnCheckDigit(randomInt);

            return code;
        }

        #endregion

        public string generateCodeWithLuhnCheckDigit(int length)
        {
            string code = string.Empty;
            code = generateRandomValue(length - 1, 0, Constants.RandomValueType.INT);
            code = code + getLuhnCheckDigit(code);
            return code;
        }

        public bool IsValidCode(string code, string EncryptionKeys, int full_length, int random_length)
        {
            bool valid = false;
            if (code.Length == full_length)
            {
                try
                {
                    string month_and_year_decrypt = decryptXORCipher((code.Substring(0, (full_length - random_length))), EncryptionKeys); // --> Get 8 first of code and Decrypt
                    string formatYearMonth = "MMdd";
                    int iEncrypt = 0;
                    if (month_and_year_decrypt.Length == formatYearMonth.Length && // --> Compare length of month year string after decrypt with length of formatYearMonth
                        int.TryParse(month_and_year_decrypt, out iEncrypt))  // --> Check month year string after decrypt has must isnumeric
                    {
                        DateTime dEncrypt = DateTime.Now;
                        if (DateTime.TryParseExact(month_and_year_decrypt, formatYearMonth, CultureInfo.InvariantCulture, DateTimeStyles.None, out dEncrypt)) // --> Check month year string after decrypt has correct format year month ?
                        {
                            string last_6_digits = code.Substring(code.Length - random_length, random_length); // --> Get last 6 digits to check isLuhnCheckDigit
                            if (isLuhnCheckDigit(last_6_digits))
                            {
                                string _5digits = last_6_digits.Substring(0, 5); // --> Get first 5 digits of last_6_digits variable
                                string luhnDigit = last_6_digits.Substring(last_6_digits.Length - 1, 1); // --> Get last 1 digits of last_6_digits variable
                                if (getLuhnCheckDigit(_5digits) == luhnDigit) // --> Get LuhnCheck of 5 number and compare with last digits
                                {
                                    valid = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    valid = false;
                }
            }
            return valid;
        }

        //public string generateReferralCode(string columnName, string tableName, int length)
        //{
        //    int loop = 0;
        //    Boolean flag = false;
        //    string code = string.Empty;
        //    while (!flag)
        //    {
        //        code = generateRandomValue(length - 3, 0, Constants.RandomValueType.INT);
        //        code = generateRandomValue(2, 0, Constants.RandomValueType.STRING).ToUpper() + code + getLuhnCheckDigit(code);
        //        flag = dal.CheckExistingData(columnName, " " + tableName + " Where " + columnName + " = '" + code + "'");
        //        loop++;
        //    }
        //    return code;
        //}

        public int generateIterationPassword()
        {
            Random random = new Random();
            return random.Next(10000, 11000);
        }

        public bool isReferralCode(string referralCode, int length)
        {
            bool flag = false;
            if (referralCode.Length == length)
            {
                string prefix = referralCode.Substring(0, 2);
                if (Regex.Matches(referralCode, @"[A-Z]").Count == 2)
                {
                    string randomInt = referralCode.Substring(2, length - 2);
                    if (Regex.Matches(randomInt, @"[0-9]").Count == length - 2)
                    {
                        return isLuhnCheckDigit(randomInt);
                    }
                }
            }
            return flag;
        }

        public bool IsValidEmail(string email)
        {
            //regular expression pattern for valid email
            string pattern = @".*@.*\..*";
            //Regular expression object
            System.Text.RegularExpressions.Regex check = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(email))
            {
                valid = true;
            }
            else
            {
                valid = check.IsMatch(email);
            }
            return valid;
        }


        /// <summary>
        /// method for determining is the user provided a valid numeric
        /// </summary>
        /// <param name="sValue">numeric value to validate</param>
        /// <returns>true is valid, false if not valid</returns>
        public bool IsValidNumeric(string sValue)
        {
            //regular expression pattern for valid email
            string pattern = @"^\d+$";
            //Regular expression object
            System.Text.RegularExpressions.Regex check = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(sValue))
            {
                valid = true;
            }
            else
            {
                valid = check.IsMatch(sValue);
            }
            return valid;
        }

        public bool IsValidFloat(string sValue)
        {
            float fOutput = 0;
            return float.TryParse(sValue, out fOutput);

        }

        public static string getAppName()
        {
            Core_App.DAL dal = new Core_App.DAL();
            string sAppName = dal.GetDataByCommand("Select Value from tbl_SiteConfig where Name = 'App_Name'", "Value");
            if (sAppName != "0")
            {
                return sAppName;
            }
            else
            {
                return "";
            }
        }

        private static string site_url = "http://codigo.sg/";
        public static string getAppURL()
        {
            Core_App.DAL dal = new Core_App.DAL();
            string sSiteURL = dal.GetDataByCommand("Select Value from tbl_SiteConfig where Name = 'App_URL'", "Value");
            if (sSiteURL != "0")
            {
                return sSiteURL;
            }
            else
            {
                return site_url;
            }
        }

        public static string getTaxRate()
        {
            Core_App.DAL dal = new Core_App.DAL();
            string value = dal.GetDataByCommand("Select [dbo].[fn_GetConfig_TaxRate]() As Value", "Value");
            if (value != "0")
            {
                return value;
            }
            else
            {
                return value;
            }
        }

        public static string getMainAppURL()
        {
            Core_App.DAL dal = new Core_App.DAL();
            string url = dal.GetDataByCommand("Select [dbo].[fn_Get_Main_App_Url]() As Value", "Value");
            if (url != "0")
            {
                return url;
            }
            else
            {
                return "";
            }
        }

        public static string getLogoUrl()
        {
            Core_App.DAL dal = new Core_App.DAL();
            string url = dal.GetDataByCommand("Select [dbo].[fn_Get_Image_Url]('', '', 'logo.png', 0) As Value", "Value");
            if (url != "0")
            {
                return url;
            }
            else
            {
                return "";
            }
        }

        const string format_number = "{0:N2}";
        public double formatNumber(object obj)
        {
            double price = 0;
            if (obj.ToString() != "")
            {
                if (double.TryParse(obj.ToString(), out price))
                {
                    return double.Parse(String.Format(format_number, obj));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        const string format_price = "{0:$#,##0.##}";
        public string formatPrice(object obj)
        {
            double price = 0;
            if (obj.ToString() != "")
            {
                if (double.TryParse(obj.ToString(), out price))
                {
                    return String.Format(format_price, price);
                }
                else
                {
                    return "$0";
                }
            }
            else
            {
                return "$0";
            }
        }

        public string formatDate(object obj)
        {
            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                return string.Format("{0:dd MMM yyyy HH:mm:ss}", Convert.ToDateTime(obj.ToString()));
            }
            else
            {
                return string.Empty;
            }
        }

        const string format_date_no_time = "MMM dd yyyy";
        public string formatDateNoTime(object obj)
        {
            return formatDateTime(obj, format_date_no_time);
        }

        //public string formatDateTime(object obj, string format)
        //{
        //    if (string.IsNullOrEmpty(obj.ToString()))
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        DateTime date = new DateTime();
        //        if (DateTime.TryParse(obj.ToString(), out date))
        //        {
        //            return date.ToString(format);
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}

        public static string formatDateTime(object obj, string format = "{0:yyyy-MM-ddTHH:mm:ss}")
        {
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.ToString()))
                {
                    if (format == "{0:yyyy-MM-ddTHH:mm:ss}")
                    {
                        return string.Format("{0:yyyy-MM-ddTHH:mm:ss}", Convert.ToDateTime(obj.ToString()));
                    }
                    else
                    {
                        DateTime date = new DateTime();
                        if (DateTime.TryParse(obj.ToString(), out date))
                        {
                            return date.ToString(format);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static DateTime getExpireDateAccessToken()
        {
            return DateTime.Now.AddDays(Constants.TIME_EXPIRE_ACCESS_TOKEN);
        }

        public string setGenderText(string gender)
        {
            if (gender.ToLower() == "true" || gender == "1")
            {
                return "male";
            }
            else if (gender.ToLower() == "false" || gender == "0")
            {
                return "female";
            }
            else
            {
                return "";
            }
        }

        public bool setGenderNumber(string gender)
        {
            if (gender.ToLower() == "male")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string setRelationshipText(string relationship)
        {
            if (relationship.ToLower() == "1")
            {
                return "parent";
            }
            else if (relationship.ToLower() == "2")
            {
                return "guardian";
            }
            else if (relationship.ToLower() == "3")
            {
                return "mother";
            }
            else if (relationship.ToLower() == "4")
            {
                return "father";
            }
            else
            {
                return "";
            }
        }

        public int setRelationshipNumber(string relationship)
        {
            if (relationship.ToLower() == "parent")
            {
                return 1;
            }
            else if (relationship.ToLower() == "guardian")
            {
                return 2;
            }
            else if (relationship.ToLower() == "mother")
            {
                return 3;
            }
            else if (relationship.ToLower() == "father")
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        public string getImageUrlAPI(string account_id, string role_name, string image_id, string image_type)
        {
            StringBuilder sbImageUrl = new StringBuilder();
            if (role_name == Constants.API_ROLE_NAME.MEMBER ||
                role_name == Constants.API_ROLE_NAME.DOCTOR ||
                role_name == Constants.API_ROLE_NAME.DEV ||
                role_name == Constants.API_ROLE_NAME.ADMIN ||
                role_name == Constants.API_ROLE_NAME.DRIVER ||
                role_name == Constants.API_ROLE_NAME.PHARMACY)
            {

                sbImageUrl.Append(getAPIURL() + "/pub/Component/ViewImageInCMS?");
                sbImageUrl.Append("role=" + QueryStringEncryption.Encrypt(role_name));
                if (account_id != "")
                {
                    sbImageUrl.Append("&acc_id=" + QueryStringEncryption.Encrypt(account_id));
                }
                if (image_id != null && image_id != "" && image_id != "0")
                {
                    sbImageUrl.Append("&img_id=" + QueryStringEncryption.Encrypt(image_id.ToString()));
                }
                sbImageUrl.Append("&img_type=" + QueryStringEncryption.Encrypt(image_type.ToString()));
                sbImageUrl.Append("&tick=" + DateTime.Now.Ticks.ToString());
            }
            return sbImageUrl.ToString();
        }

        public static string getAPIURL()
        {
            string API_URL = getMainAppURL();
            API_URL = API_URL + Constants.VERSION;
            return API_URL;
        }

        public static string Http_Post(string url, string apiName, string authorization, System.Collections.Specialized.NameValueCollection reqParam, bool isAuthorization = true, string username = "", string password = "")
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                if (isAuthorization == true)
                {
                    client.Headers["Authorization"] = authorization;
                }
                else
                {
                    client.Headers["Username"] = username;
                    client.Headers["Password"] = password;
                }
                
                if (!string.IsNullOrEmpty(apiName))
                {
                    url = url + apiName;
                }
                byte[] responseBytes = client.UploadValues(url, "POST", reqParam);
                string responseBody = Encoding.UTF8.GetString(responseBytes);
                return responseBody;
            }
        }

        public static string Http_Get(string url, string apiName, string authorization, Dictionary<string, string> reqParams)
        {
            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(url);
            sbUrl.Append(apiName);
            int i = 0;
            if (reqParams != null)
            {
                foreach (KeyValuePair<string, string> item in reqParams)
                {
                    if (i == 0)
                    {
                        sbUrl.Append(item.Key + "=" + item.Value);
                    }
                    else
                    {
                        sbUrl.Append("&" + item.Key + "=" + item.Value);
                    }
                    i++;
                }
            }
            

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sbUrl.ToString());
            request.Method = "GET";
            if (authorization != "")
            {
                request.Headers["Authorization"] = authorization;
            }
            

            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        #region Payment

        public static string generalSha256(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            SHA256Managed sha256Managed = new SHA256Managed();
            byte[] hash = sha256Managed.ComputeHash(data);
            return ByteArrayToString(hash);
        }

        public static string getRefHashToPayment(string ref_code, string booking_code, object grand_total)
        {
            return Utilities.generalSha256(String.Format("ref={0}&code={1}&grand_total={2}", ref_code, booking_code, grand_total));
        }

        public static string GetPaymentGatewayUrl(string id, int type)
        {
            if (id != "")
            {
                string actionName = "Pay";
                if (type == 2)
                {
                    actionName = "Repay";
                }
                return "PaymentGateway/" + actionName + "?id=" + Core_App.BL.QueryStringEncryption.Encrypt(id) + "&refHash=";
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region QR Code

        public static string getRefHashToScanQRCodeOfDriver(string app_code, string phone, string ref_code)
        {
            return Utilities.generalSha256(String.Format("app={0}&phone={1}&ref={2}", app_code, phone, ref_code));
        }

        public static string getRefHashToScanQRCodeOfPharmacy(string app_code, string phone, string ref_code)
        {
            return Utilities.generalSha256(String.Format("app={0}&phone={1}&ref={2}", app_code, phone, ref_code));
        }

        public static string GetQRCodeUrl(object id, int type)
        {
            if (id.ToString() != "")
            {
                return Utilities.getAPIURL() + "/pub/Component/ViewImageQCCode/?img_id=" + Core_App.BL.QueryStringEncryption.Encrypt(id.ToString());
            }
            else
            {
                return "";
            }
        }

        private static string formatMonthYearDate = "MMyydd";
        private static int prefixLength = 5;
        public static string generateQRCode(string columnName, string tableName, string codition, string bookingNo, string encryptionKeys, int length)
        {
            Core_App.DAL dal = new Core_App.DAL();
            Utilities until = new Utilities();

            int loop = 0;
            Boolean flag = false;
            string code = string.Empty;

            while (!flag)
            {
                string randomInt = generateRandomValue(length, 0, Constants.RandomValueType.INT);
                randomInt = randomInt + getLuhnCheckDigit(randomInt);

                //bookingNo = bookingNo.Substring(bookingNo.Length - prefixLength, prefixLength);
                code = bookingNo + "-" + until.encryptXORCipher(DateTime.Now.ToString(Utilities.formatMonthYearDate), encryptionKeys) + "-" + randomInt;

                flag = dal.CheckExistingData(columnName, " " + tableName + " Where " + columnName + " = '" + code + "'" + codition);
                loop++;
            }
            return code;
        }

        public static bool IsValidQRCode(string code, string decryptionKeys, int length)
        {
            System.Text.RegularExpressions.Regex checkValid = new System.Text.RegularExpressions.Regex(@"^([0-9]{13})(-([0-9]){12})-([0-9]{7})$", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            if (checkValid.IsMatch(code))
            {
                string[] codes = code.Split("-".ToCharArray());
                string booking_code = codes[0];
                string month_year_date_with_enscrypt = codes[1];
                string random_number = codes[2];

                if (isLuhnCheckDigit(random_number))
                {
                    try
                    {
                        Utilities until = new Utilities();
                        string month_year_date_decrypt = until.decryptXORCipher(month_year_date_with_enscrypt, decryptionKeys);
                        DateTime dEncrypt = DateTime.Now;
                        if (DateTime.TryParseExact(month_year_date_decrypt, formatMonthYearDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dEncrypt)) // --> Check month year date string after decrypt has correct format month year date  ?
                        {
                            //string last_6_digits = booking_code.Substring(booking_code.Length - (length + 1), (length + 1));
                            //if (isLuhnCheckDigit(last_6_digits))
                            //{
                            //    return true;
                            //}
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Corporate Info
        public static bool getPayForConsultFee(object minimum_co_payment)
        {
            float value = 0;
            float.TryParse(minimum_co_payment.ToString(), out value);
            if (value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string getCorporateDescription(object description, object minimum_co_payment, object consult_fee)
        {
            string value = description.ToString();
            float minimum_co_payment_amount = 0;
            float.TryParse(minimum_co_payment.ToString(), out minimum_co_payment_amount);
            float consult_amount = 0;
            float.TryParse(consult_fee.ToString(), out consult_amount);
            if (minimum_co_payment_amount > 0 && consult_amount > 0)
            {
                value = String.Format(value, minimum_co_payment_amount, (consult_amount - minimum_co_payment_amount));
            }
            else
            {
                value = String.Format(value, 0, 0);
            }
            return value;
        }

        #endregion

        #region Subscription Info
        public static string getSubscriptionDescription()//object medication_fee
        {
            //string value = string.Empty;
            //float medication_amount = 0;
            //float.TryParse(medication_fee.ToString(), out medication_amount);
            //if (medication_amount > 0)
            //{
            //    value = String.Format(value, (medication_amount));
            //}
            //return value;
            return "Patient pay only medication fees.";
        }

        #endregion

        #region App Setting

        public static float GetStandardFee()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_StandardFee]() As Value", "Value"));
            return value;
        }

        public static float GetDeliveryFee()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_DeliveryFee]() As Value", "Value"));
            return value;
        }

        public static string GetWHCHotLine()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            string value = DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_WHC_HotLine]() As Value", "Value");
            return value;
        }

        public static string GetGSTREGNO()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            string value = DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_GST_REG_NO]() As Value", "Value");
            return value;
        }

        public static float GetConfig_TimeOutResetMedication()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_TimeOutResetMedication]() As Value", "Value"));
            return value;
        }

        public static float GetConfig_TimeOutSkipBooking()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_TimeOutSkipBooking]() As Value", "Value"));
            return value * 60;
        }

        public static float GetConfig_TimeOutCosulation()
        {
            Core_App.DAL DAL = new Core_App.DAL();
            float value = float.Parse(DAL.GetDataByCommand("Select [dbo].[fn_GetConfig_TimeOutCosulation]() As Value", "Value"));
            return value;
        }

        #endregion
    }
}
