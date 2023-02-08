using System.ComponentModel.DataAnnotations;

namespace SkillsAPI.Data;

    public class Kayttaja {
    [Key]
    public int Id {get;set;}
    public string Kayttajanimi {get;set;} = null!;
    public byte[] Salasana {get;set;} = null!;
    public virtual ICollection<Experience> Experience { get;set;} = new List<Experience>();

    }

