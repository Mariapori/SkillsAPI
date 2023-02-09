using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsAPI.Data;

namespace SkillsAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly TietokantaContext _context;

        public ExperienceController(TietokantaContext context)
        {
            _context = context;
        }

        [HttpGet("GetExperienceByUser")]
        [AllowAnonymous]
        public async Task<IResult> GetExperienceByUser(string user)
        {
            var results = await _context.Experience.Include(o => o.Owner).Where(o => o.Owner.Kayttajanimi == user).ToListAsync();
            if(!results.Any()){
                return Results.NotFound();
            }
            var expList = new List<ExperienceModel>();

            foreach (var item in results.OrderByDescending(o => o.Start))
            {
                expList.Add(new ExperienceModel{
                    Id = item.Id,
                    Organization = item.Organization,
                    Role = item.Role,
                    Start = item.Start,
                    End = item.End
                });
            }
            return Results.Ok(expList);
        }

    }
}
