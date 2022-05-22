using System.ComponentModel.DataAnnotations;

namespace WebAPICasinoRifas.DTOs
{
    public class RifaConParticipanteCreacionDTO
    {
       
        [Required]
        [Range(1,54)]
        public int NumeroLoteria { get; set; }

        public int ParticipanteId { get; set; }
        public int RifaId { get; set; }
        public int? PremioId { get; set; }

        public bool? Ganador { get; set; }

    }
}
