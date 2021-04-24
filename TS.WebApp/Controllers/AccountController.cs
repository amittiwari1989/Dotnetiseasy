using System;
using System.Web.Mvc;
using TS.Accounts;
using TS.DBConnection;
using TS.WebApp.App_Start;
using TS.WebApp.Models;

namespace TS.WebApp.Controllers
{
    public class AccountController : Controller
    {
        Connection con = new Connection(ConnectionStrings.getConnectionStrings(), ConnectionStrings.getServerType());

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

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            UserMaster user = UserMaster.Login(model.Email, model.Password,con);

            string strp = UserMaster.EncryptString(model.Password);
            //UserLoginInfo user1 = new UserMaster();

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
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
                            return RedirectToAction("Dashboard", "Home");
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

            UserMaster user = UserMaster.GetUserByEmail(model.EmailAddress,con);

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

            int i = UserMaster.CreateUser(new_user);

            if (i > 0)
            {
                return RedirectToAction("Dashboard","Home");
            }
            else
            {
                return View(model);
            }
        }



    }
}