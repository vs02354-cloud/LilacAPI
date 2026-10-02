using System;
using System.Collections.Generic;

namespace LilacTechSys.Application.DTOs
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string DetailedDescription { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<string> Features { get; set; } = new List<string>();
        public List<string> Benefits { get; set; } = new List<string>();
        public List<string> Technologies { get; set; } = new List<string>();
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateServiceRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string DetailedDescription { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<string> Features { get; set; } = new List<string>();
        public List<string> Benefits { get; set; } = new List<string>();
        public List<string> Technologies { get; set; } = new List<string>();
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateServiceRequest : CreateServiceRequest
    {
    }
}
