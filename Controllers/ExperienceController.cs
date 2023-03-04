using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpGet("GetExperinceYears")]
        [AllowAnonymous]
        public async Task<IResult> GetExperinceYears(string user)
        {
            var result = await _context.Kayttajat.Include(o => o.Experience).FirstOrDefaultAsync(o => o.Kayttajanimi == user);
            if (result == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result.KokemusInYears);
        }

        [HttpGet]
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
                    Start = item.Start.Date,
                    End = item.End.HasValue ? item.End.Value.Date : null,
                    Description = item.Description
                });
            }
            return Results.Ok(expList);
        }

        [HttpPost]
        public async Task<IResult> PostExperience(ExperienceModel model){
            var kayttaja = _context.Kayttajat.Include(o => o.Experience).FirstOrDefault(o => o.Kayttajanimi == User.Identity.Name);
            if(kayttaja == null){
                return Results.Unauthorized();
            }
            var entry = new Experience{
                Organization = model.Organization,
                Role = model.Role,
                Start = model.Start,
                End = model.End,
                Description = model.Description};
            kayttaja.Experience.Add(entry);
            await _context.SaveChangesAsync();
            return Results.Ok();
        }

        [HttpPut]
        public async Task<IResult> UpdateExperience(ExperienceModel model){
            var kayttaja = _context.Kayttajat.Include(o => o.Experience).FirstOrDefault(o => o.Kayttajanimi == User.Identity.Name);
            if(kayttaja == null){
                return Results.Unauthorized();
            }
            var item = kayttaja.Experience.FirstOrDefault(o => o.Id == model.Id);
            if(item == null || item.Owner.Kayttajanimi != User.Identity.Name){
                return Results.Unauthorized();
            }

            item.Description = model.Description;
            item.End = model.End;
            item.Organization = model.Organization;
            item.Role = model.Role;
            item.Start = model.Start;

            await _context.SaveChangesAsync();
            return Results.Ok();
        }
        [HttpDelete]
        public async Task<IResult> DeleteExperience(ExperienceModel model){
            var kayttaja = _context.Kayttajat.Include(o => o.Experience).FirstOrDefault(o => o.Kayttajanimi == User.Identity.Name);
            if(kayttaja == null){
                return Results.Unauthorized();
            }
            var item = kayttaja.Experience.FirstOrDefault(o => o.Id == model.Id);
            if(item == null || item.Owner.Kayttajanimi != User.Identity.Name){
                return Results.Unauthorized();
            }

            kayttaja.Experience.Remove(item);
            await _context.SaveChangesAsync();
            return Results.Ok();
        }

    }
}
