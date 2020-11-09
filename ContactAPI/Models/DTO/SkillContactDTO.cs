using System;
using System.Collections.Generic;

namespace ContactAPI.Models.DTO
{
    public class SkillContactDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Level { get; set; }

        public IEnumerable<ContactInfoDTO> Contacts { get; set; }
    }

    public class ContactInfoDTO
    {
        public string ContactName { get; set; }
        public string ContactId { get; set; }

        public DateTime ContactCreatedAt { get; set; }
    }
}
