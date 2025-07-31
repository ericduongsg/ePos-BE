using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace API.BO
{

    public class AccountInfo
    {
        private string _ID;
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _REFRESH_TOKEN;
        public string REFRESH_TOKEN
        {
            get { return _REFRESH_TOKEN; }
            set { _REFRESH_TOKEN = value; }
        }

        private int _STATUS;
        public int STATUS
        {
            get { return _STATUS; }
            set { _STATUS = value; }
        }

        private bool _REQUEST_UPDATE_CONTACT_INFO;
        public bool REQUEST_UPDATE_CONTACT_INFO
        {
            get { return _REQUEST_UPDATE_CONTACT_INFO; }
            set { _REQUEST_UPDATE_CONTACT_INFO = value; }
        }

        private bool _REQUEST_ACTIVATE_ACCOUNT;
        public bool REQUEST_ACTIVATE_ACCOUNT
        {
            get { return _REQUEST_ACTIVATE_ACCOUNT; }
            set { _REQUEST_ACTIVATE_ACCOUNT = value; }
        }
        
        private int _FOR_CHILD_THAT_TURNED_21;
        public int FOR_CHILD_THAT_TURNED_21
        {
            get { return _FOR_CHILD_THAT_TURNED_21; }
            set { _FOR_CHILD_THAT_TURNED_21 = value; }
        }

        private string _TYPE;
        public string TYPE
        {
            get { return _TYPE; }
            set { _TYPE = value; }
        }
    }
}