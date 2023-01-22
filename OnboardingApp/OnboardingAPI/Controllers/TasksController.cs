using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnboardingAPI.Resources;
using OnboardingApp.Data;
using OnboardingApp.Data.SqlQuerys;
using OnboardingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnboardingAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ISQLFectch SqlFetch;
        private readonly UserManager<User> uManager;
        private readonly SignInManager<User> sManager;
        private readonly OnboardingContext context;


        public TasksController(ISQLFectch _sQLFectch,
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
        [Route("GetManagerTasks")]
        public async Task<IActionResult> GetManagerTasksAsync(int projectId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await uManager.FindByIdAsync(userId);
            var tasks = SqlFetch.GetTasksOfProjectForManager(projectId);

            return Ok(tasks);
        }

        [Authorize(Roles = "Programmer")]
        [HttpGet]
        [Route("GetProgrammerTasks")]
        public async Task<IActionResult> GetProgrammerTasksAsync(int projectId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await uManager.FindByIdAsync(userId);
            var tasks = SqlFetch.GetTasksOfProjectForProgrammer(user.Id, projectId);

            return Ok(tasks);
        }

        [Authorize(Roles = "Manager, Programmer")]
        [HttpPost]
        [Route("GetTasks")]
        public async Task<JsonResult> GetTasks([FromBody] int projectId)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await uManager.FindByIdAsync(userId);

            if (userRole == "Manager")
            {
                var tasks = SqlFetch.GetTasksOfProjectForManager(projectId);
                return new JsonResult(tasks);
            }
            else
            {
                var tasks = SqlFetch.GetTasksOfProjectForProgrammer(user.Id, projectId);
                return new JsonResult(tasks);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("GetProgrammers")]
        public async Task<IActionResult> GetProgrammers()
        {
            var users = await uManager.GetUsersInRoleAsync("Programmer");

            return Ok(users);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("CreateTaskAsync")]
        public async Task<IActionResult> CreateTaskAsync([FromBody] AddEditTask model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await uManager.FindByIdAsync(userId);

                var Task = new OnboardingApp.Domain.Task()
                {
                    Name = model.Name,
                    UserId = model.Programmer,
                    State = (OnboardingApp.Domain.StateEnum)model.State,
                    LimitDate = model.LimitDate,
                    ProjectId = model.Project
                };

                context.Tasks.Add(Task);
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        public class AddEditTaskRes
        {
            public AddEditTask model { get; set; }
            public int taskId { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Programmer")]
        [Route("EditTaskAsync")]
        public async Task<IActionResult> EditTaskAsync([FromBody] AddEditTaskRes editTaskRes)
        {
            if (ModelState.IsValid)
            {
                int id = editTaskRes.taskId;
                AddEditTask model = editTaskRes.model;

                var taskToEdit = await context.Tasks.FindAsync(id);

                taskToEdit.LimitDate = model.LimitDate;
                taskToEdit.Name = model.Name;
                taskToEdit.State = (OnboardingApp.Domain.StateEnum)model.State;
                taskToEdit.UserId = model.Programmer;
                taskToEdit.ProjectId = model.Project;

                context.Tasks.Update(taskToEdit);

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
        [Route("DeleteTaskAsync")]
        public async Task<IActionResult> DeleteTaskAsync([FromBody] int Id)
        {
            var taskToDelete = await context.Tasks.FindAsync(Id);
            context.Tasks.Remove(taskToDelete);
            await context.SaveChangesAsync();

            return Ok();
        }

    }
}
