﻿namespace FreelancePool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class ProjectsService : IProjectsService
    {
        private const int NumberOfRecentProjects = 8;
        private const int NumberOfPopularProject = 5;
        private const string ProjectNotFoundMessage = "There is no project with title: {0} in the database!";
        private const string AuthorEmailNotMatch = "The author's email of the project is not {0}";

        private readonly IDeletableEntityRepository<Project> projectsRepository;

        public ProjectsService(
            IDeletableEntityRepository<Project> projectsRepository)
        {
            this.projectsRepository = projectsRepository;
        }

        public async Task CloseAsync(int id, string executorId)
        {
            var project = this.projectsRepository.All()
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (project != null)
            {
                project.Status = ProjectStatus.Finished;
                project.ExecutorId = executorId;

                await this.projectsRepository.SaveChangesAsync();
            }
        }

        public async Task<int> CreateAsync(string title, string description, string authorId)
        {
            var project = new Project
            {
                Title = title,
                Description = description,
                Status = ProjectStatus.Open,
                AuthorId = authorId,
            };

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            return project.Id;
        }

        public T GetById<T>(int id)
        {
            return this.projectsRepository.All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public IEnumerable<T> GetLastProjects<T>(ApplicationUser user)
        {
            var projects = this.projectsRepository.All().Where(p => (int)p.Status == 1);

            if (user != null && user.UserCategories.Count > 0)
            {
                List<string> userCategories = user.UserCategories.Select(uc => uc.Category.Name).ToList();

                projects = projects.Where(p => p.ProjectCategories.Select(pc => pc.Category.Name).Any(x => userCategories.Contains(x)));
            }

            return projects
                .OrderByDescending(p => p.CreatedOn)
                .Take(NumberOfRecentProjects)
                .To<T>()
                .ToList();
        }

        public string GetAuthorId(int id)
        {
            return this.projectsRepository.All()
                .Where(p => p.Id == id)
                .Select(p => p.AuthorId)
                .FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.projectsRepository.All()
                .Where(p => (int)p.Status == 1)
                .OrderByDescending(p => p.CreatedOn)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetMostWanted<T>()
        {
            var projects = this.projectsRepository.All()
                .Where(p => (int)p.Status == 1)
                .OrderByDescending(p => p.AppliedUsers.Count)
                .Take(NumberOfPopularProject);

            return projects.To<T>().ToList();
        }

        public string GetTitleById(int projectId)
        {
            return this.projectsRepository.All()
                .Where(p => p.Id == projectId)
                .Select(p => p.Title)
                .FirstOrDefault();
        }

        public async Task<int> DeleteAsync(string title, string authorEmail)
        {
            var project = this.projectsRepository.All()
                .Where(p => p.Title == title)
                .FirstOrDefault();

            if (project == null)
            {
                throw new ArgumentNullException(string.Format(ProjectNotFoundMessage, title));
            }

            if (project.Author.Email != authorEmail)
            {
                throw new ArgumentNullException(string.Format(AuthorEmailNotMatch, authorEmail));
            }

            project.IsDeleted = true;
            return await this.projectsRepository.SaveChangesAsync();
        }
    }
}
