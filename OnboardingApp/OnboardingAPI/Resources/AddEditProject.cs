using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingAPI.Resources
{
    public class AddEditProject
    {
        [Required(ErrorMessage = "Budget is Required")]
        public double Money { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
    }
}
