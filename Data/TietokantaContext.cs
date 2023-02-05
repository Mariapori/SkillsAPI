using Microsoft.EntityFrameworkCore;
namespace SkillsAPI.Data;
    public class TietokantaContext : DbContext
    {
        public TietokantaContext(DbContextOptions options) : base(options)
        {
        }

        protected TietokantaContext(){}
    

        public virtual DbSet<Kayttaja> Kayttajat { get; set; } = null!;

    }