using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ContactAPI.Data.Repository.Interfaces;

namespace ContactAPI.Data.Models
{
    public partial class Contact : IUpdateTrackable
    {
        public Contact()
        {
            Skills = new HashSet<ContactSkill>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [StringLength(14)]
        public string Mobile { get; set; }
        [Required]
        [StringLength(5)]
        public string Zip { get; set; }
        [Required]
        [StringLength(1)]
        public string Sex { get; set; }
        

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.Contacts))]
        public virtual User CreatedUser { get; set; }
        [InverseProperty("Contact")]
        public virtual ICollection<ContactSkill> Skills { get; set; } 

        public string GetFullname()
        {
            return $"{LastName} {FirstName}";
        }
    }
}
