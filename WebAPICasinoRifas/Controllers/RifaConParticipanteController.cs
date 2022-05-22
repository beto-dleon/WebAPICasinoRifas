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
    [Route("api/tarjeta")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
    public class RifaConParticipanteController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public RifaConParticipanteController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet("{idRifa:int}/Ganador")]
        public async Task<ActionResult<Object>> Ganador(int idRifa)
        {
            var rifa = await dbContext.Rifas.FirstOrDefaultAsync(x => x.Id == idRifa);

            if (rifa == null)
            {
                return BadRequest("Rifa id ingresada incorrectamente");
            }

            var participaciones = await dbContext.RifasConParticipantes.Where(x => x.RifaId == idRifa && x.Ganador == false).ToListAsync();

            if (participaciones.Count == 0) {
                return BadRequest("No se encontraron participaciones");
            }


            Random random = new Random();

            var ganador = participaciones.OrderBy(x => random.Next()).Take(1).FirstOrDefault();

            ganador.Ganador = true;

            dbContext.RifasConParticipantes.Update(ganador);
            await dbContext.SaveChangesAsync();

            var datosParticipante = await dbContext.Participantes.Where(x => x.Id == ganador.ParticipanteId).FirstOrDefaultAsync();

            var loteria = new CartasLoteria();
            var list = loteria.mazo.ElementAt(ganador.NumeroLoteria);
            

            var boletoGanador = new
            {
                nombre = datosParticipante.Nombre,
                numero = ganador.NumeroLoteria,
                Carta = loteria.mazo.ElementAt(ganador.NumeroLoteria)
            };

            return boletoGanador;
        }

        [HttpPost("RegistrarParticipacion")]
        public async Task<ActionResult> Post( RifaConParticipanteCreacionDTO rcpDto)
        {
            var mismoNum = await dbContext.RifasConParticipantes.AnyAsync(x => x.NumeroLoteria == rcpDto.NumeroLoteria 
            && x.RifaId == rcpDto.RifaId);

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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.RifasConParticipantes.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El recurso no fue encontrado");
            }

            dbContext.Remove(new RifaConParticipante() { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
