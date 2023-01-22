using OnboardingApp.Domain;
using System.Collections.Generic;

namespace OnboardingApp.Data.SqlQuerys
{
    public interface ISQLFectch
    {
        IEnumerable<Project> GetProjects();
        IEnumerable<Project> GetProjectsOfManager(int ManagerId);
        IEnumerable<Project> GetProjectsOfProgrammer(int ProgrammerId);
        IEnumerable<Task> GetTasksOfProjectForManager(int ProjectId);
        IEnumerable<Task> GetTasksOfProjectForProgrammer(int UserId, int ProjectId);
        Task GetTaskById(int id);
        Project GetProjectById(int id);
        IEnumerable<Task> GetTasksOfProject(int ProjectId);

    }
}