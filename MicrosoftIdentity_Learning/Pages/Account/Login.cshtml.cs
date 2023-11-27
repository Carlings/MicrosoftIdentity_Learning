using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MicrosoftIdentity_Learning.Pages.Shared.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (!ModelState.IsValid) return Page();

            if(Credential.UserName == "admin" && Credential.Password == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Role", "HRManager"),
                    new Claim("EmploymentDate", "2023-08-01")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authOptions = new AuthenticationProperties()
                {
                    IsPersistent = Credential.RememberMe,
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authOptions);

                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
    public class Credential
    {
        [Required (ErrorMessage = "Enter your User Name")]
        [Display (Name = "User Name")]
        public string UserName { get; set; } = string.Empty;

        [Required (ErrorMessage = "Enter your Password")]
        [Display (Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
