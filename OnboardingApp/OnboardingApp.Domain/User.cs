using Microsoft.AspNetCore.Identity;
using System;

namespace OnboardingApp.Domain
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
