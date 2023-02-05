using System.ComponentModel.DataAnnotations;

namespace SkillsAPI.Data;

public class Kayttaja 
{
[Key]
public int Id {get;set;}
public string Kayttajanimi {get;set;} = null!;
public string Salasana {get;set;} = null!;

}

