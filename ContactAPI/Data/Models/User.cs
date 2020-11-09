using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAPI.Data.Models
{
    public partial class User
    {
        public User()
        {
            Contacts = new HashSet<Contact>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }
        [Required]
        [StringLength(256)]
        public string PasswordSalt { get; set; }

        [InverseProperty(nameof(Contact.CreatedUser))]
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
