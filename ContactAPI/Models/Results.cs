using System.Collections.Generic;

namespace ContactAPI.Models
{
    public class Results<T> where T : class
    {
        public IEnumerable<string> Errors { get; set; }
        public bool Succeded { get; set; }

        public T Value { get; set; }
    }
}
