using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GymManagementSystem.DAL.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using GymManagementSystem.BLL.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
namespace GymManagementSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser>userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }
        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName,model.Password,model.RememberMe,lockoutOnFailure:true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User{UserId}SignedIn,",user.Id);
                return RedirectToAction(nameof(HomeController.Index),"Home");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User{UserId}Is Loced Out", user.Id);
                ModelState.AddModelError(string.Empty, "This Account Is Temporaly Locked");
                return View(model);
            }
            else if (result.IsNotAllowed) 
            {
                ModelState.AddModelError(string.Empty,"Sign-In Is NOt Allowed For This Account");
            }
            else
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
            return View(model);


        }
        #endregion
        #region SignOut
        [HttpPost]
        [Authorize]
        public async Task<IActionResult>Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion
        #region AccessDenied
        public IActionResult AccessDenied() => View();
        #endregion
    }
}
