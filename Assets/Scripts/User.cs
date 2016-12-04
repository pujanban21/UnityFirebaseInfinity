using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class User
    {
        private const string TAG = "UserInfo";

        private string mUserName;
        private string mEmail;
        private string mPassword;
        private string mUID;

        public User() { }

        public User(string username, string email, string password) {
            this.mEmail = email;
            this.mUserName = username;
            this.mPassword = password;
        }

        public Dictionary<string, object> UserInfo() {
            Dictionary<string, object> userInformation = new Dictionary<string, object>();
            userInformation[StringValues.USER_NAME] = mUserName;
            userInformation[StringValues.EMAIL] = mEmail;
            userInformation[StringValues.PASSWORD] = mPassword;
            return userInformation;
        }
    }
}
