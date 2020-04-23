namespace FreelancePool.Web.ViewModels.Projects
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using Microsoft.AspNetCore.Mvc;

    public class CloseProjectViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Executor username")]
        public string ExecutorUserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Executor email")]
        [Remote(action: "IsEmailValid", controller: "Projects")]
        public string ExecutorEmail { get; set; }

        public int StarGivenOrTaken { get; set; }

        [StringLength(500)]
        public string Recommendation { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Project, CloseProjectViewModel>().ForMember(
                m => m.Title,
                opt => opt.MapFrom(x => x.Title));
        }
    }
}
