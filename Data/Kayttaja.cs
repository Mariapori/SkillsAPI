using System;
using System.ComponentModel.DataAnnotations;

namespace SkillsAPI.Data;

    public class Kayttaja {
    [Key]
    public int Id {get;set;}
    public string Kayttajanimi {get;set;} = null!;
    public byte[] Salasana {get;set;} = null!;
    public virtual ICollection<Experience> Experience { get;set;} = new List<Experience>();

    public decimal KokemusInYears {
        get
        {
            decimal years = 0;
            foreach (var experience in this.Experience)
            {
                years += (decimal)(experience.GetDuration().TotalDays / 365.2425);
            }
            return Math.Round(years,2);
        }
    }

    }

