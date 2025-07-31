using System.Web.Mail;
using System;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using NLog;
namespace API.BL
{
    public class eMail_Helper
    {


        public static bool GMAIL_SMTP_Sender(string sDisplayName, string sTo, string sBCC, string sSubject, string sBody, Boolean bHTML, string sAttachmentPath)
        {


            pub_Function pub_Function = new pub_Function();
            string sUserName = string.Empty; string sPassword = string.Empty; string sSMTP = string.Empty, sSMTP_User_Name = string.Empty; int iPortNo = 0;
            pub_Function.getEmailConfig(ref sUserName, ref sPassword, ref sSMTP, ref iPortNo, ref sBCC, ref sSMTP_User_Name);

            if (sUserName != string.Empty & sPassword != string.Empty & sSMTP != string.Empty & iPortNo != 0)
            {
                System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
                objEmail.To.Add(sTo);
                //if (!string.IsNullOrEmpty(sSMTP_User_Name))
                //{
                //    objEmail.From = new MailAddress(sUserName);
                //}
                //else
                //{
                //    objEmail.From = new MailAddress(sUserName, sDisplayName, System.Text.Encoding.UTF8);
                //}
                objEmail.From = new MailAddress(sUserName, sDisplayName, System.Text.Encoding.UTF8);
                objEmail.Subject = sSubject;
                objEmail.Body = @sBody;

                objEmail.SubjectEncoding = System.Text.Encoding.UTF8;
                objEmail.BodyEncoding = System.Text.Encoding.UTF8;
                objEmail.IsBodyHtml = bHTML;
                objEmail.Priority = System.Net.Mail.MailPriority.High;

                //if (sBCC.Trim() != "")
                //{
                //    objEmail.Bcc.Add(sBCC);
                //}
                if (sAttachmentPath.Trim() != "")
                {
                    Attachment MyAttachment = new Attachment(sAttachmentPath);
                    objEmail.Attachments.Add(MyAttachment);
                }

                SmtpClient client = new SmtpClient();
                if (sPassword != "-1")
                {
                    if (!string.IsNullOrEmpty(sSMTP_User_Name))
                    {
                        client.Credentials = new System.Net.NetworkCredential(sSMTP_User_Name, sPassword);
                    }
                    else
                    {
                        client.Credentials = new System.Net.NetworkCredential(sUserName, sPassword);
                    }
                    client.EnableSsl = true;
                    client.Port = iPortNo;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                }
                client.Host = sSMTP;

                try
                {
                    client.Send(objEmail);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger logger = LogManager.GetLogger("Write-Log");
                    logger.Error("Error send email: " + ex.ToString());
                    return false;
                }
            }
            else
            {

                return false;
            }
        }

        //public static bool GMAIL_SMTP_Sender_Feedback(string sDisplayName, string sTo, string sFrom, string sBCC, string sSubject, string sBody, Boolean bHTML, string sAttachmentPath)
        //{
        //    //string sUserName = ConfigurationManager.AppSettings["email_Username"].ToString();
        //    //string sPassword = getDecrypt_Account(ConfigurationManager.AppSettings["email_Password"].ToString());
        //    //string sSMTP = ConfigurationManager.AppSettings["SMTP"].ToString();
        //    //int iPortNo = int.Parse(ConfigurationManager.AppSettings["PortNo"].ToString());

        //    pub_Function pub_Function = new pub_Function();
        //    string sUserName = string.Empty; string sPassword = string.Empty; string sSMTP = string.Empty, sSMTP_User_Name = string.Empty; int iPortNo = 0;
        //    pub_Function.getEmailConfig(ref sUserName, ref sPassword, ref sSMTP, ref iPortNo, ref sBCC, ref sSMTP_User_Name);
        //    if (sUserName != string.Empty & sPassword != string.Empty & sSMTP != string.Empty & iPortNo != 0)
        //    {
        //        System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
        //        objEmail.To.Add(sTo);
        //        if (sSMTP_User_Name == "")
        //        {
        //            objEmail.From = new MailAddress(sFrom, sDisplayName, System.Text.Encoding.UTF8);
        //        }
        //        else
        //        {
        //            objEmail.From = new MailAddress(sSMTP_User_Name);
        //        }
        //        objEmail.Subject = sSubject;
        //        objEmail.Body = sBody;

        //        objEmail.SubjectEncoding = System.Text.Encoding.UTF8;
        //        objEmail.BodyEncoding = System.Text.Encoding.UTF8;
        //        objEmail.IsBodyHtml = bHTML;
        //        //objEmail.Priority = System.Net.Mail.MailPriority.High;

        //        if (sBCC.Trim() != "")
        //        {
        //            objEmail.Bcc.Add(sBCC);
        //        }
        //        if (sAttachmentPath.Trim() != "")
        //        {
        //            Attachment MyAttachment = new Attachment(sAttachmentPath);
        //            objEmail.Attachments.Add(MyAttachment);
        //        }

        //        SmtpClient client = new SmtpClient();
        //        client.Credentials = new System.Net.NetworkCredential(sUserName, sPassword);

        //        client.Port = iPortNo;
        //        client.Host = sSMTP;
        //        client.EnableSsl = true;
        //        try
        //        {
        //            client.Send(objEmail);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static string GMAIL_SMTP_Sender_Test(string sDisplayName, string sTo, string sBCC, string sSubject, string sBody, Boolean bHTML, string sAttachmentPath)
        //{
        //    pub_Function pub_Function = new pub_Function();
        //    string sUserName = string.Empty; string sPassword = string.Empty; string sSMTP = string.Empty; int iPortNo = 0;
        //    //            pub_Function.getEmailConfig(ref sUserName, ref sPassword, ref sSMTP, ref iPortNo, ref sBCC);
        //    string mes = string.Empty;
        //    sUserName = "AKIAIZ5F7MXI6MSDUR2A";
        //    sPassword = "AsnUvqpgeBGSTK6bj7XZxGxCuQSe3e7730J1g77oz9s4";
        //    sSMTP = "email-smtp.us-east-1.amazonaws.com";
        //    iPortNo = 25;

        //    if (sUserName != string.Empty & sPassword != string.Empty & sSMTP != string.Empty & iPortNo != 0)
        //    {
        //        System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
        //        objEmail.To.Add(sTo);
        //        objEmail.From = new MailAddress("do-not-reply@fastfast.delivery");
        //        objEmail.Subject = sSubject;
        //        objEmail.Body = sBody;

        //        objEmail.SubjectEncoding = System.Text.Encoding.UTF8;
        //        objEmail.BodyEncoding = System.Text.Encoding.UTF8;
        //        objEmail.IsBodyHtml = bHTML;
        //        objEmail.Priority = System.Net.Mail.MailPriority.High;

        //        if (sBCC.Trim() != "")
        //        {
        //            objEmail.Bcc.Add(sBCC);
        //        }
        //        if (sAttachmentPath.Trim() != "")
        //        {
        //            Attachment MyAttachment = new Attachment(sAttachmentPath);
        //            objEmail.Attachments.Add(MyAttachment);
        //        }

        //        SmtpClient client = new SmtpClient();
        //        if (sPassword != "-1")
        //        {
        //            client.Credentials = new System.Net.NetworkCredential(sUserName, sPassword);
        //            client.EnableSsl = true;
        //            client.Port = iPortNo;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        }
        //        client.Host = sSMTP;

        //        try
        //        {
        //            client.Send(objEmail);
        //            mes = "Success";
        //        }
        //        catch (Exception ex)
        //        {
        //            mes = ex.Message;
        //        }
        //        return mes;
        //    }
        //    else
        //    {
        //        return mes;
        //    }
        //}

        public static bool GMAIL_SMTP_Sender_Width_Image(string sDisplayName, string sTo, string sEmail_2, string sBCC, string sSubject, string sBody, Boolean bHTML, string sAttachmentPath, string img_logo, string img_delivery, string img_pacel, string img_bg, string img_driver)
        {
            pub_Function pub_Function = new pub_Function();
            string sUserName = string.Empty; string sPassword = string.Empty; string sSMTP = string.Empty, sSMTP_User_Name = string.Empty; int iPortNo = 0;
            pub_Function.getEmailConfig(ref sUserName, ref sPassword, ref sSMTP, ref iPortNo, ref sBCC, ref sSMTP_User_Name);

            if (sUserName != string.Empty & sPassword != string.Empty & sSMTP != string.Empty & iPortNo != 0)
            {
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(@sBody, null, MediaTypeNames.Text.Html);

                LinkedResource lr_logo = new LinkedResource(img_logo, MediaTypeNames.Image.Jpeg);
                lr_logo.ContentId = "logo";
                avHtml.LinkedResources.Add(lr_logo);

                LinkedResource lr_delivery = new LinkedResource(img_delivery, MediaTypeNames.Image.Jpeg);
                lr_delivery.ContentId = "delivery";
                avHtml.LinkedResources.Add(lr_delivery);

                LinkedResource lr_parcel = new LinkedResource(img_pacel, MediaTypeNames.Image.Jpeg);
                lr_parcel.ContentId = "parcel";
                avHtml.LinkedResources.Add(lr_parcel);

                LinkedResource lr_bg = new LinkedResource(img_bg, MediaTypeNames.Image.Jpeg);
                lr_bg.ContentId = "bg";
                avHtml.LinkedResources.Add(lr_bg);

                //LinkedResource lr_driver = new LinkedResource(img_driver, MediaTypeNames.Image.Jpeg);
                //lr_driver.ContentId = "driver";
                //avHtml.LinkedResources.Add(lr_driver);

                System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();
                objEmail.To.Add(sTo);
                if (!string.IsNullOrEmpty(sEmail_2))
                {
                    objEmail.To.Add(sEmail_2);
                }


                objEmail.From = new MailAddress(sUserName, sDisplayName, System.Text.Encoding.UTF8);
                objEmail.Subject = sSubject;
                objEmail.AlternateViews.Add(avHtml);

                objEmail.SubjectEncoding = System.Text.Encoding.UTF8;
                objEmail.BodyEncoding = System.Text.Encoding.UTF8;
                objEmail.IsBodyHtml = bHTML;
                objEmail.Priority = System.Net.Mail.MailPriority.High;

                if (sBCC.Trim() != "")
                {
                    objEmail.Bcc.Add(sBCC);
                }
                if (sAttachmentPath.Trim() != "")
                {
                    Attachment MyAttachment = new Attachment(sAttachmentPath);
                    objEmail.Attachments.Add(MyAttachment);
                }

                SmtpClient client = new SmtpClient();
                if (sPassword != "-1")
                {
                    if (!string.IsNullOrEmpty(sSMTP_User_Name))
                    {
                        client.Credentials = new System.Net.NetworkCredential(sSMTP_User_Name, sPassword);
                    }
                    else
                    {
                        client.Credentials = new System.Net.NetworkCredential(sUserName, sPassword);
                    }
                    client.EnableSsl = true;
                    client.Port = iPortNo;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                }
                client.Host = sSMTP;

                try
                {
                    client.Send(objEmail);

                    lr_logo.Dispose();
                    lr_parcel.Dispose();
                    lr_delivery.Dispose();
                    lr_bg.Dispose();
                    avHtml.Dispose();
                    objEmail.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                    if (System.IO.File.Exists(img_driver))
                    {
                        System.IO.File.Delete(img_driver);
                    }
                    return false;
                }
            }
            else
            {
                if (System.IO.File.Exists(img_driver))
                {
                    System.IO.File.Delete(img_driver);
                }
                return false;
            }
        }
    }


}
