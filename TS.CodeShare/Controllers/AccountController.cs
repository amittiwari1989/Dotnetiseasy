using Facebook;
using System;
using System.Configuration;
using System.Web.Mvc;
using TS.Accounts;
using TS.CodeShare.App_Start;
using TS.CodeShare.Helpers;
using TS.CodeShare.Models;
using TS.DBConnection;

namespace TS.WebApp.Controllers
{
    public class AccountController : Controller
    {
        Connection con = new Connection(ConnectionStrings.getConnectionStrings(), ConnectionStrings.getServerType());
        string _app_id = ConfigurationManager.AppSettings["FclientId"].ToString(); // "870322430140856";
        string _client_sec = ConfigurationManager.AppSettings["client_sec"].ToString(); //"6d86307bd9c7cd76b738b9941c827cc8";
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Logger.log.Info("Login: emailId=" + model.Email + ",IP=" + CommonStuff.GetLocalIPAddress());
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            UserMaster user = UserMaster.Login(model.Email, model.Password, con);

            string strp = UserMaster.EncryptString(model.Password);
            //UserLoginInfo user1 = new UserMaster();

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            if (user.emailvalidation == 0)
            {
                ModelState.AddModelError("ValidateEmail", "Please Validate your Email First");
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            switch (user.user_status)
            {
                case UserStatus.Disabled:
                    return RedirectToLocal(returnUrl + "&Disabled");
                case UserStatus.Suspended:
                    return RedirectToLocal(returnUrl + "&Suspended");
                case UserStatus.LoggedIn:
                    return RedirectToLocal(returnUrl + "&LoggedIn");
                //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case UserStatus.Active:
                    {
                        if (strp == user.hashed_password)
                        {
                            Session["uid"] = user.uid;
                            Session["DisplayName"] = user.display_name;
                            Session["cid"] = user.cid;
                            if (string.IsNullOrEmpty(returnUrl))
                                return RedirectToAction("PostArtical", "Artical");
                            else
                                return Redirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                        }
                    }
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }

            return View(model);
        }

        public ActionResult AfterLogin()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult CreateUser(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(CreateUserViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "password and Confirm password doesnot match");
                return View(model);
            }

            UserMaster user = UserMaster.GetUserByEmail(model.EmailAddress, con);

            if (user != null)
            {
                ModelState.AddModelError("", "Email already Registered");
                return View(model);
            }

            string strp = UserMaster.EncryptString(model.Password);

            UserMaster new_user = new UserMaster();
            new_user.cid = Convert.ToInt32(Session["cid"]);
            new_user.email = model.EmailAddress;
            new_user.display_name = model.DisplayName;
            new_user.hashed_password = strp;
            new_user.mobile_number = model.Mobile;
            new_user.created_by_uid = Convert.ToInt32(Session["uid"]);

            int i = UserMaster.CreateUser(new_user, con);

            if (i > 0)
            {
                user = UserMaster.GetUserByEmail(model.EmailAddress, con);
                CommonStuff.SendEmail(user.email, "Welcome to DotNetIsEasy.com", CommonStuff.getEmailVerificationBody(user.email, user.emailvalidationToken));

                //user = UserMaster.Login(model.EmailAddress, model.Password, con);

                //Session["uid"] = user.uid;
                //Session["DisplayName"] = user.display_name;
                //Session["cid"] = user.cid;
                return RedirectToAction("Success", "Account");
            }
            else
            {
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout(string returnUrl)
        {
            Session["DisplayName"] = null;
            Session["uid"] = null;
            Session.RemoveAll();

            ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction("Login", "Account");
            //return View();
        }

        [AllowAnonymous]
        public ActionResult VerifyEmail(string emailId, string rnd)
        {


            UserMaster.VerifyEmail(emailId, rnd, con);
            return RedirectToAction("Login", "Account");
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResendEmail()
        {

            return View();
            //return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResendEmail(LoginViewModel model)
        {
            UserMaster user = UserMaster.GetUserByEmail(model.Email, con);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            CommonStuff.SendEmail(model.Email, "Welcome to DotNetIsEasy.com", CommonStuff.getEmailVerificationBody(user.email, user.emailvalidationToken));
            return RedirectToAction("Login", "Account");
            //return View();
        }


        [AllowAnonymous]
        public ActionResult Success(string msg)
        {
            AlertModel model = new AlertModel();
            model.AlertType = 1;
            model.Title = "Success";
            model.Message = (string.IsNullOrEmpty(msg) ? "You have successfully registered with us, Please check your email and verify your email id. Please check your spam folder if not deliver in inbox" : msg);
            return View(model);
            //return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(LoginViewModel model)
        {
            UserMaster user = UserMaster.GetUserByEmail(model.Email, con);
            if (user == null)
            {
                ModelState.AddModelError("", "Email is not registered with us.");
                return View(model);
            }

            string rnd = CommonStuff.GetRandomString(30);

            UserMaster.GeneratePasswordToken(model.Email, rnd, con);
            CommonStuff.SendEmail(model.Email, "Reset your password", CommonStuff.getEmailPasswordReset(user.email, rnd));
            return RedirectToAction("Success", "Account", new { msg = "A password reset link has been send on your email id.  Please check your spam folder if not deliver in inbox" });
            //return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string emailId, string Token)
        {
            ViewBag.emailId = emailId;
            ViewBag.Token = Token;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string emailId, string Token, CreateUserViewModel model)
        {

           Logger.log.Info("ResetPassword: emailId=" + emailId + ",Token=" + Token);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "password and Confirm password doesnot match");
                return View(model);
            }

            UserMaster user = UserMaster.GetUserByEmail(emailId, con);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Reset Link");
                return View(model);
            }

            if (user.reset_token != Token)
            {
                ModelState.AddModelError("", "Invalid Reset Link");
                return View(model);
            }
            if (user.reset_token_expiry <=DateTime.Now)
            {
                ModelState.AddModelError("", "password reset link has expired!");
                return View(model);
            }

            UserMaster.UpdatePassword(emailId, model.Password, con);

            return RedirectToAction("Login", "Account");


        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "password and Confirm password doesnot match");
                return View(model);
            }

            UserMaster user = UserMaster.GetUserById(Convert.ToInt32(Session["uid"]), con);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid User or Session");
                return View(model);
            }

            if (user.hashed_password != UserMaster.EncryptString(model.Password))
            {
                ModelState.AddModelError("", "Invalid Old Password");
                return View(model);
            }
            UserMaster.UpdatePassword(user.email, model.Password, con);
            //Write Code to send password change email
            return RedirectToAction("Success", "Account", new { msg = "You have changed you password Successfully." });
        }


        [AllowAnonymous]
        public ActionResult FacebookLogin()
        {

            //return View();
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = _app_id,
                client_secret = _client_sec,
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
            //return View();
        }




        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }


        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = _app_id,
                client_secret = _client_sec,
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;

            // Store the access token in the session for farther use
            Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;

            // Get the user's information, like email, first name, middle name etc
            dynamic me = fb.Get("me?fields=email,first_name,middle_name,last_name,id,name,name_format,picture");
            string email = me.email;
            string firstname = me.first_name;
            string middlename = me.middle_name;
            string lastname = me.last_name;
            string name = me.name;
            string name_format = me.name_format;
            string fb_id = me.id;
            dynamic pic = me.picture;

            //string picture = me.picture;

            // Set the auth cookie
            //FormsAuthentication.SetAuthCookie(email, false);

            UserMaster user = new UserMaster();

            string pict = pic[0].url;
            user = UserMaster.GetUserByFacebookId(me.id, con);
            if (user == null)
            {
                user = new UserMaster();
                user.email = (string.IsNullOrEmpty(email) ? me.id : email);
                user.display_name = firstname + " " + lastname;
                user.FacebookId = me.id;
                user.created_by_uid = 1;
                user.hashed_password = "N/A";
                user.mobile_number = "N/A";

                user.cid = 1;

                int i = UserMaster.CreateUser(user, con);
                user.uid = i;
            }

            Session["DisplayName"] = user.display_name;

            Session["uid"] = user.uid;


            return RedirectToAction("Index", "Home");
        }

    }
}