using System;
using System.ComponentModel.DataAnnotations;

namespace ContactAPI.Models
{
    public class ContactPutRequestModel : ContactRequestModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
