using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ContactAPI.Data.Repository.Interfaces;
using ContactAPI.Models;

namespace ContactAPI.Data.Models
{
    [Table("Contact_Skills")]
    public partial class ContactSkill : IUpdateTrackable
    {
        public Guid Id { get; set; }
        [Key]
        [Column("Contact_Id")]
        public Guid ContactId { get; set; }
        [Key]
        [Column("Skill_id")]
        public Guid SkillId { get; set; }
        [Column(TypeName = "datetime")]


        [ForeignKey(nameof(ContactId))]
        [InverseProperty(nameof(Models.Contact.Skills))]
        public virtual Contact Contact { get; set; }

        [ForeignKey(nameof(SkillId))]
        [InverseProperty(nameof(Models.Skill.Contacts))]
        public virtual Skill Skill { get; set; }

        public string GetSkillName()
        {
            return Skill.Name;
        }

        public string GetSkillLevel()
        {
            return ((ContactEnum.Expertise) Skill.Expertise).ToString();
        }

        public string GetContactName()
        {
            return Contact.GetFullname();
        }

        public string GetContactId()
        {
            return Contact.Id.ToString();
        }
        public DateTime ContactCreatedAt()
        {
            return Contact.CreatedAt;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
