using System;
using System.ComponentModel.DataAnnotations;

namespace ContactAPI.Models
{
    public class SkillPutRequestModel : SkillRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ContactId { get; set; }
    }
}
