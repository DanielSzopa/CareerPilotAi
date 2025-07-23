using CareerPilotAi.Infrastructure.Email;
using CareerPilotAi.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                _logger.LogError("Error creating user {Email}: {ErrorCode}, {Error}", model.Email, error.Code, error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
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

            return View(new LoginViewModel 
            { 
                Email = string.Empty,
                Password = string.Empty,
                ReturnUrl = returnUrl, 
                Message = message 
            });
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
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "JobApplication");
                    }
                }

                if (result.IsNotAllowed)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        _logger.LogWarning("Login attempt with non-existent user: {Email}", model.Email);
                        ModelState.AddModelError(string.Empty, "Login failed. Please check your email and password.");
                        return View(model);    
                    }

                    if (!await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        _logger.LogWarning("Login attempt with unconfirmed email and invalid password: {Email}, {UserId}", model.Email, user.Id);
                        ModelState.AddModelError(string.Empty, "Login failed. Please check your email and password.");
                        return View(model);
                    }
                    
                    if (!await _userManager.IsEmailConfirmedAsync(user))
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
            return RedirectToAction(nameof(LogoutConfirmation));
        }

        [HttpGet("logout")]
        public IActionResult LogoutConfirmation()
        {
            return View();
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent user: {Email}", model.Email);
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // Always redirect to confirmation page to prevent email enumeration attacks
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action(nameof(ResetPassword), "Auth",
                    new { userId = user.Id, token = resetToken }, Request.Scheme);

                try
                {
                    await _emailService.SendPasswordResetEmailAsync(model.Email, resetLink!, cancellationToken);
                    _logger.LogInformation("Password reset email sent to user: {Email}, {UserId}", model.Email, user.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send password reset email to user: {Email}, {UserId}", model.Email, user.Id);
                }
            }
            else
            {
                _logger.LogWarning("Password reset attempted for unconfirmed user: {Email}", model.Email);
            }

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet("forgot-password-confirmation")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string? userId, string? token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Password reset attempted with missing userId or token");
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent user: {UserId}", userId);
                return RedirectToAction("Error", "Home");
            }

            return View(new ResetPasswordViewModel { UserId = user.Id, Code = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent user: {UserId}", model.UserId);
                ModelState.AddModelError(string.Empty, "Invalid password reset request.");
                return View(model);
            }

            var tokenVerification = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, UserManager<IdentityUser>.ResetPasswordTokenPurpose, model.Code);
            if(!tokenVerification)
            {
                _logger.LogWarning("Password reset token verification failed for user: {Email}, {UserId}, {RequestUserId}", user.Email, user.Id, model.UserId);
                ModelState.AddModelError(string.Empty, "Password reset link may have expired or is invalid. Please try again or request a new one.");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successful for user: {Email}, {UserId}", user.Email, user.Id);
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            ModelState.AddModelError(string.Empty, "Password reset link may have expired or is invalid. Please try again or request a new one.");
            _logger.LogError("Password reset failed for user {Email}: {Errors}", user.Email, string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}")));

            return View(model);
        }

        [HttpGet("reset-password-confirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
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