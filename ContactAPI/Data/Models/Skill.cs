using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAPI.Data.Models
{
    public partial class Skill
    {
        public Skill()
        {
            Contacts = new HashSet<ContactSkill>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int Expertise { get; set; }


        [InverseProperty(nameof(ContactSkill.Skill))]
        public virtual ICollection<ContactSkill> Contacts { get; set; }
    }
}
