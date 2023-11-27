using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicrosoftIdentity_Learning.Pages.HR
{
    [Authorize(Policy = "HROnly")]
    public class HumanResourcePageModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
