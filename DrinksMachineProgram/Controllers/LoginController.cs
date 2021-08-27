using DrinksMachineProgram.Authentication;
using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Models;
using DrinksMachineProgram.Resources;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DrinksMachineProgram.Controllers
{

    public class LoginController : Controller
    {

        #region Attributes

        private readonly ILoginManager _loginManager;

        #endregion Attributes

        #region CTOR

        public LoginController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        #endregion CTOR

        #region Public Methods

        // Get: /Login/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await _loginManager.Logout();

            ViewData["ReturnUrl"] = returnUrl;

            LoginModel login = new LoginModel();

            return View(login);
        }

        // POST: /Login/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginModel login,
            string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider();
                byte[] hashCode = cryptoProvider.ComputeHash(Encoding.ASCII.GetBytes(login.Password));

                Entities.User user = UsersBL
                    .Instance
                    .Detail(login.UserName, hashCode);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, TextResources.MessageErrorLoginIncorrect);

                    return View(login);
                }

                await _loginManager.Login(user);

                if (string.IsNullOrEmpty(returnUrl) == false &&
                    string.Equals(returnUrl, "/") == false &&
                    Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction(nameof(SettingsController.Index), "Settings");
            }

            return View(login);
        }

        // POST: /Login/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _loginManager.Logout();

            return RedirectToAction("LogIn", "Login");
        }

        private new IActionResult Redirect(string returnUrl)
        {

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion Public Methods

    }

}
