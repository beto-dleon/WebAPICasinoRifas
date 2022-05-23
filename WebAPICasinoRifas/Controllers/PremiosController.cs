using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPost("CrearPremio")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        public async Task<ActionResult> Post(PremioCreacionDTO premioDto)
        {
            var rifa = await dbContext.Rifas.FirstOrDefaultAsync(rifaBD => rifaBD.Id == premioDto.RifaId);
            if (rifa == null)
            {
                return BadRequest("Esta rifa no se encuentra");
            }
            var existePremioMismoNom = await dbContext.Premios.AnyAsync(x => x.Nombre == premioDto.Nombre);

            if(existePremioMismoNom)
            {
                return BadRequest($"Ya existe un premio con el nombre: {premioDto.Nombre}");
            }
            
            var premio = mapper.Map<Premio>(premioDto);
            premio.Rifa = rifa;
            dbContext.Add(premio);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("EditarPremio")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        public async Task<ActionResult> Put(int id, PremioCreacionDTO premioCreacionDTO)
        {
            var premioDB = await dbContext.Premios.FirstOrDefaultAsync(x => x.Id == id);

            if(premioDB == null)
            {
                return NotFound();
            }

            premioDB = mapper.Map(premioCreacionDTO, premioDB);

            await dbContext.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PremioPatchDTO> patchDocument)
        {
            if( patchDocument == null)
            {
                return BadRequest();
            }
            var premioDb = await dbContext.Premios.FirstOrDefaultAsync(x => x.Id == id);

            if(premioDb == null)
            {
                return NotFound();
            }

            var premioDTO = mapper.Map<PremioPatchDTO>(premioDb);

            patchDocument.ApplyTo(premioDTO, ModelState);

            var esValido = TryValidateModel(premioDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(premioDTO, premioDb);

            await dbContext.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Premios.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El recurso no fue encontrado");
            }

            dbContext.Remove(new Premio { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
