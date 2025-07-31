using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_App.BL
{
    public class Constants
    {
        //Key & salt for query string encryption 
        public static string QUERY_STRING_SALT = ConfigurationManager.AppSettings["QUERY_STRING_SALT"];
        public static string QUERY_STRING_KEY = ConfigurationManager.AppSettings["QUERY_STRING_KEY"];

        //Rijndael key & salt for public use
        public static string RIJNDAEL_PUB_SALT = ConfigurationManager.AppSettings["RIJNDAEL_PUB_SALT"];
        public static string RIJNDAEL_PUB_KEY = ConfigurationManager.AppSettings["RIJNDAEL_PUB_KEY"];

        //Rijndael key & salt for private use
        public static string RIJNDAEL_PRV_SALT = ConfigurationManager.AppSettings["RIJNDAEL_PRV_SALT"];
        public static string RIJNDAEL_PRV_KEY = ConfigurationManager.AppSettings["RIJNDAEL_PRV_KEY"];

        public class API_ISSUE_REFRESH_TOKEN
        {
            public static Boolean MEMBER = false;
            public static Boolean DOCTOR = true; //true: Single login only
            public static Boolean DRIVER = false; //true: Single login only
            public static Boolean PHARMACY = false; //true: Single login only
        }

        public class API_ROLE_NAME
        {
            public static string ADMIN = "admin";
            public static string MEMBER = "member";
            public static string DOCTOR = "doctor";
            public static string DRIVER = "driver";
            public static string PHARMACY = "pharmacy";
            public static string WEB = "web";
            public static string DEV = "dev";
            public static string STRIPE = "stripe";
            public static string PUBLIC = "public";
        }

        //public const string API_MEMBER_ROLE_NAME = "member";
        //public const string API_DOCTOR_ROLE_NAME = "doctor";
        //public const string API_DISPENSARY_ROLE_NAME = "dispensary";

        public static int CMS_PAGE_SIZE = 10;

        public static string CMS_ACTION_VIEW = "1";
        public static string CMS_ACTION_ADD = "2";
        public static string CMS_ACTION_EDIT = "3";
        public static string CMS_ACTION_DEL = "4";

        public enum RijndaelHashAlgorithm
        {
            MD5,
            SHA,
            SHA256,
            SHA512
        };

        public enum RijndaelHashType
        {
            PUBLIC,
            PRIVATE
        };

        public enum RandomValueType
        {
            INT,
            STRING,
            PASSWORD,
            ALL
        };

        public enum CheckValueType
        {
            FLOAT,
            INT,
            EMAIL
        };

        // - - - White Coat - - -

        public const string PROJECT_NAME = "AUClinic";
        public const string VERSION = "v1";
        public const string DELIMITER = ",";
        public const string AUTHENTICATION_TYPE = "Bearer";

        public static class AUTHENTICATION
        {
            public static string TYPE = "Bearer";
            public static string MEMBER_USER_NAME = "apiMember";
            public static string MEMBER_PASSWORD = "b?WCjrN9ng4";
            public static string DOCTOR_USER_NAME = "apiDoctor";
            public static string DOCTOR_PASSWORD = "N!DDw9DWC0c";
            public static string DRIVER_USER_NAME = "apiDriver";
            public static string DRIVER_PASSWORD = "A!DRi5DVE4r";
            public static string PHARMACY_USER_NAME = "apiPharmacy";
            public static string PHARMACY_PASSWORD = "A!PHa8RMC4Y";
            public static string WEB_USER_NAME = "apiWeb";
            public static string WEB_PASSWORD = "D!TMD17NBt9Y";
        }

        public class EncryptionKeys
        {
            public static string ADMIN = "2116";
            public static string MEMBER = "1610";
            public static string DOCTOR = "2721";
            public static string DRIVER = "2517";
            public static string PHARMACY = "5197";
            public static string BOOKING = "4916";
            public static string REFILL_MEDICATION_BOOKING = "2917";
            public static string ORDER = "5968";
            public static string SUPPLIER = "3526";
            public static string MEDICATION = "2426";
            public static string CORPORATE_MEMBER_GROUP = "2030";
            public static string CORPORATE_MEMBER_GROUP_MEMBER = "3117";
            public static string PAYMENT_TRANSACTION_ID = "15294010042017";
            public static string QR_CODE = "92191510";
            public static string REF_CODE_OF_QR_CODE_DRIVER = "1705092120";
            public static string REF_CODE_OF_QR_CODE_PHARMACY = "2309201705";
            public static string CARD_NO = "4517";
            public static string SUBSCRIPTION_PACKAGE = "4017";
            public static string SUBSCRIPTION_PACKAGE_MEMBER = "4217";
        }

        public class EMAIL_TEMPLATE_PATH
        {
            public static string API_ACTIVATE_ACCOUNT = "~/emailTemplate/Account_Activation.html";
            public static string API_REGISTER_ACCOUNT = "~/emailTemplate/Register_Account.html";
            public static string API_RECOVERY_PASSWORD = "~/emailTemplate/Account_Recovery.html";
            public static string API_INVITE_CHILD = "~/emailTemplate/Invite_Child.txt";
            public static string API_SEND_PASSWORD = "~/emailTemplate/Send_Password.txt";
            public static string API_SEND_SUPPORT = "~/emailTemplate/Send_Support.txt";
            public static string CMS_SEND_ORDER = "~/emailTemplate/Create_Order.txt";
            public static string CMS_ACCOUNT_LOCK = "~/emailTemplate/Account_Lock.txt";
            public static string CMS_ACCOUNT_LOGIN_FAILED = "~/emailTemplate/Account_Login_Failed.txt";
            public static string CMS_IP_ADDRESS_LOCK = "~/emailTemplate/Blocked_IP_Address.txt";
            public static string API_RESERVATION_CONFIRMATION_PATIENT = "~/emailTemplate/Reservation_Confirmation_Patient.html";
            public static string API_RESERVATION_CONFIRMATION_DOCTOR = "~/emailTemplate/Reservation_Confirmation_Doctor.html";
            public static string API_RESERVATION_CANCELLATION_PATIENT = "~/emailTemplate/Reservation_Cancellation_Patient.html";
            public static string API_RESERVATION_CANCELLATION_DOCTOR = "~/emailTemplate/Reservation_Cancellation_Doctor.html";
        }

        public class S3_Folder
        {
            public static string TEMPS = "Temps";
            public static string ADMIN = "Admins";
            public static string MEMBER = "Members";
            public static string DOCTOR = "Doctors";
            public static string DRIVER = "Drivers";
            public static string PHARMACY = "Pharmacys";
            public static string ADVERTISE = "Advertises";
            public static string DOCTOR_CATEGORY = "DoctorCategories";
            public static string STYLE = "Styles";
            public static string OTHER = "Others";
            public static string CORPORATE = "Corporates";
            public static string ClinicCategory = "ClinicCategories";
            public static string ClinicImages = "ClinicImages";
        }

        public class Image_Type
        {
            public static string ADMIN = "admin-profile";
            public static string MEMBER = "member";
            public static string DOCTOR_PROFILE = "doctor_profile";
            public static string DOCTOR_SIGNATURE = "doctor_singnature";
            public static string DRIVER_PROFILE = "driver_profile";
            public static string PHARMACY_PROFILE = "pharmacy_profile";
            public static string ADVERTISE = "advertise";
            public static string DOCTOR_CATEGORY = "doctor_category";
            public static string OTHER = "other";
            public static string ClinicCategory = "clinic_category";
            public static string ClinicImages = "clinic_images";
        }

        public class Image_Size
        {
            public static int NORMAL = 0;
            public static int THUMB_CMS = 1;
        }

        public class Image_Size_Name
        {
            public static string NORMAL = "";
            public static string THUMB_CMS = "thumb-cms-";
        }

        public class API_Image_Type
        {
            public static string MEM_PROFILE = "1";
            public static string MEM_FRONT_IDENTICATION_CARD = "1.1";
            public static string MEM_BACK_IDENTICATION_CARD = "1.2";
            public static string MEM_PASSPORT = "1.3";
            public static string MEM_BIRTH_CERTIFICATE = "1.4";
            public static string MEM_SIGNATURE = "1.5";
            public static string DOCTOR_PROFILE = "2";
            public static string DOCTOR_SIGNATURE = "2.1";
            public static string DRIVER_PROFILE = "5";
            public static string PHARMACY_PROFILE = "6";
            public static string ADVERTISE = "7";
            public static string DOCTOR_CATEGORY = "8";
            public static string ClinicCategory = "9";
            public static string ClinicImages = "10";

            public static string BOOKING_SYMPTOM = "3.1";
            public static string BOOKING_DRIVER_COLLECTED_MEDICINE = "3.2";
            public static string BOOKING_PATIENT_SIGNATURE = "3.3";
            public static string BOOKING_DRIVER_SIGNATURE = "3.4";
            public static string ADMIN_PROFILE = "4";
            public static string TEMPS = "-1";
        }

        public class SignalR_Type
        {
            public static string DOCTOR = Constants.PROJECT_NAME + "_doctor";
            public static string CMS = Constants.PROJECT_NAME + "_cms";
            public static string MEMBER = Constants.PROJECT_NAME + "_member";
        }

        public class SignalR_Action
        {

            public static int DRIVER_INDICATED_PATIENT_INVALID_QR_CODE = -5;
            public static int DRIVER_INDICATED_PATIENT_INVALID_IC = -4;
            public static int DRIVER_INDICATED_PATIENT_NOT_HOME = -3;
            public static int DRIVER_INDICATED_PATIENT_UNCONTACABLE = -2;
            public static int MEMBER_CANCEL_BOOKING = -1;
            public static int MEMBER_CREATE_BOOKING = 1;
            public static int MEMBER_REALY_FOR_CONSULTING = 2;
            public static int MEMBER_RECONECT_TO_DOCTOR = 3;
            public static int MEMBER_PAY_REFLL_MEDICATION = 4;
            public static int MEMBER_REGISTER_DEVICE = 7;
            public static int MEMBER_REGISTER_ACCOUNT = 6;
            public static int MEMBER_PAID_CONSULATION = 5;
            public static int MEMBER_CREATE_BOOKING_ON_QUEUE = 8;
            public static int DOCTOR_COMPLETED_CONSULTING = 10;
            public static int QUEUE_NO_HAS_BEEN_CHANGED = 11;
            public static int MEMBER_COMPLETE_CONSULATION = 98;
            public static int DRIVER_COMPLETE_DELIVERY = 99;


            public static int DOCTOR_READY = 21;
            public static int DOCTOR_UPDATED_PRESCRIPTION = 22;
            public static int DOCTOR_ENDCALL = 23;
            public static int DOCTOR_COMPLETED_BOOKING = 24;

            public static int MEMBER_TEST_BROADCAST = -9999;
        }

        public class API_Notification_Type
        {
            //public static int DOCTOR_CANCELED_CONSULTING = -4;
            //public static int DOCTOR_SKIPPED_CONSULTING = -3;
            public static int MEMBER_UNABLE_CONNECT_TO_DOCTOR = -2;
            public static int TIME_CONSULTING_EXPIRED = -1;
            //public static int DOCTOR_STARTED_CONSULTING = 1;
            //public static int DOCTOR_UPDATE_PRESCRIPTION = 2;
            //public static int DOCTOR_END_CALL_CONSULTING = 3;
            //public static int DOCTOR_COMPLETED_CONSULTING = 4;

            public class PATIENT
            {
                public static int CORPORATE_REMOVED_STATE = -10;
                public static int IC_INVALID = -9;
                public static int QR_CODE_INVALID = -8;
                public static int NOT_HOME = -7;
                public static int DRIVER_UNCONTACTABLE = -6;
                public static int DRIVER_START_UNCONTACTABLE = -5;
                public static int DOCTOR_CANCELED_CONSULTING = -4;
                public static int DOCTOR_SKIPPED_CONSULTING = -3;

                public static int DOCTOR_STARTED_CONSULTING = 1;
                public static int DOCTOR_UPDATE_PRESCRIPTION = 2;
                public static int DOCTOR_END_CALL_CONSULTING = 3;
                public static int DOCTOR_COMPLETED_CONSULTING = 4;
                public static int DRIVER_CONTACTED = 5;
                public static int DRIVER_MEDICINE_COLLECTED = 6;
                public static int DRIVER_STARTED_DELIVERING = 7;
                public static int DRIVER_HAS_SCANED_QR_CODE = 8;
                public static int DRIVER_HAS_COMPLETED_DELIVER = 9;

                public static int PHARMACY_HAS_SCANED_QR_CODE = 10;
                public static int PHARMACY_HAS_CONFIRMED_IC = 11;
                public static int PHARMACY_HAS_COMPLETED_COLLECTION = 12;
                public static int QUEUE_NO_HAS_BEEN_CHANGED = 13;

                public static int CORPORATE_ACTIVATED_STATE = 14;
                public static int DRIVER_IS_ASSIGNED = 15;
                public static int DOCTOR_STARTED_RESERVATION = 16;
            }

            public class DRIVER
            {
                public static int REASSIGN_JOB_TO_DRIVER_OTHER = -1;
                public static int ASSIGN_NEW_JOB = 1;
                public static int PHARMACY_HAS_SCANED_QR_CODE = 2;
                public static int PHARMACY_MARK_IC_VALID = 3;
                public static int PHARMACY_COMPLETE_COLLECTION = 4;
                public static int PHARMACY_HAS_CONFIRMED_IC = 5;
                public static int PHARMACY_HAS_COMPLETED_COLLECTION = 6;
            }
        }

        public class API_Notification_Type_Message
        {

            public static string MEMBER_CANCEL_BOOKING = "Sorry, patient has canceled the consultant";
            public static string MEMBER_UNABLE_CONNECT_TO_DOCTOR = "Sorry, we are unable to connect to your doctor at this moment."; //"Unable to connect to {0}";

            public class PATIENT
            {
                public static string DRIVER_HAS_COMPLETED_DELIVER = "Your medication has been delivered. Thank you for choosing WhiteCoat as the doctor in your family."; //"Your medication has been delivered!";
                public static string IC_INVALID = "Sorry, the identification document you have provided is invalid. Please contact WhiteCoat during our office hours for assistance in the completion of this delivery."; //"Driver has indicated your delivery as IC invalid.";
                public static string QR_CODE_INVALID = "Sorry, the QR code you have provided is invalid. Please contact WhiteCoat during our office hours for assistance in the completion of this delivery."; //"Driver has indicated your delivery as QR code invalid.";
                public static string NOT_HOME = "Hi, we were at your delivery destination and there was no one there. Please contact WhiteCoat at {0} asap."; //"Driver was unable to deliver your order.";
                public static string DRIVER_HAS_SCANED_QR_CODE = "Driver has scanned QR code successfully.";
                public static string DRIVER_IS_ASSIGNED = "Your driver, {0}, has been assigned to pick up your medication. You will receive a notification when it is on its way to you.";
                public static string DRIVER_STARTED_DELIVERING = "Your driver, {0}, has collected your medication and will deliver it soon. Please have your identification document on standby for verification."; //"Your delivery is on the way.";
                public static string DRIVER_MEDICINE_COLLECTED = "Driver has collected medicine.";
                public static string DRIVER_CONTACTED = "Your driver is on his way to collect your medicine now.";//"Driver has indicated your delivery as contacted.";
                public static string DRIVER_UNCONTACTABLE = "Hi, our driver was unable to reach you. Please contact WhiteCoat at {0} asap."; //"Driver cannot contact you.";
                public static string DRIVER_START_UNCONTACTABLE = "Driver started countdown for uncontactable.";
                public static string DOCTOR_CANCELED_CONSULTING = "Doctor {0} cancelled the consult."; //"Doctor has canceled cons0ultant."; //Constants.API_Notification_Type.PATIENT.DOCTOR_CANCELED_CONSULTING
                public static string DOCTOR_SKIPPED_CONSULTING = "Sorry, we were unable to reach you. To continue or cancel, please click here."; //"Doctor has skipped consultant.";
                public static string DOCTOR_STARTED_CONSULTING = "Your doctor is now ready to see you. Click here."; //"Consultant {0} has been started.";
                public static string DOCTOR_UPDATE_PRESCRIPTION = "Your prescription has been updated."; //"Doctor has been updated prescription.";
                public static string DOCTOR_END_CALL_CONSULTING = "Your call has ended. Please hold while your doctor wraps up your consultation."; //"Doctor end call.";
                public static string DOCTOR_COMPLETED_CONSULTING = "Your doctor has completed a review of your consultation. Any approved medical document(s) you have requested for are accessible now."; //"Consultant {0} has been set to completed.";
                public static string DOCTOR_STARTED_RESERVATION = "Doctor {0} is ready and waiting for you to join.";

                public static string PHARMACY_HAS_SCANED_QR_CODE = "QR code scan success."; //"Pharmacy has scanned QR code successfully.";
                public static string PHARMACY_HAS_CONFIRMED_IC = "IC verified."; //"Pharmacy has confirmed IC successfully.";
                public static string PHARMACY_HAS_COMPLETED_COLLECTION = "Your medicine is packed."; //"Pharmacy has completed collection.";
                //public static string QUEUE_NO_HAS_BEEN_CHANGED = "Your queue number has been updated.";
                public static string QUEUE_NO_HAS_BEEN_CHANGED = "Your estimated wait time is about {0} minute(s). Thank you for your patience.";
                //public static string QUEUE_NO_HAS_BEEN_CHANGED = "Your doctor is now ready to see you. Click here";

                public static string CORPORATE_ACTIVATED_STATE = "Hi, your account is now covered with your company's corporate plan. You may now proceed to consult our doctors with this account."; //"Your corporate account under {0} Company has become activated.";
                public static string CORPORATE_REMOVED_STATE = "Your account has been removed from this corporate plan."; //"Your corporate account under {0} Company has become inactive.";
            }

            public class DRIVER
            {
                public static string REASSIGN_JOB_TO_DRIVER_OTHER = "Sorry, this delivery has assigned to another driver.";
                public static string ASSIGN_NEW_JOB = "New delivery available!!";
                public static string PHARMACY_HAS_SCANED_QR_CODE = "Pharmacy has scanned QR code successfully.";
                public static string PHARMACY_MARK_IC_VALID = "Pharmacy has indicated your delivery as valid IC.";
                public static string PHARMACY_COMPLETE_COLLECTION = "Pharmacy has completed collection.";
                public static string PHARMACY_HAS_CONFIRMED_IC = "Pharmacy has confirmed IC successfully.";
                public static string PHARMACY_HAS_COMPLETED_COLLECTION = "Pharmacy has completed collection.";
            }
        }

        public class API_Notification_Type_Title
        {
            public static string CONSULTATION = "Consultation";
            public static string RESERVATION = "Reservation";
        }

        public class SCHEDULER_JOB
        {
            public static int CHECK_BOOKING_EXPIRED = -1;
        }

        public class DOCTYPE_TYPE
        {
            public static string WHITECOAT = "white_coat";
            public static string LOCUMS = "locums";
            public static string CLINICS = "clinics";
        }

        public class MEMBER_ACCOUNT_TYPE
        {
            public static string INDIVIDUAL = "individual";
            public static string CORPORATE = "corporate";
            public static string SUBSCRIPTION = "subscription";
        }

        public class CORPORATE_CODE_PAYMENT_TYPE
        {
            public static string CONSULTING_FEES_ONLY = "OPC";
            public static string CONSULTING_FEES_ONLY_WITH_CO_PAID = "OPCWCPPAID";
            public static string CONSULTING_FEES_AND_FEES_MEDICATION_FEES = "APCM";
            public static string CONSULTING_FEES_AND_FEES_MEDICATION_FEES_WITH_CO_PAID = "APCMWCPPAID";
        }

        public class DELIVERT_TYPE
        {
            public static string SELT_COLLECT = "self_collect";
            public static string DELIVER = "deliver";
        }

        public class SELF_COLLECT
        {
            public static string WHITECOAT = "whitecoat";
            public static string EXTERNAL = "external";
        }

        public class SEARCH_KEYWORD_HISTORY_GROUP
        {
            public static int BOOKING = 1;
            public static int DOCTOR = 2;
            public static int PATIENT = 3;
            public static int REFILL_MEDICATION = 4;
            public static int PRODUCT = 5;
            public static int SUPPLIES = 6;
            public static int CORPORATE = 7;
            public static int SUBSCRIPTION = 8;
            public static int DRIVER = 7;
        }

        public const string MESSAGE_SMS_SEND_PASSWORD = "Your " + PROJECT_NAME + " one time password is ";
        public const string MOBILE_CODE_SINGAPORE = "+65";


        public const double TIME_EXPIRE_ACCESS_TOKEN = 365;

        public const int LENGTH_ACTIVE_CODE = 4;
        public const int LENGTH_INVITE_CODE = 4;
        public const int LENGTH_RECOVERY_CODE = 6;
        public const int LENGTH_MEMBER_CODE = 6;
        public const int LENGTH_ORDER_CODE = 5;
        public const int LENGTH_ADMIN_CODE = 6;
        public const int LENGTH_DOCTOR_CODE = 6;
        public const int LENGTH_DRIVER_CODE = 6;
        public const int LENGTH_PHARMACY_CODE = 6;
        public const int LENGTH_CORPORATE_CODE = 6;
        public const int LENGTH_CORPORATE_CODE_MEMBER = 6;
        public const int LENGTH_BOOKING_CODE = 5;
        public const int LENGTH_SUPPLIER_CODE = 6;
        public const int LENGTH_MEDICATION_CODE = 6;
        public const int LENGTH_PAYMENT_TRANSACTION_ID = 6;
        public const int LENGTH_QR_CODE = 6;
        public const int LENGTH_REF_CODE_OF_QR_CODE = 6;
        public const int LENGTH_CARD_NO = 6;
        public const int LENGTH_SUBSCRIPTION_CODE = 6;
        public const int LENGTH_RANDOM_IMAGE_NAME = 5;
        public const int FULL_LENGTH_CODE = 14;

        public const int MIN_LENGTH_PASSWORD = 6;
        public const int MAX_LENGTH_PASSWORD = 25;
        public const int MAX_ITEM_SYSMTOMS_IMAGE = 4;
        public const int CACHED_MEDIUM = 36000; // 10 hours
        //public const string ROLE_DISPENSARY = "3eaa37c1-a9c0-464b-9254-819a04b28fd9";

        public class MEMBER_TYPE_ID
        {
            public static string NRIC_FIN = "nric_fin";
            //public static string NRIC_FIN_BACK = "";
            public static string PASSPORT = "passport";
            public static string BIRTH_CERTIFICATE = "birth_certificate";
        }

        public class MEMBER_TYPE_ID_NUMBER
        {
            public static int NRIC_FIN = 1;
            //public static string NRIC_FIN_BACK = "";
            public static int PASSPORT = 2;
            public static int BIRTH_CERTIFICATE = 3;
        }

        public class ACTION_UPDATE_CARD_BOOKING
        {
            public static int NORMAL = 1;
            public static int REFILL_MEDICATION = 2;
        }
    }
}