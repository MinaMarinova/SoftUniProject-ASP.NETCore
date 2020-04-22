namespace FreelancePool.Services.Data.Tests.Common
{
    using System.Reflection;

    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Categories;
    using FreelancePool.Web.ViewModels.Components;
    using FreelancePool.Web.ViewModels.Projects;

    public class MapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(AllCategoriesViewModel).GetTypeInfo().Assembly,
                typeof(CategoryListViewModel).GetTypeInfo().Assembly,
                typeof(FreelancerViewModel).GetTypeInfo().Assembly,
                typeof(CloseProjectViewModel).GetTypeInfo().Assembly,
                typeof(ProjectDetailsViewModel).GetTypeInfo().Assembly,
                typeof(ProjectViewModel).GetTypeInfo().Assembly,
                typeof(ProjectTitleAndApplicantsViewModel).Assembly);
        }
    }
}
