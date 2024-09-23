using LibraryManagement.Entities;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace LibraryManagement.Controllers
{
    // Authentication controller for user sign-up, login, and logout functionality
    public class AuthController : Controller
    {
        // Static list to store users
        private static List<UserEntity> _users = new List<UserEntity>();

        private readonly IDataProtector _dataProdector;

        // Constructor that sets up the data protector for password encryption
        public AuthController(IDataProtectionProvider dataProdectionProvider)
        {
            _dataProdector = dataProdectionProvider.CreateProtector("security");
        }

        // GET action to display the SignUp view
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // POST action to handle user sign-up
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel formData)
        {
            // Validate form data
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            // Check if passwords match
            if (formData.Password != formData.PasswordConfirm)
            {
                ModelState.AddModelError(nameof(formData.PasswordConfirm), "Passwords do not match.");
                return View(formData);
            }

            // Check if the email is already registered
            var user = _users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());
            if (user is not null)
            {
                ModelState.AddModelError(nameof(formData.Email), "This email is already registered.");
                return View(formData);
            }

            // Create a new user entity and add it to the static user list
            var newUser = new UserEntity()
            {
                Id = 1,
                Email = formData.Email.ToLower(),
                Password = _dataProdector.Protect(formData.Password), // Encrypt the password
                FullName = formData.FullName,
                PhoneNumber = formData.PhoneNumber
            };

            _users.Add(newUser); // Add user to the list
            return RedirectToAction("Login"); // Redirect to login after successful registration
        }

        // GET action to display the Login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST action to handle user login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel formData)
        {
            // Check if the email exists in the user list
            var user = _users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());

            // If the user is not found, return an error message
            if (user is null)
            {
                ViewBag.Error = "Email address not registered. Please sign up.";
                return View(formData);
            }

            // Decrypt the stored password for comparison
            var rawPassword = _dataProdector.Unprotect(user.Password);

            // Check if the password matches
            if (rawPassword != formData.Password)
            {
                ViewBag.Error = "Invalid password. Please try again.";
                return View(formData);
            }

            // If credentials are correct, sign the user in using cookie authentication
            var claims = new List<Claim>
            {
                new Claim("email", user.Email),
                new Claim("id", user.Id.ToString())
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Set authentication properties, such as expiration time
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48))
            };

            // Sign in the user with the created claims
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), authProperties);
            return RedirectToAction("Index", "Home"); // Redirect to the home page after successful login
        }

        // POST action to handle user logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user from the authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth"); // Redirect to login page after logout
        }
    }
}
