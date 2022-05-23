using System.ComponentModel.DataAnnotations;

namespace WebAPICasinoRifas.DTOs
{
    public class PremioPatchDTO
    {
        [Required]
        public string Nombre { get; set; }

        public bool Disponibilidad { get; set; }

        
    }
}
