using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;

namespace ContactAPI.Data.Repository.Interfaces
{
    public abstract class IUpdateTrackable
    {
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }

        public void InsertDBDateTrackingInfo(IEnumerable<Claim> claims)
        {
            string subId = claims.SingleOrDefault(p => p.Type.ToLower() == "id")?.Value;
            if ( CreatedAt == DateTime.MinValue )
            {
                CreatedAt = DateTime.Now;
                CreatedBy = new Guid(subId);
            }

            UpdatedAt = DateTime.Now;
            UpdatedBy = new Guid(subId);
        }
    }
}
