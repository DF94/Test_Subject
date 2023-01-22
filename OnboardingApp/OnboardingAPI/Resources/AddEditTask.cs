using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingAPI.Resources
{
    public enum StateEnum
    {
        [Display(Name = "To Do")]
        To_Do = 1,
        [Display(Name = "In progress")]
        In_Progress = 2,
        [Display(Name = "Finished")]
        Finished = 3
    }

    public class AddEditTask
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Data Limit is Required")]
        public DateTime LimitDate { get; set; }

        [Required(ErrorMessage = "State is Required")]
        public int State { get; set; }

        [Required(ErrorMessage = "Programmer is Required")]
        public int Programmer { get; set; }

        [Required(ErrorMessage = "Programmer is Required")]
        public int Project { get; set; }

    }
}
