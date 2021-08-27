using Microsoft.AspNetCore.Http;

using System;
using System.Security.Claims;

namespace DrinksMachineProgram.Authentication
{

    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated ?? false;

        public int Id => Convert.ToInt32(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Sid)?.Value);

        public string FirstName => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.GivenName)?.Value;

        public string LastName => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Surname)?.Value;

        public string UserName => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;

        public string FullName => string.IsNullOrWhiteSpace(FirstName) == false ? $"{LastName}, {FirstName}" : LastName;

    }

}