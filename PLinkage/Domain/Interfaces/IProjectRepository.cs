using PLinkage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLinkage.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(Guid projectId);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Guid projectId);
    }
}
