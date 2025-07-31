using API.BL;
using API.BO;
using Core_App.BL;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WhiteCoat.API.Providers
{
    public static class ContextHelper
    {

    }

    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        Utilities until_bl = new Utilities();
        private Core_App.DAL dal = new Core_App.DAL();
        private string account_type = string.Empty;
        List<dynamic> corporate_infos = null;

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //var allowedOrigin = "*";
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var header = context.Request.Headers.Get("Authorization");

            if (header != null)
            {
                var authHeader = AuthenticationHeaderValue.Parse(header);

                if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                    var parts = parameter.Split(':');
                    string userName, password;
                    userName = password = string.Empty;
                    if (parts.Length == 2)
                    {
                        userName = parts[0];
                        password = parts[1];
                    }

                    var form = await context.Request.ReadFormAsync();
                    account_type = form["account_type"];

                    if (
                            (
                                userName == Constants.AUTHENTICATION.MEMBER_USER_NAME.ToString() &
                                password == Constants.AUTHENTICATION.MEMBER_PASSWORD.ToString() &&
                                account_type == Constants.API_ROLE_NAME.MEMBER
                             ) ||
                             (
                                userName == Constants.AUTHENTICATION.DOCTOR_USER_NAME.ToString() &
                                password == Constants.AUTHENTICATION.DOCTOR_PASSWORD.ToString() &&
                                account_type == Constants.API_ROLE_NAME.DOCTOR
                             ) ||
                             (
                                userName == Constants.AUTHENTICATION.DRIVER_USER_NAME.ToString() &
                                password == Constants.AUTHENTICATION.DRIVER_PASSWORD.ToString() &&
                                account_type == Constants.API_ROLE_NAME.DRIVER
                             )
                        ||
                             (
                                userName == Constants.AUTHENTICATION.PHARMACY_USER_NAME.ToString() &
                                password == Constants.AUTHENTICATION.PHARMACY_PASSWORD.ToString() &&
                                account_type == Constants.API_ROLE_NAME.PHARMACY
                             )
                        )
                    {


                        string email = string.Empty, mem_id = string.Empty;
                        if (form["email"] != null)
                        {
                            email = form["email"];
                        }

                        string push_notification_token = string.Empty;
                        if (form["push_notification_token"] != null)
                        {
                            push_notification_token = form["push_notification_token"];
                        }

                        string device_type = string.Empty;
                        if (form["device_type"] != null)
                        {
                            device_type = form["device_type"];
                        }


                        if (account_type != Constants.API_ROLE_NAME.MEMBER &&
                            account_type != Constants.API_ROLE_NAME.DOCTOR &&
                            account_type != Constants.API_ROLE_NAME.DRIVER &&
                            account_type != Constants.API_ROLE_NAME.PHARMACY)
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), ErrorString.ACCOUNT_TYPE_INVALIDATE);
                            return;
                        }
                        if (Account_BL.convertToNumber(device_type) == -1 && account_type == Constants.API_ROLE_NAME.MEMBER)
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), "Device type must is ios or android");
                            return;
                        }
                        if (email.Length < 0)
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), "The email address field is required.");
                            return;
                        }
                        if (email.Length > 50)
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), "The email address does not exceed 50 characters");
                            return;
                        }
                        if (!until_bl.IsValidEmail(email))
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), "The email address is invalid ");
                            return;
                        }
                        if (context.Password.Length < 6)
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), "Password must be at least 6 characters");
                            return;
                        }
                        if (context.Password.Length > 50)
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), "The password does not exceed 50 characters");
                            return;
                        }

                        AccountInfo accountInfo = null;
                        ApiResult apiResult = new ApiResult(ErrorCodes.OK, ErrorString.OK, null);
                        if (account_type == Constants.API_ROLE_NAME.MEMBER)
                        {
                            Member_BL member_bl = new Member_BL();
                            apiResult = member_bl.custCheckLoginAttemp(email);
                            if (apiResult.errorCode == ErrorCodes.OK)
                            {
                                accountInfo = member_bl.Login(email, context.Password, Constants.API_ISSUE_REFRESH_TOKEN.MEMBER, push_notification_token, Account_BL.convertToNumber(device_type));
                            }
                        }
                        else if (account_type == Constants.API_ROLE_NAME.DRIVER)
                        {
                            Driver_BL driver_bl = new Driver_BL();
                            apiResult = driver_bl.CheckLoginAttemp(email);
                            if (apiResult.errorCode == ErrorCodes.OK)
                            {
                                accountInfo = driver_bl.Login(email, context.Password, Constants.API_ISSUE_REFRESH_TOKEN.DRIVER, push_notification_token, Account_BL.convertToNumber(device_type));
                            }
                        }
                        else if (account_type == Constants.API_ROLE_NAME.PHARMACY)
                        {
                            Pharmacy_BL pharmacy_bl = new Pharmacy_BL();
                            apiResult = pharmacy_bl.CheckLoginAttemp(email);
                            if (apiResult.errorCode == ErrorCodes.OK)
                            {
                                accountInfo = pharmacy_bl.Login(email, context.Password, Constants.API_ISSUE_REFRESH_TOKEN.PHARMACY, push_notification_token, Account_BL.convertToNumber(device_type));
                            }
                        }
                        else
                        {
                            Doctor_BL doctor_bl = new Doctor_BL();
                            accountInfo = doctor_bl.Login(email, context.Password, Constants.API_ISSUE_REFRESH_TOKEN.DOCTOR);// , push_notification_token, Account_BL.convertToNumber(device_type)
                        }

                        if (apiResult.errorCode != ErrorCodes.OK)
                        {
                            int errorCode = (int)ErrorCodes.BAD_REQUEST;
                            context.SetError(errorCode.ToString(), apiResult.message);
                        }
                        else if (accountInfo.STATUS == -125)
                        {
                            int errorCode = (int)ErrorCodes.UNEXPECTED;
                            context.SetError(errorCode.ToString(), ErrorString.UNEXPECTED);
                        }
                        else if (accountInfo.STATUS == -99 && account_type == Constants.API_ROLE_NAME.MEMBER)
                        {
                            int errorCode = (int)ErrorCodes.ACCOUNT_NOT_FOUND;
                            context.SetError(errorCode.ToString(), ErrorString.PLEASE_TRY_AGAIN_SIGN_UP);
                        }
                        else if (accountInfo.STATUS == -99)
                        {
                            int errorCode = (int)ErrorCodes.ACCOUNT_NOT_FOUND;
                            context.SetError(errorCode.ToString(), ErrorString.ACCOUNT_NOT_FOUND);
                        }
                        //else if (accountInfo.REQUEST_UPDATE_CONTACT_INFO == true && account_type != Constants.API_ROLE_NAME.MEMBER)
                        //{
                        //    int errorCode = (int)ErrorCodes.UPDATE_CONTACT_INFO;
                        //    context.SetError(errorCode.ToString(), ErrorString.UPDATE_CONTACT_INFO);
                        //}
                        else if (accountInfo.STATUS == 0 && account_type == Constants.API_ROLE_NAME.MEMBER)
                        {
                            DataSet dsInfo = dal.GetDataset("Select Mem_Email, mem_id From tbl_Member Where Mem_Email = '" + email + "'", "dsInfo");//, Mem_Parent_ID 

                            //string email = dsInfo.Tables[0].Rows[0]["Mem_Email"].ToString();
                            mem_id = dsInfo.Tables[0].Rows[0]["mem_id"].ToString();
                            //string parent_id = dsInfo.Tables[0].Rows[0]["Mem_Parent_ID"].ToString();

                            int errorCode = (int)ErrorCodes.ACCOUNT_NOT_ACTIVATE;
                            if (email == "")
                            {
                                errorCode = (int)ErrorCodes.UPDATE_CONTACT_INFO;
                            }

                            //string access_token = pub_Function.GetAccessToken(mem_id,
                            //                                                  accountInfo.REFRESH_TOKEN,
                            //                                                  bool.Parse(accountInfo.REQUEST_UPDATE_CONTACT_INFO.ToString()),
                            //                                                  accountInfo.REQUEST_ACTIVATE_ACCOUNT,
                            //                                                  ((accountInfo.FOR_CHILD_THAT_TURNED_21 > 0) ? true : false),
                            //                                                  "",
                            //                                                  account_type);

                            dynamic obj = new System.Dynamic.ExpandoObject();
                            obj.Email = email;
                            //obj.access_token = access_token;
                            if (account_type == Constants.API_ROLE_NAME.MEMBER)
                            {
                                obj.message = ErrorString.MEMBER_ACCOUNT_NOT_ACTIVATE;
                            }
                            else
                            {
                                obj.message = ErrorString.ACCOUNT_NOT_ACTIVATE;
                            }

                            string message = JsonConvert.SerializeObject(obj);
                            //string message = phone + "|" + ErrorString.ACCOUNT_NOT_ACTIVATE;

                            context.SetError(errorCode.ToString(), message);
                        }
                        else if (accountInfo.STATUS == 0 && account_type != Constants.API_ROLE_NAME.MEMBER)
                        {
                            int errorCode = (int)ErrorCodes.ACCOUNT_NOT_ACTIVATE;
                            context.SetError(errorCode.ToString(), ErrorString.ACCOUNT_NOT_ACTIVATE);
                        }
                        else if (accountInfo.STATUS == -1)
                        {
                            int errorCode = (int)ErrorCodes.ACCOUNT_LOCKED;
                            context.SetError(errorCode.ToString(), ErrorString.ACCOUNT_LOCKED);
                        }
                        else
                        {
                            if (account_type == Constants.API_ROLE_NAME.MEMBER)
                            {
                                var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, account_type));
                                oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountInfo.ID));
                                oAuthIdentity.AddClaim(new Claim("rfr-tkn", accountInfo.REFRESH_TOKEN));
                                oAuthIdentity.AddClaim(new Claim("request_update_contact_info", accountInfo.REQUEST_UPDATE_CONTACT_INFO.ToString()));
                                oAuthIdentity.AddClaim(new Claim("request_activate_account", (!accountInfo.REQUEST_ACTIVATE_ACCOUNT).ToString()));
                                oAuthIdentity.AddClaim(new Claim("for_child_that_turned_21", (accountInfo.FOR_CHILD_THAT_TURNED_21 > 0) ? "True" : "False"));

                                //DataSet dsInfo = dal.GetDataset("Select  CMG.CMG_ID As [id], CMG_name As name, CMG_address As [address], CMG_email As email, CMG_mobile As mobile, CMGM_ID As corporate_member_id From tbl_CorporateMemberGroup CMG Join tbl_CorporateMemberGroup_Member CMGM On CMG.CMG_ID = CMGM.CMG_ID Where Mem_ID = '" + Utilities.decryptRijndael(accountInfo.ID, Constants.RijndaelHashType.PRIVATE) + "' And CMGM_status = 1", "dsInfo");

                                //if (dsInfo.Tables.Count > 0 && dsInfo.Tables[0].Rows.Count > 0)
                                //{
                                //    corporate_infos = new List<dynamic>();
                                //    DataRow row = dsInfo.Tables[0].Rows[0];
                                //    dynamic corporate_info = new System.Dynamic.ExpandoObject();
                                //    corporate_info.name = row["name"].ToString();
                                //    corporate_infos.Add(corporate_info);
                                //}
                                //dsInfo.Dispose();

                                var ticket = new AuthenticationTicket(oAuthIdentity, null);
                                context.Validated(ticket);
                            }
                            else
                            {
                                var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, account_type));
                                oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, accountInfo.ID));
                                oAuthIdentity.AddClaim(new Claim("rfr-tkn", accountInfo.REFRESH_TOKEN));
                                if (account_type == Constants.API_ROLE_NAME.PHARMACY)
                                {
                                    oAuthIdentity.AddClaim(new Claim("type", accountInfo.TYPE));
                                }
                                var ticket = new AuthenticationTicket(oAuthIdentity, null);
                                context.Validated(ticket);
                            }
                        }
                    }
                    else
                    {
                        int errorCode = (int)ErrorCodes.UNAUTHORIZED;
                        context.SetError(errorCode.ToString(), ErrorString.UNAUTHORIZED);
                    }
                }
                else
                {
                    int errorCode = (int)ErrorCodes.UNAUTHORIZED;
                    context.SetError(errorCode.ToString(), ErrorString.UNAUTHORIZED);
                }
            }
            else
            {
                int errorCode = (int)ErrorCodes.UNAUTHORIZED;
                context.SetError(errorCode.ToString(), ErrorString.UNAUTHORIZED);
            }
            //return Task.FromResult<object>(null);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            //if (account_type == Constants.API_ROLE_NAME.MEMBER)
            //{
            //    context.AdditionalResponseParameters.Add("corporate", corporate_infos);
            //}

            return Task.FromResult<object>(null);
        }

    }
}