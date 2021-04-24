using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.Accounts
{
    public class AccountsCommonStuff
    {

        public static string GetRandomString(int length)
        {
            Random ran = new Random();

            String b = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_";
            
            String random = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(73);
                random = random + b.ElementAt(a);
            }

            return random;
        }
        
    }

    
}