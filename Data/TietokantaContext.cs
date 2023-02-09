using Microsoft.EntityFrameworkCore;
using SkillsAPI.Data;
namespace SkillsAPI.Data;
    public class TietokantaContext : DbContext
    {
        public TietokantaContext(DbContextOptions options) : base(options)
        {
        }

        protected TietokantaContext(){}
    

        public DbSet<Kayttaja> Kayttajat { get; set; } = default!;
    

        public DbSet<SkillsAPI.Data.Experience> Experience { get; set; } = default!;

    }