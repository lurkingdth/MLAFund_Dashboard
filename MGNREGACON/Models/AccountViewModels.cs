using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MLAFund.Models

{ 

    // ka name change kar lijye kucha  apne according
     public class NICApplicationPrincipal : ClaimsPrincipal
{

    public NICApplicationPrincipal(NICApplicationIdentity identity)
        : base(identity)
    {

    }

    public NICApplicationPrincipal(ClaimsPrincipal claimsPrincipal)
        : base(claimsPrincipal)
    {

    }
}

public class NICApplicationIdentity : ClaimsIdentity
{
    public const string RolesClaimType = "http://www.nic.in/Role";
    public const string GroupClaimType = "http://www.nic.in/Group";
    public const string IPClaimType = "http://www.nic.in/IP";
    public const string IdClaimType = "http://www.nic.in/Id";


    public NICApplicationIdentity(IEnumerable<Claim> claims, string authenticationType)
        : base(claims, authenticationType: authenticationType)
    {

        AddClaims(from @group in claims where @group.Type == GroupClaimType select @group);
        AddClaims(from role in claims where role.Type == RoleClaimType select role);
        AddClaims(from id in claims where id.Type == IdClaimType select id);
        AddClaims(from ip in claims where ip.Type == IPClaimType select ip);
    }

    public NICApplicationIdentity(IEnumerable<string> roles, IEnumerable<string> groups, string IP, int id)
    {
        AddClaims(from @group in groups select new Claim(GroupClaimType, @group));
        AddClaims(from role in roles select new Claim(RolesClaimType, role));
        AddClaim(new Claim(IdClaimType, id.ToString()));
        AddClaim(new Claim(IPClaimType, IP.ToString()));
    }

    public IEnumerable<string> Roles
    {
        get
        {
            return from claim in FindAll(RolesClaimType) select claim.Value;
        }
    }

    public IEnumerable<string> Groups { get { return from claim in FindAll(GroupClaimType) select claim.Value; } }

    public int Id { get { return Convert.ToInt32(FindFirst(IdClaimType).Value); } }
    public string IP { get { return FindFirst(IPClaimType).Value; } }
}

public class LoggingHelper
{
    public static IAuthenticationManager AuthenticationManager
    {
        get
        {
            return HttpContext.Current.GetOwinContext().Authentication;
        }
    }
    public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
    {
        if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
        {
            response.Redirect(returnUrl);
        }
        else
        {
            response.Redirect("~/");
        }
    }
    public static bool IsLocalUrl(string url)
    {
        return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
    }
}
public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public static class UserHelper
    {
        public static string GetUserRole(this IPrincipal User)
        {
            return (User as ClaimsPrincipal).Claims.FirstOrDefault(j => j.Type == ClaimTypes.Role).Value;
        }
        public static string GetUserDistrict(this IPrincipal User)
        {
            return (User as ClaimsPrincipal).Claims.FirstOrDefault(j => j.Type == "district").Value;
        }


    }

}
