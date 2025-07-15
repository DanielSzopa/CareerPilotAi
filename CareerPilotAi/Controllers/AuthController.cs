using CareerPilotAi.Infrastructure.Email;
using CareerPilotAi.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilotAi.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            EmailService emailService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                var userAlreadyConfirmedEmail = await _userManager.IsEmailConfirmedAsync(existingUser);
                if (userAlreadyConfirmedEmail)
                {
                    _logger.LogInformation("User tried to register with email {Email}, but account already exists and is confirmed.", model.Email);
                    ModelState.AddModelError(string.Empty, "An account with this email already exists. Please log in or reset your password.");
                    return View(model);
                }
                else
                {
                    _logger.LogInformation("User tried to register with email {Email}, but account already exists and is not confirmed.", model.Email);
                    return RedirectToAction(nameof(RegisterConfirmation), new { isAlreadyRegisteredButNotConfirmed = true });
                }
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with email: {Email}", model.Email);

                var confirmationLink = await GetRegistrationConfirmationLink(user);

                try
                {
                    await _emailService.SendRegistrationVerificationEmailAsync(user.Email!, confirmationLink!, cancellationToken);
                    _logger.LogInformation("Email confirmation link sent to user: {Email}", model.Email);
                    return RedirectToAction(nameof(RegisterConfirmation));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send confirmation email to user: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Sign up failed, something happened on our end. Please contact support.");
                }
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError("Error creating user {Email}: {Error}", model.Email, error.Description);
                ModelState.AddModelError(string.Empty, "Sign up failed, something happened on our end. Please contact support.");
            }

            return View(model);
        }

        [HttpGet("register-confirmation")]
        public IActionResult RegisterConfirmation(bool isAlreadyRegisteredButNotConfirmed = false)
        {
            string message = isAlreadyRegisteredButNotConfirmed
            ? "An account with this email already exists but is not confirmed. Please check your email for the confirmation link or resend it."
            : string.Empty;

            ViewData["Message"] = message;
            return View();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Email confirmation attempted with missing userId or token");
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Email confirmation attempted for non-existent user: {UserId}", userId);
                return RedirectToAction("Error", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email confirmed for user: {Email}, {UserId}", user.Email, user.Id);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
            _logger.LogError("Email confirmation failed for user: {Email}, {UserId}, Errors: {Errors}", user.Email, user.Id, errors);
            return RedirectToAction("Error", "Home");
        }

        [HttpGet("resend-confirmation")]
        public IActionResult ResendConfirmation()
        {
            return View();
        }

        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation(ResendConfirmationViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("Resend confirmation attempted for non-existent user: {Email}", model.Email);
                return RedirectToAction(nameof(RegisterConfirmation));
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogInformation("Resend confirmation attempted for already confirmed user: {Email}, {UserId}", model.Email, user.Id);
                return RedirectToAction(nameof(Login), new { isAlreadyRegisteredAndConfirmed = true });
            }

            var confirmationLink = await GetRegistrationConfirmationLink(user);

            try
            {
                await _emailService.SendRegistrationVerificationEmailAsync(user.Email!, confirmationLink!, cancellationToken);
                _logger.LogInformation("Confirmation email resent to user: {Email}, {UserId}", model.Email, user.Id);
                return RedirectToAction(nameof(RegisterConfirmation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to resend confirmation email to user: {Email}, {UserId}", model.Email, user.Id);
                ModelState.AddModelError(string.Empty, "Failed to send confirmation email. Please try again later or contact support.");
                return View(model);
            }
        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null, bool isAlreadyRegisteredAndConfirmed = false)
        {
            var message = isAlreadyRegisteredAndConfirmed
                ? "You already have registered and confirmed account. Please log in or reset your password."
                : string.Empty;

            return View(new LoginViewModel { ReturnUrl = returnUrl, Message = message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in: {Email}", model.Email);
                    return RedirectToLocal(model?.ReturnUrl);
                }

                if (result.IsNotAllowed)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                    {
                        _logger.LogWarning("Login attempt with unconfirmed email: {Email}, {UserId}", model.Email, user.Id);
                        return RedirectToAction(nameof(RegisterConfirmation), new { isAlreadyRegisteredButNotConfirmed = true });
                    }
                }

                _logger.LogError("Invalid login attempt for user {Email}.", model.Email);
                ModelState.AddModelError(string.Empty, "Login failed. Please check your email and password.");
            }

            return View(model);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For this example, we'll just redirect to reset password directly
                // In a real application, you would send an email with a reset link
                return RedirectToAction(nameof(ResetPassword), new { email = model.Email });
            }

            return View(model);
        }

        [HttpGet("forgot-password-confirmation")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string? email = null)
        {
            return View(new ResetPasswordViewModel { Email = email ?? string.Empty });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            // In a real application, you would use a proper reset token
            // For this example, we'll just reset the password directly
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [HttpGet("reset-password-confirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        private async Task<string> GetRegistrationConfirmationLink(IdentityUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth",
                    new { userId = user.Id, token = token }, Request.Scheme);

            return confirmationLink ?? throw new InvalidOperationException("Failed to generate confirmation link.");
        }
    }
}