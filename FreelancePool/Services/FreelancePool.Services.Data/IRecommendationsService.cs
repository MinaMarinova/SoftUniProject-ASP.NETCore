namespace FreelancePool.Services.Data
{
    using System.Threading.Tasks;

    public interface IRecommendationsService
    {
        Task CreateAsync(string authorId, string executorId, string recommendation);
    }
}
