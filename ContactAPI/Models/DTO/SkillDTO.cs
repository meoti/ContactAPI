using System;

namespace ContactAPI.Models.DTO
{
    public class SkillDTO
    {
        public Guid Id { get; set; }

        public Guid SkillId { get; set; }

        public string SkillName { get; set; }

        public string SkillLevel { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
