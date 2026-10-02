using System.Collections.Generic;
using LilacTechSys.Domain.Common;

namespace LilacTechSys.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
