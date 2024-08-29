using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login
{
    public class LoginData
    {
        public string username;
        public string password;

        public LoginData()
        {
            this.username = "";
            this.password = "";
        }

        public LoginData(string user, string pw)
        {
            this.username = user;
            this.password = pw;
        }
    }

    public static bool TestLogin(LoginData user)
    {
        if (user.username == "admin")
            return true;
        else
            return false;
    }
}
