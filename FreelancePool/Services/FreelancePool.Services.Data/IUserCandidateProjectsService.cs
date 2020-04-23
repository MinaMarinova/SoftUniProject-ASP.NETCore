namespace FreelancePool.Services.Data
{
    using System.Threading.Tasks;

    public interface IUserCandidateProjectsService
    {
        Task ApplyAsync(string userId, int projectId);
    }
}
