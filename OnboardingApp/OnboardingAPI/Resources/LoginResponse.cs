using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingAPI.Resources
{
    public class LoginResponse
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }

    }
}
