using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingApp.Domain
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

    public class Task
    {
        public int Id { get; set; }
        public StateEnum State { get; set; }
        public string Name { get; set; }
        public DateTime LimitDate { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
