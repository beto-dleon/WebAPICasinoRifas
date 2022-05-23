using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPICasinoRifas.DTOs;
using WebAPICasinoRifas.Entitys;
using WebAPICasinoRifas.Utilidades;

namespace WebAPICasinoRifas.Controllers
{
    [ApiController]
    [Route("api/rifas")]
    public class RifasController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<RifasController> logger;

        public RifasController(ApplicationDbContext context, IMapper mapper, ILogger<RifasController> logger)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
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

        

        [HttpGet("{idRifa:int}/NumerosDisponiblesEnRifa")]
        public async Task<ActionResult<List<int>>> GetNumeros(int idRifa)
        {
            var participacionesRifaDB = await dbContext.RifasConParticipantes.Where(x => x.RifaId == idRifa).ToListAsync();

            var numdisp = new List<int>();
            for (int i = 1; i <= 54; i++)
            {
                numdisp.Add(i);
            }

            foreach (var p in participacionesRifaDB)
            {
                foreach (var i in numdisp)
                {
                    if (i == p.NumeroLoteria)
                    {
                        numdisp.Remove(i);
                        break;
                    }
                }
            }


            return numdisp;
        }

        [HttpGet("consultar")]
        public async Task<ActionResult<List<GetRifaDTO>>> Get()
        {
            logger.LogInformation("Se estan obteniendo las Rifas");
            var rifas = await dbContext.Rifas.ToListAsync();
            return mapper.Map<List<GetRifaDTO>>(rifas);
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
