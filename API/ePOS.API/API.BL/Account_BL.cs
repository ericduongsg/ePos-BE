using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using API.BO;
using Core_App;
using Core_App.BL;

namespace API.BL
{
    public enum AccountType
    {
        Developer = 99, // --> using for Test api
        Member = 1,
        Doctor = 2,
        Driver = 5,
        Pharmacy = 6,
        Web = 7
    }

    public static class DeviceType
    {
        public static int None = 0;
        public static int Ios = 1;
        public static int Android = 2;
    }

    public static class DeviceType_Text
    {
        public static string None = "";
        public static string Ios = "ios";
        public static string Android = "android";
    }

    public static class LENGTH_PUSH_NOFICATION_TOKEN
    {
        public static int Ios = 250;
        public static int Android = 250;
    }

    public class Account_BL
    {
        private DAL dal = new DAL();
        private Utilities until = new Utilities();

        public static int convertToNumber(string device_type)
        {
            device_type = device_type.ToLower().Trim();
            if (device_type == DeviceType_Text.Ios)
            {
                return DeviceType.Ios;
            }
            else if (device_type == DeviceType_Text.Android)
            {
                return DeviceType.Android;
            }
            else
            {
                return DeviceType.None;
            }
        }

        public static string convertToText(int device_type)
        {
            if (device_type == DeviceType.Ios)
            {
                return DeviceType_Text.Ios;
            }
            else if (device_type == DeviceType.Android)
            {
                return DeviceType_Text.Android;
            }
            else
            {
                return DeviceType_Text.None;
            }
        }

        public string createToken()
        {
            return Guid.NewGuid().ToString("D");
        }

    
    }
}