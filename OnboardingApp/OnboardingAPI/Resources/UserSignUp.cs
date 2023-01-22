using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingAPI.Resources
{
    public class UserSignUp
    {
        [Required(ErrorMessage = "EMail is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        public string Role { get; set; }
    }
}
