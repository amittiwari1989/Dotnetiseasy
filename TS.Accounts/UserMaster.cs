using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using TS.DBConnection;

namespace TS.Accounts
{
    public enum UserType : byte
    {
        Local = 0,
        ActiveDirectory = 1,
        LocalDemo = 2,
        Facebook =3
    };

    public enum UserPasswordReset : byte
    {
        None = 0,
        Email = 1,
        Mobile = 2,
        EmailMobile = 3
    };

    public enum UserStatus : byte
    {
        Disabled = 0,
        Active = 1,
        LoggedIn = 2,
        Suspended = 3
    };

    public class UserMaster
    {
        #region fields


        public int uid { get; set; }
        public int cid { get; set; }
        public UserType User_type { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public string domain { get; set; }
        public string domain_username { get; set; }
        public string hashed_password { get; set; }
        public string salt { get; set; }
        public string mobile_number { get; set; }
        public string mobile_OTP { get; set; }
        public UserPasswordReset password_reset { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastPasswordChanged { get; set; }
        public DateTime? LastLogout { get; set; }
        public DateTime? LastLoginFail { get; set; }
        public int LoginFailCount { get; set; }
        public UserStatus user_status { get; set; }
        public string session_token { get; set; }
        public DateTime? session_token_expiry { get; set; }
        public string reset_token { get; set; }
        public DateTime? reset_token_expiry { get; set; }
        public DateTime created_on { get; set; }
        public int? created_by_uid { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public int emailvalidation { get; set; }
        public string emailvalidationToken { get; set; }
        public string FacebookId { get; set; }
        #endregion

        public UserMaster() { }

        public UserMaster(DataRow DR)
        {
            //UserMaster user = new UserMaster();
            this.uid = Convert.ToInt32(DR["uid"]);
            this.cid = Convert.ToInt32(DR["cid"]);
            this.User_type = (UserType)Convert.ToInt32(DR["User_type"]);
            this.display_name = Convert.ToString(DR["display_name"]);
            this.email = Convert.ToString(DR["email"]);
            this.domain = Convert.ToString(DR["domain"]);
            this.domain_username = Convert.ToString(DR["domain_username"]);
            this.hashed_password = Convert.ToString(DR["hashed_password"]);
            this.salt = Convert.ToString(DR["salt"]);
            this.mobile_number = Convert.ToString(DR["mobile_number"]);
            this.mobile_OTP = Convert.ToString(DR["mobile_OTP"]);
            this.password_reset = (UserPasswordReset)Convert.ToInt32(DR["password_reset"]);
            if (!string.IsNullOrEmpty(DR["LastLogin"].ToString()))
                this.LastLogin = Convert.ToDateTime(DR["LastLogin"]);
            if (!string.IsNullOrEmpty(DR["LastPasswordChanged"].ToString()))
                this.LastPasswordChanged = Convert.ToDateTime(DR["LastPasswordChanged"]);
            if (!string.IsNullOrEmpty(DR["LastLogout"].ToString()))
                this.LastLogout = Convert.ToDateTime(DR["LastLogout"]);
            if (!string.IsNullOrEmpty(DR["LastLoginFail"].ToString()))
                this.LastLoginFail = Convert.ToDateTime(DR["LastLoginFail"]);
            this.LoginFailCount = Convert.ToInt32(DR["LoginFailCount"]);
            this.user_status = (UserStatus)Convert.ToInt32(DR["user_status"]);
            this.session_token = Convert.ToString(DR["session_token"]);
            if (!string.IsNullOrEmpty(DR["session_token_expiry"].ToString()))
                this.session_token_expiry = Convert.ToDateTime(DR["session_token_expiry"]);
            this.reset_token = Convert.ToString(DR["reset_token"]);
            if (!string.IsNullOrEmpty(DR["reset_token_expiry"].ToString()))
                this.reset_token_expiry = Convert.ToDateTime(DR["reset_token_expiry"]);
            this.created_on = Convert.ToDateTime(DR["created_on"]);
            this.created_by_uid = Convert.ToInt32(DR["created_by_uid"]);
            this.date_created = Convert.ToDateTime(DR["date_created"]);
            this.date_modified = Convert.ToDateTime(DR["date_modified"]);
            this.emailvalidation = Convert.ToInt32(DR["emailvalidation"]);
            this.emailvalidationToken = Convert.ToString(DR["emailvalidationToken"]);
            this.FacebookId = Convert.ToString(DR["FacebookId"]);
        }

        public static UserMaster Login(string UserName, string Passwort, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            cmd.CommandText = "SELECT * FROM UserMaster where email=@email";
            cmd.Parameters.Add("@email", DBType.VarChar, UserName);
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            if (DT.Rows.Count == 0)
            {
                return null;
            }

            UserMaster user = new UserMaster(DT.Rows[0]);

            string encp = EncryptString(Passwort);
            if (encp != user.hashed_password)
                return null;

            return user;
        }

        public static UserMaster GetUserByEmail(string Email, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            cmd.CommandText = "SELECT * FROM UserMaster where email=@email";
            cmd.Parameters.Add("@email", DBType.VarChar, Email);
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            if (DT.Rows.Count == 0)
            {
                return null;
            }
            UserMaster user = new UserMaster(DT.Rows[0]);
            return user;
        }

        public static UserMaster GetUserByFacebookId(string FacebookId, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            cmd.CommandText = "SELECT * FROM UserMaster where FacebookId=@FacebookId";
            cmd.Parameters.Add("@FacebookId", DBType.VarChar, FacebookId);
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            if (DT.Rows.Count == 0)
            {
                return null;
            }
            UserMaster user = new UserMaster(DT.Rows[0]);
            return user;
        }


        public static UserMaster GetUserById(int uid, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            cmd.CommandText = "SELECT * FROM UserMaster where uid=@uid";
            cmd.Parameters.Add("@uid", DBType.Int, uid);
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            if (DT.Rows.Count == 0)
            {
                return null;
            }
            UserMaster user = new UserMaster(DT.Rows[0]);
            return user;
        }


        public static int CreateUser(UserMaster user, Connection con = null)
        {

            //Connection con = new Connection();
            if (con == null)
                con = new Connection();

            Command cmd = new Command();
            cmd.CommandText = @"INSERT INTO UserMaster(cid, display_name, email, hashed_password, mobile_number,salt, password_reset, LoginFailCount, user_status, created_on, date_created, date_modified,created_by_uid,emailvalidation,emailvalidationToken,FacebookId) 
                                    VALUES(@cid, @display_name, @email, @hashed_password,@mobile_number, 'N/A', 1, 0, 1, @TimeStamp, @TimeStamp, @TimeStamp,@created_by_uid,@emailvalidation,@emailvalidationToken,@FacebookId);" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");
            cmd.Parameters.Add("@cid", DBType.Int, user.cid);
            cmd.Parameters.Add("@email", DBType.VarChar, user.email);
            cmd.Parameters.Add("@display_name", DBType.VarChar, user.display_name);
            cmd.Parameters.Add("@hashed_password", DBType.VarChar, user.hashed_password);
            cmd.Parameters.Add("@mobile_number", DBType.VarChar, user.mobile_number);
            cmd.Parameters.Add("@created_by_uid", DBType.VarChar, user.created_by_uid);
            cmd.Parameters.Add("@TimeStamp", DBType.DateTime, DateTime.Now);
            cmd.Parameters.Add("@emailvalidation", DBType.Int, 0);
            cmd.Parameters.Add("@emailvalidationToken", DBType.VarChar, AccountsCommonStuff.GetRandomString(25));
            cmd.Parameters.Add("@FacebookId", DBType.VarChar, user.FacebookId);

            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            if (DT.Rows.Count > 0)
            {
                //SendEmail(user.email);
                return Convert.ToInt32(DT.Rows[0][0]);
            }
            else
                return 0;
        }

        public static string EncryptString(string ClearText)
        {

            byte[] clearTextBytes = Encoding.UTF8.GetBytes(ClearText);

            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();

            MemoryStream ms = new MemoryStream();
            byte[] rgbIV = Encoding.ASCII.GetBytes("ryojvlzmdalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("hcxilkqbbhczfeultgbskdmaunivmfuo");
            CryptoStream cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV),
             CryptoStreamMode.Write);

            cs.Write(clearTextBytes, 0, clearTextBytes.Length);

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public string DecryptString(string EncryptedText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(EncryptedText);

            MemoryStream ms = new MemoryStream();

            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();


            byte[] rgbIV = Encoding.ASCII.GetBytes("ryojvlzmdalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("hcxilkqbbhczfeultgbskdmaunivmfuo");

            CryptoStream cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIV),
            CryptoStreamMode.Write);

            cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());

        }




        public static UserMaster VerifyEmail(string emailId, string rnd, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            cmd.CommandText = "SELECT * FROM UserMaster where email=@email";
            cmd.Parameters.Add("@email", DBType.VarChar, emailId);
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            if (DT.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                UserMaster user = new UserMaster(DT.Rows[0]);
                if (user.emailvalidationToken == rnd)
                {
                    cmd.CommandText = "Update UserMaster SET emailvalidation=1 WHERE email=@email";
                    con.cmd = cmd;
                    con.ExecuteNonQuery();
                    return user;

                }
                else
                    return null;
            }


        }

        public static string GeneratePasswordToken(string emailId, string rnd, Connection con)
        {
            if (con == null)
                con = new Connection();
            
            Command cmd = new Command();
            
            cmd.CommandText = "Update UserMaster SET reset_token=@reset_token,reset_token_expiry=@reset_token_expiry WHERE email=@email";
            cmd.Parameters.Add("@reset_token", DBType.VarChar, rnd);
            cmd.Parameters.Add("@reset_token_expiry", DBType.DateTime, DateTime.Now.AddDays(1));
            cmd.Parameters.Add("@email", DBType.VarChar, emailId);
            con.cmd = cmd;
            con.ExecuteNonQuery();
            return rnd;

        }

        public static int UpdatePassword(string emailId, string password, Connection con)
        {
            if (con == null)
                con = new Connection();

            Command cmd = new Command();

            cmd.CommandText = "Update UserMaster SET hashed_password=@hashed_password,LastPasswordChanged=@LastPasswordChanged,LoginFailCount=0,reset_token_expiry=@reset_token_expiry WHERE email=@email";
            cmd.Parameters.Add("@hashed_password", DBType.VarChar, EncryptString(password));
            cmd.Parameters.Add("@LastPasswordChanged", DBType.DateTime, DateTime.Now);
            cmd.Parameters.Add("@reset_token_expiry", DBType.DateTime, DateTime.Now);
            cmd.Parameters.Add("@email", DBType.VarChar, emailId);
            con.cmd = cmd;
            con.ExecuteNonQuery();
            return 1;

        }

    }



}
