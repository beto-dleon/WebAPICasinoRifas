using System.ComponentModel.DataAnnotations;

namespace WebAPICasinoRifas.DTOs
{
    public class RifaConParticipanteCreacionDTO
    {
       
        [Required]
        public int NumeroLoteria { get; set; }
        public int PremioId { get; set; }
     
    }
}
