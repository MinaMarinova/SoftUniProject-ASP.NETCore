﻿namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;

    public interface IProjectsService
    {
        IEnumerable<T> GetLastProjects<T>(ApplicationUser user);

        Task<int> CreateAsync(string title, string description, string authorId);

        T GetById<T>(int id);

        Task CloseAsync(int id, string executorId);

        string GetAuthorId(int id);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetMostWanted<T>();

        string GetTitleById(int projectId);

        Task<int> DeleteAsync(string title, string authorEmail);
    }
}
