using System;
using System.ComponentModel.DataAnnotations;

namespace SkillsAPI.Data;

    public class Kayttaja {
    [Key]
    public int Id {get;set;}
    public string Kayttajanimi {get;set;} = null!;
    public byte[] Salasana {get;set;} = null!;
    public virtual ICollection<Experience> Experience { get;set;} = new List<Experience>();

    public int KokemusInYears {
        get
        {
            int years = 0;
            var zeroTime = new DateTime(1, 1, 1);
            foreach (var experience in this.Experience)
            {
                years += (zeroTime + experience.GetDuration()).Year - 1;
            }
            return years;
        }
    }

    }

