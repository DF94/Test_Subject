using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnboardingAPI.Resources;
using OnboardingApp.Data;
using OnboardingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingAPI
{
    public class Init
    {
        public static void SeedData(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager, OnboardingContext obContext)
        {
            //SeedRoles(roleManager);
            
            //SeedUsers(userManager);
            
            //SeedProjects(userManager, obContext);
            //SeedTasks(userManager, obContext);
        }

        

        public static void SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                roleManager.CreateAsync(new IdentityRole<int> { Name = "Manager" }).Wait();
            }

            if (!roleManager.RoleExistsAsync("Programmer").Result)
            {
                roleManager.CreateAsync(new IdentityRole<int> { Name = "Programmer" }).Wait();
            }
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("diogo@gmail.com").Result == null)
            {
                var model = new UserSignUp
                {
                    Email = "diogo@gmail.com",
                    Name = "Diogo Figueira",
                    Password = "SecurePassword123",
                    Role = "Programmer"
                };

                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, model.Role).Wait();
                }
            }

            if (userManager.FindByEmailAsync("james@gmail.com").Result == null)
            {
                var model = new UserSignUp
                {
                    Email = "james@gmail.com",
                    Name = "James Bond",
                    Password = "SecurePassword123",
                    Role = "Manager"
                };

                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, model.Role).Wait();
                }
            }

            if (userManager.FindByEmailAsync("boss@gmail.com").Result == null)
            {
                var model = new UserSignUp
                {
                    Email = "boss@gmail.com",
                    Name = "Tony Stark",
                    Password = "SecurePassword123",
                    Role = "Manager"
                };

                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, model.Role).Wait();
                }
            }

            if (userManager.FindByEmailAsync("hulk@gmail.com").Result == null)
            {
                var model = new UserSignUp
                {
                    Email = "hulk@gmail.com",
                    Name = "Bruce Banner",
                    Password = "SecurePassword123",
                    Role = "Programmer"
                };

                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, model.Role).Wait();
                }
            }
        }
    
        

        public static void SeedProjects(UserManager<User> userManager, OnboardingContext obContext)
        {
            var user2 = userManager.FindByEmailAsync("james@gmail.com").Result;
            var user3 = userManager.FindByEmailAsync("boss@gmail.com").Result;

            List<OnboardingApp.Domain.Task> Tasks1 = new List<OnboardingApp.Domain.Task>();
            List<OnboardingApp.Domain.Task> Tasks2 = new List<OnboardingApp.Domain.Task>();

            var task = new OnboardingApp.Domain.Task()
            {
                State = OnboardingApp.Domain.StateEnum.To_Do,
                Name = "Resolver problemas com o identity framework",
                LimitDate = DateTime.Now.AddDays(20),
                UserId = 1,
                ProjectId = 1
            };

            var task2 = new OnboardingApp.Domain.Task()
            {
                State = OnboardingApp.Domain.StateEnum.In_Progress,
                Name = "Resolver problemas com o angular",
                LimitDate = DateTime.Now.AddDays(30),
                UserId = 1,
                ProjectId = 1
            };

            var task3 = new OnboardingApp.Domain.Task()
            {
                State = OnboardingApp.Domain.StateEnum.To_Do,
                Name = "Push solution to git",
                LimitDate = DateTime.Now.AddDays(15),
                UserId = 1,
                ProjectId = 1
            };

            var task4 = new OnboardingApp.Domain.Task()
            {
                State = OnboardingApp.Domain.StateEnum.To_Do,
                Name = "Disapprove of Russia Decisions while pushing solution to git",
                LimitDate = DateTime.Now.AddDays(15),
                UserId = 4,
                ProjectId = 2
            };

            var task5 = new OnboardingApp.Domain.Task()
            {
                State = OnboardingApp.Domain.StateEnum.To_Do,
                Name = "Judge Diogo's coding capabilities",
                LimitDate = DateTime.Now.AddDays(15),
                UserId = 4,
                ProjectId = 2
            };

            Tasks1.Add(task);
            Tasks1.Add(task2);
            Tasks1.Add(task3);
            Tasks2.Add(task4);
            Tasks2.Add(task5);

            if (user2 != null && user3 != null)
            {
                var project = new Project()
                {
                    Name = "Project X",
                    UserId = user2.Id,
                    Money = 99999,
                    Tasks = Tasks1
                };


                var project2 = new Project()
                {
                    Name = "Project Y",
                    UserId = user3.Id,
                    Money = 100000,
                    Tasks = Tasks2
                };

                var project3 = new Project()
                {
                    Name = "Project Z",
                    UserId = user3.Id,
                    Money = 10101010,
                    Tasks = new List<OnboardingApp.Domain.Task>()
                };

                if (obContext.Projects.FirstOrDefault(x => x.Name == "Project X") == null)
                {
                    obContext.Projects.Add(project);
                    foreach(OnboardingApp.Domain.Task taskk in Tasks1)
                    {
                        obContext.Tasks.Add(taskk);
                    }
                }

                if (obContext.Projects.FirstOrDefault(x => x.Name == "Project Y") == null)
                {
                    obContext.Projects.Add(project2);
                    foreach (OnboardingApp.Domain.Task taskk in Tasks2)
                    {
                        obContext.Tasks.Add(taskk);
                    }
                }

                if (obContext.Projects.FirstOrDefault(x => x.Name == "Project Z") == null)
                {
                    obContext.Projects.Add(project3);
                }
                obContext.SaveChanges();
            }
        }

        public static void SeedTasks(UserManager<User> userManager, OnboardingContext obContext)
        {
            var user1 = userManager.FindByEmailAsync("diogo@gmail.com").Result;
            var user2 = userManager.FindByEmailAsync("hulk@gmail.com").Result;

            if (obContext.Tasks.FirstOrDefault(x => x.Name == "Resolver problemas com o identity framework") == null)
            {
                var project = obContext.Projects.FirstOrDefault(x => x.Id == 1 && x.Name == "Project X");
                if (project != null)
                {
                    var task = new OnboardingApp.Domain.Task()
                    {
                        State = OnboardingApp.Domain.StateEnum.To_Do,
                        Name = "Resolver problemas com o identity framework",
                        LimitDate = DateTime.Now.AddDays(20),
                        UserId = user1.Id,
                        ProjectId = project.Id
                    };
                    obContext.Tasks.Add(task);
                }
            }

            if (obContext.Tasks.FirstOrDefault(x => x.Name == "Resolver problemas com o angular") == null)
            {
                var project = obContext.Projects.FirstOrDefault(x => x.Id == 1 && x.Name == "Project X");
                if (project != null)
                {
                    var task2 = new OnboardingApp.Domain.Task()
                    {
                        State = OnboardingApp.Domain.StateEnum.In_Progress,
                        Name = "Resolver problemas com o angular",
                        LimitDate = DateTime.Now.AddDays(30),
                        UserId = user1.Id,
                        ProjectId = project.Id
                    };
                    obContext.Tasks.Add(task2);
                }
            }

            if (obContext.Tasks.FirstOrDefault(x => x.Name == "Push solution to git") == null)
            {
                var project = obContext.Projects.FirstOrDefault(x => x.Id == 1 && x.Name == "Project X");
                if (project != null)
                {
                    var task3 = new OnboardingApp.Domain.Task()
                    {
                        State = OnboardingApp.Domain.StateEnum.To_Do,
                        Name = "Push solution to git",
                        LimitDate = DateTime.Now.AddDays(15),
                        UserId = user1.Id,
                        ProjectId = project.Id
                    };
                    obContext.Tasks.Add(task3);
                }
            }

            if (obContext.Tasks.FirstOrDefault(x => x.Name == "Punch Putin while pushing solution to git") == null)
            {
                var project = obContext.Projects.FirstOrDefault(x => x.Id == 2 && x.Name == "Project Y");
                if (project != null)
                {
                    var task4 = new OnboardingApp.Domain.Task()
                    {
                        State = OnboardingApp.Domain.StateEnum.To_Do,
                        Name = "Disapprove of Russia Decisions while pushing solution to git",
                        LimitDate = DateTime.Now.AddDays(15),
                        UserId = user2.Id,
                        ProjectId = project.Id
                    };
                    obContext.Tasks.Add(task4);
                }
            }

            if (obContext.Tasks.FirstOrDefault(x => x.Name == "Judge Diogo's coding capabilities") == null)
            {
                var project = obContext.Projects.FirstOrDefault(x => x.Id == 1 && x.Name == "Project X");
                if (project != null)
                {
                    var task5 = new OnboardingApp.Domain.Task()
                    {
                        State = OnboardingApp.Domain.StateEnum.To_Do,
                        Name = "Judge Diogo's coding capabilities",
                        LimitDate = DateTime.Now.AddDays(15),
                        UserId = user2.Id,
                        ProjectId = project.Id
                    };
                    obContext.Tasks.Add(task5);
                }
            }

            obContext.SaveChanges();
        }
    }
}
