using Microsoft.EntityFrameworkCore;
using OnboardingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnboardingApp.Data.SqlQuerys
{
    public class SQLFectch : ISQLFectch
    {
        private readonly OnboardingContext context;

        public SQLFectch(OnboardingContext obContext)
        {
            context = obContext;
        }

        public IEnumerable<Project> GetProjectsOfManager(int ManagerId)
        {
            return context.Projects
                .Where(o => o.UserId == ManagerId)
                .ToList();
        }

        public IEnumerable<Project> GetProjectsOfProgrammer(int ProgrammerId)
        {
            var tasks = context.Tasks
                .Where(o => o.UserId == ProgrammerId)
                .ToList();

            List<int> tasksIDs = new List<int>();

            foreach (var task in tasks)
            {
                tasksIDs.Add(task.ProjectId);
            }

            return context.Projects
                .Where(o => tasksIDs.Contains(o.Id))
                .ToList();
        }

        public IEnumerable<Task> GetTasksOfProjectForManager(int ProjectId)
        {
            return context.Tasks
                .Where(o => o.ProjectId == ProjectId) 
                .ToList();
        }

        public IEnumerable<Task> GetTasksOfProjectForProgrammer(int UserId, int ProjectId)
        {
            return context.Tasks
                .Where(o => o.UserId == UserId && o.ProjectId == ProjectId)
                .ToList();
        }

        public IEnumerable<Project> GetProjects()
        {
            return context.Projects
                .ToList();
        }
        public Project GetProjectById(int id)
        {
            return context.Projects
                .Where(o => o.Id == id)
                .Include(o=>o.Tasks)
                .FirstOrDefault();
        }

        public Task GetTaskById(int id)
        {
            return context.Tasks
                .Where(o => o.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Task> GetTasksOfProject(int ProjectId)
        {
            return context.Tasks
                .Where(o => o.ProjectId == ProjectId)
                .ToList();
        }

    }
}
