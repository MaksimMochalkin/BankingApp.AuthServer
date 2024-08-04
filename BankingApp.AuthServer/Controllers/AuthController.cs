namespace BankingApp.AuthServer.Controllers
{
    using BankingApp.AuthServer.ViewModels;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        [Route("registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "User registered successfully!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("admin-registration")]
        public IActionResult AdminRegistration()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "HasAdminRole")]
        [Route("admin-registration")]
        public async Task<IActionResult> AdminRegistration(AdminRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isRoleExist = await _roleManager.RoleExistsAsync(model.Role);

            if (!isRoleExist)
            {
                ModelState.AddModelError(model.Email, "Role does not exist.");
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);

                return View();

            }

            ModelState.AddModelError(model.Email, "Role does not exist.");
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError(model.Email, "User not found");
                return View(model);
            }

            var signinResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (signinResult.Succeeded)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var isAdmin = claims.Any(r => r.Value.Equals("Administrator"));
                if (isAdmin)
                {
                    return Redirect("https://localhost:7293");
                }

                if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                {
                    return BadRequest("Return url required parametr");
                }

                return Redirect(model.ReturnUrl);
            }

            ModelState.AddModelError(model.Email, "Sign in failed");

            return View();
        }

        [HttpGet]
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is not null)
            {
                return BadRequest(new { message = "User not found." });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { message = "Password changed successfully." });
            }

            return BadRequest(result.Errors);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutResult = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutResult.PostLogoutRedirectUri);
        }
    }
}
