using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FireForce.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected string CurrentUsername => User.Identity?.Name ?? "System";
        protected string CurrentUserRole => User.FindFirst(ClaimTypes.Role)?.Value ?? "";

        protected int CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(userIdClaim, out var userId) ? userId : 0;
            }
        }

        protected bool IsAdmin => CurrentUserRole == "Admin";

        protected bool IsCaptain => CurrentUserRole == "Captain";

        protected bool IsFirefighter => CurrentUserRole == "Firefighter";

        protected void ShowSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void ShowErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }

        protected void ShowInfoMessage(string message)
        {
            TempData["InfoMessage"] = message;
        }
    }
}