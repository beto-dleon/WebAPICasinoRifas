using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPICasinoRifas.DTOs;
using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas.Controllers
{
    [ApiController]
    [Route("api/tarjeta")]
    public class RifaConParticipanteController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public RifaConParticipanteController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpPost("{RegistrarParticipacion}")]
        [Authorize]
        public async Task<ActionResult> Post(int ParticipanteId, int RifaId, RifaConParticipanteDTO rcpDto)
        {
            var mismoNum = await dbContext.RifasConParticipantes.AnyAsync(x => x.NumeroLoteria == rcpDto.NumeroLoteria);

            if( mismoNum)
            {
                return BadRequest($"Ya existe un participante con este número de lotería asignado");
            }

            var rcp = mapper.Map<RifaConParticipante>(rcpDto);
            rcp.Ganador = false;
            dbContext.Add(rcp);
            await dbContext.SaveChangesAsync();

            return Ok();

        }

    }
}
