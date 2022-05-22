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
    [Route("api/rifas")]
    public class RifasController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public RifasController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetRifaDTO>> Get(int id)
        {
            var rifa = await dbContext.Rifas.
                Include(r=> r.Premios).
                FirstOrDefaultAsync(rifaBD => rifaBD.Id == id);

            if(rifa == null)
            {
                return NotFound();
            }

            return mapper.Map<GetRifaDTO>(rifa);
        }

        [HttpPost("CrearRifa")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        public async Task<IActionResult> Post([FromBody] RifaCreacionDTO rifaCreacionDTO)
        {
            var existeRifaMismoNom = await dbContext.Rifas.AnyAsync(x => x.Nombre == rifaCreacionDTO.Nombre);

            if (existeRifaMismoNom)
            {
                return BadRequest($"Ya existe una rifa con el nombre {rifaCreacionDTO.Nombre}");
            }

            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);

            dbContext.Add(rifa);
            await dbContext.SaveChangesAsync();

            return Ok();

        }


        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Rifas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El recurso no fue encontrado");
            }

            dbContext.Remove(new Rifa { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();

        }

    }
}
