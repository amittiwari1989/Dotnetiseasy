using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;

namespace TS.CodeShare.Helpers
{
    public class CommonStuff
    {

        public static string GetRandomString(int length,bool includeAlphabet =true, bool includeNumbers = true,bool includespecialChar = false,bool capsOnly=false)
        {
            Random ran = new Random();

            String b = "";
            if (includeAlphabet)
                b += "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (includeNumbers)
                b += "1234567890";
            if(includespecialChar)
                b+="!@#$%^&*()_";

            String random = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(b.Length);
                random = random + b.ElementAt(a);
            }

            return (capsOnly?random.ToUpper(): random);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("ipaddress not found");
        }

        public static void SendEmail(string emailId, string Subject, string Body)
        {
            MailMessage msgs = new MailMessage();
            msgs.To.Add(emailId);
            MailAddress address = new MailAddress("info@dotnetiseasy.com");
            msgs.From = address;
            msgs.Subject = Subject;// "Welcome to DotNetIsEasy.com";
            string htmlBody = Body;

            msgs.Body = htmlBody;
            msgs.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("info@dotnetiseasy.com", "pass@word1");
            //Send the msgs  
            client.Send(msgs);

        }

        public static string getEmailVerificationBody(string emailId, string random)
        {
            return @"<p><strong>Welcome to DotNetEasy.com</strong></p>
<p><strong>Good to see you here. </strong></p>
<p><strong>Please click on below link to verify your email or copy paste in browser</strong></p>
<p><strong><a title=""Verify Email"" href=""http://www.dotnetiseasy.com/Account/VerifyEmail/?emailId=" + emailId + "&rnd=" + random + @""" target=""_blank"">http://www.dotnetiseasy.com/Account/VerifyEmail/?emailId=" + emailId + "&rnd=" + random + "</a></strong></p>";
        }

        public static string getEmailPasswordReset(string emailId, string random)
        {
            return @"<p><strong>Welcome to DotNetEasy.com</strong></p>
<p><strong>Please click on below link to reset your password or copy paste in browser</strong></p>
<p><strong>This link will be expired in 24 Hrs</strong></p>
<p><strong><a title=""Password Reset"" href=""http://www.dotnetiseasy.com/Account/ResetPassword/?emailId=" + emailId + "&Token=" + random + @""" target=""_blank"">http://www.dotnetiseasy.com/Account/ResetPassword/?emailId=" + emailId + "&Token=" + random + "</a></strong></p>";
        }


        public static string getEmailChangePassword(string emailId, string random)
        {
            return @"<p><strong>Your password has been changed</strong></p>
<p><strong>Your Password has been changes successfully</strong></p>
<p><strong>If this was not you please </strong></p>
<p><strong><a title=""Password Reset"" href=""http://www.dotnetiseasy.com/Account/ResetPassword/?emailId=" + emailId + "&Token=" + random + @""" target=""_blank"">http://www.dotnetiseasy.com/Account/ResetPassword/?emailId=" + emailId + "&Token=" + random + "</a></strong></p>";
        }
    }


}