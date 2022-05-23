using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPICasinoRifas.DTOs;
using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas.Controllers
{
    [ApiController]
    [Route("api/participantes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParticipantesController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ParticipantesController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpPost("CrearParticipante")]
        public async Task<ActionResult> Post(ParticipanteCreacionDTO partisDTO)
        {
            var parti = mapper.Map<Participante>(partisDTO);

            dbContext.Add(parti);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

    }
}
