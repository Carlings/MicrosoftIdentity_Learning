using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace MicrosoftIdentity_Learning.Pages.Account.AuthorizationCustomRequirements
{
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {
        public HRManagerProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }

        public int ProbationMonths { get; }
    }

    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            if(!context.User.HasClaim(x => x.Type == "EmploymentDate"))
                return Task.CompletedTask;

            if(DateTime.TryParse(context.User.FindFirst(x => x.Type == "EmploymentDate")?.Value, out DateTime currentMonth))
            {
                var period = DateTime.Now - currentMonth;
                if(period.Days > 30 * requirement.ProbationMonths)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
