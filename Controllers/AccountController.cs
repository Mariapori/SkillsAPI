using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillsAPI.Data;
using SkillsAPI.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SkillsAPI.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly TietokantaContext _db;
    private readonly IConfiguration _config;
    private const string salt = "Suol44123!";

    public AccountController(ILogger<AccountController> logger, TietokantaContext db, IConfiguration config)
    {
        _logger = logger;
        _db = db;
        _config = config;
    }

    [HttpPost]
    [Route("api/login")]
    public async Task<IResult> Login(Kirjautumismodel model)
    {
        if(string.IsNullOrEmpty(model.Kayttajanimi) || string.IsNullOrEmpty(model.Salasana))
        {
            return Results.Unauthorized();
        }

        var user = await _db.Kayttajat.FirstOrDefaultAsync(o => o.Kayttajanimi == model.Kayttajanimi);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        var valid = PasswordService.VerifyHash(Encoding.UTF8.GetBytes(model.Salasana), Encoding.UTF8.GetBytes(salt),user.Salasana);

        if (!valid) 
        { 
            return Results.Unauthorized();
        }

        string? issuer = _config["Jwt:Issuer"];
        string? audience = _config["Jwt:Audience"];
        string? jwtKey = _config["Jwt:Key"];

        if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(jwtKey))
        {
            return Results.Unauthorized();
        }

        var key = Encoding.ASCII.GetBytes(jwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, model.Kayttajanimi),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);

        return Results.Ok(stringToken);
    }
    [HttpPost]
    [Route("api/register")]
    public async Task<IResult> Register(Kirjautumismodel model)
    {
        if (string.IsNullOrEmpty(model.Kayttajanimi) || string.IsNullOrEmpty(model.Salasana))
        {
            return Results.Unauthorized();
        }

        if(_db.Kayttajat.Where(o => o.Kayttajanimi == model.Kayttajanimi).Count() > 0)
        {
            return Results.Unauthorized();
        }

        await _db.Kayttajat.AddAsync(new Kayttaja { Kayttajanimi = model.Kayttajanimi, Salasana = PasswordService.CreateHash(Encoding.UTF8.GetBytes(model.Salasana),Encoding.UTF8.GetBytes(salt)) });
        await _db.SaveChangesAsync();

        return Results.Ok();
    }
}
