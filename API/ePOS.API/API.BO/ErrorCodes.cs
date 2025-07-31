using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.BO
{
    public enum ErrorCodes
    {
        /// <summary>
        /// No error
        /// </summary>
        OK = 0,
        /// <summary>
        /// Bad request
        /// </summary>
        BAD_REQUEST = 400,
        /// <summary>
        /// Basic authentication invalidate
        /// </summary>
        UNAUTHORIZED = 401,
        /// <summary>
        /// Could not find content
        /// </summary>
        NOT_FOUND = 404,
        /// <summary>
        /// UnExpected
        /// </summary>
        UNEXPECTED = 405,
        /// <summary>
        /// Account not found
        /// </summary>
        ACCOUNT_NOT_FOUND = 406,
        /// <summary>
        /// Account has been blocked
        /// </summary>
        ACCOUNT_LOCKED = 407,
        /// <summary>
        /// Token invalidate or expired
        /// </summary>
        TOKEN_INVALIDATE = 408,
        /// <summary>
        /// Token changed
        /// </summary>
        TOKEN_CHANGED = 409,



        // - - - - - WhiteCost - - - - - 
        /// <summary>
        /// The mobile number already exists
        /// </summary>
        MOBILE_ALREADY_EXISTS = 410,
        /// <summary>
        /// The email already exists
        /// </summary>
        EMAIL_ALREADY_EXISTS = 411,
        /// <summary>
        /// Account has not been activated
        /// </summary>
        ACCOUNT_NOT_ACTIVATE = 412,
        /// <summary>
        /// Account has been activated
        /// </summary>
        ACCOUNT_ALREADY_ACTIVATE = 413,

        /// <summary>
        /// Update stripe card token
        /// </summary>
        UPDATE_STRIPE_CARD_TOKEN = 414,

        /// <summary>
        /// Booking processing(When Create booking Normal or Create booking refill)
        /// </summary>
        BOOKING_PROCESSING = 415,

        /// <summary>
        /// Update Contact Info
        /// </summary>
        UPDATE_CONTACT_INFO = 416,

        /// <summary>
        /// Booking processed(Complete booking)
        /// </summary>
        BOOKING_PROCESSED = 417,

        /// <summary>
        /// Doctor skipped consult
        /// </summary>
        DOCTOR_SKIPPED = 418,

        /// <summary>
        /// Doctor haven't skipped consult
        /// </summary>
        DOCTOR_NOT_SKIPPED = 419,

        /// <summary>
        /// Request again otp code
        /// </summary>
        MEMBER_REQUEST_AGAIN_OTP_CODE = 420,

        /// <summary>
        /// Requestd otp code
        /// </summary>
        MEMBER_REQUESTED_OTP_CODE = 421,
    }

    public class ErrorString
    {
        public static string OK = "Success";
        public static string ACCOUNT_LOCKED = "Your account has been locked.";
        public static string ACCOUNT_TYPE_INVALIDATE = "Account_Type must be member or doctor.";
        public static string ACCOUNT_NOT_FOUND = "Account could not be found.";
        
        public static string UNAUTHORIZED = "Request require authorization";
        public static string BAD_AUTHORIZE = "Authorization has been denied for this request.";
        public static string BAD_REQUEST = "Parameter not correct.";
        public static string UNEXPECTED = "Sorry. An unexpected error has occurred.";
        public static string METHOD_NOT_ALLOWED = "A request was made of a resource using a request method not supported by that resource.";
        public static string TOKEN_INVALIDATE = "Invalidate token.";
        public static string TOKEN_CHANGED = "Token has expired.";
        public static string UPDATE_CONTACT_INFO = "Please update contact information.";

        // - - - - - WhiteCost - - - - - 
        public static string PLEASE_TRY_AGAIN_SIGN_UP = "Please try again or click on the Sign Up link below to create a new account with us.";
        public static string SEND_ONE_TIME_PASSWORD = "One time password has been sent.";
        public static string MEMBER_ACCOUNT_NOT_ACTIVATE = "Please activate your account by entering the OTP sent to your email.";
        public static string ACCOUNT_NOT_ACTIVATE = "Account has not been activated.";
        public static string ACCOUNT_ALREADY_ACTIVATE = "Account has been activated.";
        public static string ACCOUNT_CORPORATE_NOT_ACTIVATE = "Account corporate is not activated.";
        //public static string RESEND_OTP_SUCCESS = "Your OTP has been sent via SMS to your mobile.";
        public static string RESEND_OTP_SUCCESS = "Your OTP has been sent to your email.";
        public static string OTP_INVALID = "You have entered an invalid OTP. Please try again.";

        public static string INVALID_DEVICE_ID_IOS = "Push notification token invalid.";
        public static string SENT_ACTIVATION_CODE = "An activation code has been sent to your email.";
        //public static string MOBILE_ALREADY_EXISTS = "The phone number you have entered already exists.";
        public static string MOBILE_ALREADY_EXISTS = "Mobile number already exists. Please use different contact details.";
        public static string MOBILE_NOT_FOUND = "Mobile number could not be found.";
        public static string DOCTOR_NOT_FOUND = "The doctor could not be found.";
        public static string CHILD_NOT_FOUND = "The child could not be found.";
        public static string SYMPTOM_NOT_FOUND = "The symptom could not be found.";
        public static string ALLERGY_NOT_FOUND = "The allergy could not be found.";
        public static string MEDICATION_NOT_FOUND = "The medication could not be found.";
        public static string MEDICATION_REACTION_NOT_FOUND = "The medication reaction could not be found.";
        public static string MEDICATION_DUPLICATED = "Medications that cannot be duplicated for prescription.";
        public static string MEDICATION_USAGE_PERIOD_NOT_FOUND = "The medication usage period could not be found.";
        public static string MEDICATION_ALERGIC = "{0} has a allergic reaction to {1}. Please try another suitable medication.";

        public static string REFILL_EXPIRY_DATE_REQUIRED = "Refill expiry date required.";
        public static string REFILL_EXPIRY_DATE_INVALID = "Refill expiry date must equal or greater than today.";
        public static string REFILL_MEDICATION_ALREADY_ACTIVATE = "Refill medication has been activated.";
        public static string REFILL_MEDICATION_NOT_ACTIVATE = "Refill medication has not been activated.";

        public static string CHILD_NOT_ACCESS = "Unauthorised request.";//"Don't have permission to access this child";
        //public static string NRIC_FIN_ALREADY_EXISTS = "The NRIC/FIN no. you have entered already exists.";
        public static string NRIC_FIN_ALREADY_EXISTS = "NRIC / FIN / already exists. Please try again.";
        //public static string PASSPORT_ALREADY_EXISTS = "The passport you have entered already exists.";
        public static string PASSPORT_ALREADY_EXISTS = "Passport No. already exists. Please try again.";
        public static string BIRTH_CERTIFICATE_ALREADY_EXISTS = "The birth certificate you have entered already exists.";
        public static string EMAIL_NOT_FOUND = "The email address is invalid. Please re-enter your email address.";//"The email address you entered could not be found.";
        //public static string EMAIL_ALREADY_EXISTS = "Email address already existed.";//"The email already exists";
        public static string EMAIL_ALREADY_EXISTS = "Email address already exists. Please use a different email address.";//"The email already exists";
        public static string UPDATE_STRIPE_CARD_TOKEN = "Stripe's customer ID does not exist, booking must be created with Stripe's card token.";
        public static string BOOKING_NOT_ACCESS = "Unauthorised request.";//"Don't have permission to access this booking";
        public static string RESERVATION_NOT_ACCESS = "Unauthorised request.";//"Don't have permission to access this booking";
        //public static string CHILD_NOT_ACCESS = "Unauthorised request";//"Don't have permission to access this booking";

        public static string NO_DELIVERY = "There is no delivery.";
        public static string NO_BOOKING = "There is no booking.";
        public static string NO_RESERVATION = "There is no reservation.";
        public static string NO_SPECIALIST = "There is no specialist.";
        public static string NO_FLAG_PATIENT = "There is no flag patient.";
        public static string NO_NOTE_PATIENT = "There is no note patient.";
        public static string BOOKING_NOT_FOUND = "The booking could not be found.";
        public static string RESERVATION_NOT_FOUND = "The reservation could not be found.";
        public static string GENDER_INDENTITY_FOUND = "The gender indentity could not be found.";

        public static string QR_CODE_NOT_FOUND = "The QR code could not be found.";
        public static string QR_CODE_INVALID = "QR code invalid.";
        public static string BOOKING_QR_INVALID = "Sorry, this delivery has been indicated as QR code invalid.";
        public static string BOOKING_IC_INVALID = "Sorry, this delivery has been indicated as IC invalid.";
        public static string BOOKING_PATIENT_NOT_HOME = "Sorry, this delivery has been indicated as patient not home.";
        public static string BOOKING_PATIENT_UNCONTACTABLE = "Sorry, this delivery has been indicated as patient uncontactable.";
        public static string BOOKING_PATIENT_CONTACTED = "Sorry, this delivery has been indicated as patient contactable.";
        public static string BOOKING_PHARMACY_NOT_SCAN_QR_CODE = "Sorry, this delivery have not scanned by pharmacy yet.";
        public static string BOOKING_PHARMACY_SCANED_QR_CODE = "Sorry, this delivery has been pharmacy scanned QR code before.";
        public static string BOOKING_DRIVER_SCANED_QR_CODE = "Sorry, this delivery has been scanned QR code before.";
        public static string BOOKING_DRIVER_NOT_SCAN_QR_CODE = "Sorry, please scan QR code before complete.";
        public static string BOOKING_HAS_BEEN_STARTED = "Sorry, this delivery has been started before.";
        //public static string BOOKING_NOT_STARTED = "Sorry, this delivery has been started deliveries before";
        public static string BOOKING_HAS_BEEN_COLLECTED_MEDICINE = "Sorry, this delivery has been collected medicine before.";

        public static string BOOKING_HAS_NOT_COLLECT_MEDICINE = "Sorry, this delivery need to set as collected first.";

        public static string BOOKING_PHARMACY_NOT_COMPLETE_COLLECTION = "Sorry, this delivery have not completed by pharmacy yet.";


        public static string BOOKING_PROCESSING = "There is a booking still in processing.";
        public static string BOOKING_COMPLETED = "Sorry, this booking has been completed.";
        public static string BOOKING_CANCELED = "Sorry, this booking has been cancelled.";
        public static string BOOKING_CONSULTED = "Sorry, this booking has been consulted.";
        public static string PATIENT_COMPLETED_BOOKING = "Sorry, this booking already marked as completed before.";
        public static string BOOKING_PAID = "Sorry, this booking has been paid.";
        public static string BOOKING_END_CALLED = "Sorry, this call ended.";
        public static string BOOKING_NOT_START = "Sorry, this booking haven't started.";
        public static string DOCTOR_SKIPPED = "Doctor is still engaged with a patient. We will notify you once the doctor ready.";

        public static string BOOKING_NOT_END_CALL = "Sorry, please end call before complete booking.";
        public static string BOOKING_NOT_PAID = "Sorry, this booking non payment.";
        public static string BOOKING_NOT_AVAILABLE = "Sorry, this booking is no longer available.";
        public static string BOOKING_INVALID = "Booking invalid.";
        public static string RESERVATION_INVALID = "reservation invalid.";
        public static string CARD_INVALID = "Card invalid.";
        public static string CHILD_INVALID = "Child invalid.";
        public static string RECOVERY_CODE_INVALID = "Recovery code invalid.";
        public static string MEDICATION_HAS_BEEN_REFILL_BEFORE = "Medication(s) {0} has been refill before.";
        public static string QUANTITY_MEDICATION_IN_INVERTORY_IS_NOT_ENOUGH = "The quantity of {0} in the inventory is not enough for your prescription.";

        public static string DOCTOR_NOT_READY = "Sorry, this doctor is currently not ready. Please select another doctor.";
        public static string NO_DOCTOR_FIND = "There is no doctors available at the moment. We will find you another in {0} secs.";

        public static string DELIVERY_TIMESLOT_NOT_FOUND = "Delivery timeslot could not be found.";
        public static string DELIVERY_ADDRESS_NOT_FOUND = "Delivery address could not be found.";
        public static string MEDICATION_NOT_FOUND_IN_PRESCRIPTION = "Medication could not be found in prescription.";
        public static string DELIVERY_ADDRESS_AND_DELIVERY_TIMESPLOT_REQUIRED = "Delivery address and delivery timeslot id are required.";
        public static string PHARMACY_REQUIRED = "Pharmacy id is required.";
        public static string PHARMACY_INVALID = "Pharmacy invalid.";
        public static string AGE_LESS_THAN_21 = "Age should be greater than 0 and less than 18 years. Please enter a valid date of birth.";
        public static string COPORATE_NOT_FOUND = "Corporate could not be found.";
        public static string COPORATE_MEMBER_NOT_FOUND = "Corporate's member could not be found.";
        public static string COPORATE_MEMBER_INVALID = "Corporate's member invalid.";
        
        public static string SUBSCRIPTION_NOT_FOUND = "Subscription could not be found.";
        public static string SUBSCRIPTION_MEMBER_NOT_FOUND = "Subscription's member could not be found.";
        public static string SUBSCRIPTION_MEMBER_INVALID = "Subscription's member invalid.";
        public static string SUBSCRIPTION_EXCEEDED = "Subscription has been exceeded.";
        public static string SUBSCRIPTION_EXPIRED = "Subscription has been expired.";
        public static string SUBSCRIPTION_ACCOUNT_NOT_ACTIVATE = "Account subscription is not activated.";
        public static string CARD_REQUIRED = "Please select card before book.";
        public static string RESERVATION_TIMESLOT_INVALID = "Reservation timeslot invalid."; 

        public static string BOOKING_ONLY_APPLY_FOR_DELIVER = "Can not update. Only apply for deliver booking.";
        public static string BOOKING_NOT_ASSIGN_TO_DRIVER = "This delivery haven't assign to driver.";
        public static string BOOKING_NOT_START_DELIVER = "Sorry, this delivery need to set as started first.";
        public static string IMAGE_NOT_FOUND = "The image could not be found.";
        public static string IMAGE_NOT_ACCESS = "Unauthorised request.";
        public static string UNAUTHORISED_REQUEST = "Unauthorised request.";

        public static string NOT_SCAN_QR_BEFORE_CONFIRM_IC = "Sorry, please scan QR code before confirm IC.";
        public static string BOOKING_CONFIRMED_IC = "Sorry, IC has been confirmed before.";
        public static string MEDICATION_HAS_BEEN_PHARMACY_COMPLETED_BEFORE = "Medication(s) {0} has been completed before.";
        public static string REQUEST_FORGOT_PASSWORD_SUCCESS = "We have received your request. Please check your email and follow the instructions to reset your password.";
        public static string CURRENT_PASSWORD_INCORRECT = "The current password you have entered is incorrect. Please try again.";
        public static string UPDATE_PASSWORD_SUCCESS = "Your password has been successfully updated.";

        public static string NO_NOTIFICATION = "There is no notification.";
        public static string CANNCEL_BOOKING_SUCCESS = "Your consultation has been cancelled.";
        public static string CANNCEL_RESERVATION_SUCCESS = "Your reservation has been cancelled.";

        public static string DURATION_UNIT_NOT_FOUND = "Duration unit could not be found.";
        public static string UOM_UNIT = "UOM unit could not be found.";
        public static string ADD_CARD_BEFORE_PAYMENT = "Please select card before payment.";

        public static string NO_DATA = "There is no data.";
        public static string TIMESLOTS_NOT_AVAILABLED = "Selected timeslot is not available";
        public static string FEEDBACK_SUCCESS = "Thank you for taking the time to provide us with your feedback. We strive towards improving our services and take your feedback seriously.";
        //public static string FEEDBACK_SUCCESS = "";

        public static string LANGUAGES_SPOKEN_INVALID = "The languages spoken invalid.";
        public static string DOCTOR_CATEGORY_INVALID = "The doctor categories spoken invalid.";
    }
}