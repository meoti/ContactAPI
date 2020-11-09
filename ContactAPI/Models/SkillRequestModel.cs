using System.ComponentModel.DataAnnotations;

namespace ContactAPI.Models
{
    public class SkillRequestModel
    {
        [Required]
        [MinLength(5)] // hmm will need more info to validate skill name
        public string Name { get; set; }

        [Required]
        public ContactEnum.Expertise Level { get; set; }
    }
}
