using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnboardingApp.Data.SqlQuerys;
using OnboardingApp.Domain;
using System.Security.Claims;
using OnboardingApp.Data;
using OnboardingAPI.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OnboardingAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ISQLFectch SqlFetch;
        private readonly UserManager<User> uManager;
        private readonly SignInManager<User> sManager;
        private readonly OnboardingContext context;

        public ProjectsController(ISQLFectch _sQLFectch,
                                  UserManager<User> UManager,
                                  SignInManager<User> SManager,
                                  OnboardingContext obContext)
        {
            SqlFetch = _sQLFectch;
            uManager = UManager;
            sManager = SManager;
            context = obContext;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("GetManagerProjects")]
        public async Task<IActionResult> GetManagerProjectsAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await uManager.FindByIdAsync(userId);
            var projects = SqlFetch.GetProjectsOfManager(user.Id);

            return Ok(projects);
        }

        [Authorize(Roles = "Programmer")]
        [HttpGet]
        [Route("GetProgrammerProjects")]
        public async Task<IActionResult> GetProgrammerProjectsAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await uManager.FindByIdAsync(userId);
            var projects = SqlFetch.GetProjectsOfProgrammer(user.Id);

            return Ok(projects);
        }

        [Authorize(Roles = "Programmer, Manager")]
        [HttpGet]
        [Route("GetProjects")]
        public async Task<JsonResult> GetProjectsAsync()
        {
            //var userMail = User.FindFirstValue(ClaimTypes.Name);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await uManager.FindByIdAsync(userId);

            if (userRole == "Manager")
            {
                var projects = SqlFetch.GetProjectsOfManager(user.Id);
                return new JsonResult(projects);
            }
            else
            {
                var projects = SqlFetch.GetProjectsOfProgrammer(user.Id);
                return new JsonResult(projects);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("CreateProjectAsync")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] AddEditProject model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await uManager.FindByIdAsync(userId);

                var Project = new Project()
                {
                    Name = model.Name,
                    Money = model.Money,
                    UserId = user.Id,
                    Tasks = new List<OnboardingApp.Domain.Task>()
                };

                context.Projects.Add(Project);
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        public class EditProjectRes
        {
            public AddEditProject model { get; set; }
            public int projectId { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("EditProjectAsync")]
        public async Task<IActionResult> EditProjectAsync([FromBody] EditProjectRes editProjectRes)
        {
            if (ModelState.IsValid)
            {
                int id = editProjectRes.projectId;
                AddEditProject model = editProjectRes.model;

                var project = SqlFetch.GetProjectById(id);

                project.Money = model.Money;
                project.Name = model.Name;

                context.Projects.Update(project);
                
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("DeleteProjectAsync")]
        public async Task<IActionResult> DeleteProjectAsync([FromBody] int id)
        {
            var project = SqlFetch.GetProjectById(id);

            if(project.Tasks != null)
            {
                //var taskList = SqlFetch.GetTasksOfProject(id);

                foreach (OnboardingApp.Domain.Task t in project.Tasks)
                {
                    context.Tasks.Remove(t);
                };

                //context.Tasks.RemoveRange(taskList);
            }

            context.Projects.Remove(project);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
