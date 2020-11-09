using System;
using System.ComponentModel.DataAnnotations;

namespace ContactAPI.Models
{
    public class PagingOptions
    {
        [Range(0, double.PositiveInfinity, ErrorMessage = "Offset must be positive.")]
        public int? Offset { get; set; } = 0;

        [Range(1, 50, ErrorMessage = "Limit must be greater than 0 and less than 50.")]
        public int? Limit { get; set; } = 30;
    }
}
