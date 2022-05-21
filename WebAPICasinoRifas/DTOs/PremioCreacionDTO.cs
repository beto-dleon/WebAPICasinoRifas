using System.ComponentModel.DataAnnotations;

namespace WebAPICasinoRifas.DTOs
{
    public class PremioCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }
       
        public bool Disponibilidad { get; set; }

        public int Orden { get; set; }
        [Required]
        public int RifaId { get; set; }
    }
}
