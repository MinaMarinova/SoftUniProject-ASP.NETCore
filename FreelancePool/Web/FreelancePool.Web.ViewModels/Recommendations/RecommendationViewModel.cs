namespace FreelancePool.Web.ViewModels.Recommendations
{
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Users;

    public class RecommendationViewModel : IMapFrom<Recommendation>
    {
        public string Content { get; set; }

        public UserCardViewModel Author { get; set; }
    }
}
