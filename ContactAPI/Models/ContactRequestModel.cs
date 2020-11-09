using System.ComponentModel.DataAnnotations;

namespace ContactAPI.Models
{
    public class ContactRequestModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "The email address is not valid")]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(14)]
        [RegularExpression(@"^((0041|0)|\+41)(\s?\(0\))?(\s)?[1-9]{2}(\s)?[0-9]{3}(\s)?[0-9]{2}(\s)?[0-9]{2}$")]
        public string Mobile { get; set; }

        [Required]
        [StringLength(5)]
        public string Zip { get; set; }

        [Required]
        public ContactEnum.Sex Sex { get; set; }
    }
}
