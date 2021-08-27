using DrinksMachineProgram.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DrinksMachineProgram.Authentication
{

    public class LoginManager : ILoginManager
    {

        private readonly IHttpContextAccessor _context;

        public LoginManager(IHttpContextAccessor context)
        {
            _context = context;
        }

        public async Task Login(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, "admin")
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "local", "name", "role");
            ClaimsPrincipal main = new ClaimsPrincipal(identity);

            await _context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, main);
        }

        public async Task Logout()
        {
            await _context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }

}
