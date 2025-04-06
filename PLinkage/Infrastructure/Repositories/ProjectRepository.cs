using Newtonsoft.Json;
using PLinkage.Domain.Interfaces;
using PLinkage.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace PLinkage.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _projectFilePath;

        public ProjectRepository()
        {
            // Get project base path
            string _projectPath = AppDomain.CurrentDomain.BaseDirectory;

            // Get the full path to the JSON folder
            string _jsonFolderPath = Path.GetFullPath(Path.Combine(_projectPath, @"..\..\..\..\..\json"));

            // Combine folder path with the file name for projects
            _projectFilePath = Path.Combine(_jsonFolderPath, "Projects.txt");
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            if (File.Exists(_projectFilePath))
            {
                var json = await File.ReadAllTextAsync(_projectFilePath);
                return JsonConvert.DeserializeObject<List<Project>>(json) ?? new List<Project>();
            }
            return new List<Project>();
        }

        public async Task<Project?> GetProjectByIdAsync(Guid projectId)
        {
            var projects = await GetAllProjectsAsync();
            return projects.Find(p => p.ProjectId == projectId);
        }

        public async Task AddProjectAsync(Project project)
        {
            var projects = await GetAllProjectsAsync();
            projects.Add(project);
            await SaveToFileAsync(projects);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var projects = await GetAllProjectsAsync();
            var existingProject = projects.Find(p => p.ProjectId == project.ProjectId);
            if (existingProject != null)
            {
                projects.Remove(existingProject);
                projects.Add(project);
                await SaveToFileAsync(projects);
            }
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            var projects = await GetAllProjectsAsync();
            var project = projects.Find(p => p.ProjectId == projectId);
            if (project != null)
            {
                projects.Remove(project);
                await SaveToFileAsync(projects);
            }
        }

        private async Task SaveToFileAsync(List<Project> projects)
        {
            var json = JsonConvert.SerializeObject(projects, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync(_projectFilePath, json);
        }
    }
}
