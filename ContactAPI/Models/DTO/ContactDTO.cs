using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace ContactAPI.Models.DTO
{
    public class ContactDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Fullname { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string Zip { get; set; }

        public string Sex { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<SkillDTO> Skills { get; set; }

    }
}
