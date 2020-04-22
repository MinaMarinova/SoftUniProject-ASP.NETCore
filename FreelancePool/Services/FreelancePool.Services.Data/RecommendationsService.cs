namespace FreelancePool.Services.Data
{
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;

    public class RecommendationsService : IRecommendationsService
    {
        private readonly IDeletableEntityRepository<Recommendation> recommendationsRepository;

        public RecommendationsService(IDeletableEntityRepository<Recommendation> recommendationsRepository)
        {
            this.recommendationsRepository = recommendationsRepository;
        }

        public async Task CreateAsync(string authorId, string executorId, string recommendation)
        {
            if (!string.IsNullOrWhiteSpace(recommendation))
            {
                var recommendationToAdd = new Recommendation
                {
                    AuthorId = authorId,
                    RecipientId = executorId,
                    Content = recommendation,
                };

                await this.recommendationsRepository.AddAsync(recommendationToAdd);
                await this.recommendationsRepository.SaveChangesAsync();
            }
        }
    }
}
