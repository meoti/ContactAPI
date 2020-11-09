using System.ComponentModel;

namespace ContactAPI.Models
{
    public static class ContactEnum
    {
        public enum Sex
        {
            [Description("Male")]
            M = 1,

            [Description("Female")]
            F
        }

        public enum Expertise
        {
            [Description("Beginner")]
            Beginner = 1,

            [Description("Junior")]
            Junior,

            [Description("Mid-level")]
            Intermidiate,

            [Description("Senior")]
            Senior,

            [Description("Expert")]
            Expert
        }

        public enum TrackableTypes
        {
            Contacts,
            Skills
        }
    }
}
