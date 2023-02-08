using System.ComponentModel.DataAnnotations;

namespace SkillsAPI.Data
{
    public class Experience
    {
        [Key]

        public int Id { get; set; }
        public Kayttaja Owner { get; set; } = null!;

        [Required]
        public string Organization { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        [Required]
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime? End { get; set; }
        public string Description { get; set; } = string.Empty;

        public TimeSpan GetDuration()
        {
            DateTime endValue = DateTime.Now;
            if (End.HasValue)
            {
                endValue = End.Value;
            }
            return endValue - Start;
        }

        public override string ToString()
        {
            string duration = string.Empty;
            if(GetDuration().TotalDays > 365)
            {
                duration = $"{GetDuration().TotalDays / 365}v";
            }

            if(GetDuration().TotalDays < 365)
            {
                duration = $"{GetDuration().TotalDays / 31}m";
            }

            return $"{Organization} - {Role} - {duration}";
        }
    }
}
