namespace FreelancePool.Services.Data.Tests.Common
{
    using System.Reflection;

    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Categories;
    using FreelancePool.Web.ViewModels.Components;

    public class MapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(CategoryListViewModel).GetTypeInfo().Assembly,
                typeof(FreelancerViewModel).GetTypeInfo().Assembly);
        }
    }
}
