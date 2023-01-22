using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingApp.Domain
{
    public class Project
    {
        public int Id { get; set; }
        public double Money { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public List<Task> Tasks { get; set; }

    }
}
