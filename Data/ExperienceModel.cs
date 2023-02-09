using System.ComponentModel.DataAnnotations;

namespace SkillsAPI.Data
{
    public class ExperienceModel
    {
        public int Id {get;set;}
        public string Organization { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime? End { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
