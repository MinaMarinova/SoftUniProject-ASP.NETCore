using FreelancePool.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FreelancePool.Data.Models
{
    public class Message : BaseDeletableModel<int>
    {
        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
