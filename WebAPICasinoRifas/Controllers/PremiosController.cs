using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPICasinoRifas.DTOs;
using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas.Controllers
{
    [ApiController]
    [Route("api/premios")]
    public class PremiosController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public PremiosController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpPost("{CrearPremio}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] PremiosDTO premioDto)
        {
            var existePremioMismoNom = await dbContext.Premios.AnyAsync(x => x.Nombre == premioDto.Nombre);

            if(existePremioMismoNom)
            {
                return BadRequest($"Ya existe un premio con el nombre: {premioDto.Nombre}");
            }

            var premio = mapper.Map<Premio>(premioDto);

            dbContext.Add(premio);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

    }
}
