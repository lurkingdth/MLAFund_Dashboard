using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MLAFund.Models;
using MLAFund.Models.Data;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MLAFund.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
      
        public AccountController()
        {
        }

        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(tbl_Users data, string returnUrl)
        {
            try
            {
                var dbobj = new RuralWorksData();
            var check = dbobj.tbl_Users.Where(x => x.username == data.username && x.password == data.password).FirstOrDefault();
            if (check != null)
            {

                //Session["district"] = check.district;
                //Session["username"] = check.username;
                //Session["role"] = check.user_role;
                //Session["dptcode"] = check.dpt_code;
                SignIn(GetClaims(check));
                
                return RedirectToAction("index", "Home");
            }
            else
            {
                ViewBag.msg = "wrong user name and password";
                return View();
            }
            }
            catch(Exception e)
            {
                ViewBag.msg = "Check Internet connection Properly";
            }
            return View();
        }

        public ActionResult Logout()
        {
              LoggingHelper.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("login"); 
        }

        private void SignIn(List<Claim> claims)
        {
            var claimsIdentity = new NICApplicationIdentity(claims,
            DefaultAuthenticationTypes.ApplicationCookie);
            LoggingHelper.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            LoggingHelper.AuthenticationManager.SignIn
            (new AuthenticationProperties() { IsPersistent = false, ExpiresUtc = DateTime.UtcNow.AddMinutes(20), AllowRefresh = false }, claimsIdentity);
            HttpContext.User = new NICApplicationPrincipal
            (LoggingHelper.AuthenticationManager.AuthenticationResponseGrant.Principal);
        }

        private List<Claim> GetClaims(tbl_Users user)
        {

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.id.ToString()));
            claims.Add(new Claim(NICApplicationIdentity.IPClaimType, HttpContext.Request.UserHostAddress));
            claims.Add(new Claim(NICApplicationIdentity.IdClaimType, user.id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.username));
            claims.Add(new Claim(ClaimTypes.Role, user.user_role.ToString()));
            claims.Add(new Claim("district", user.district.ToString()));
            claims.Add(new Claim(ClaimTypes.Actor, JsonConvert.SerializeObject(user)));
           
            return claims;
        }

    }
}