using System.ComponentModel.DataAnnotations;
using WebAPICasinoRifas.Entitys;
using WebAPICasinoRifas.Validaciones;

namespace WebAPICasinoRifas.DTOs
{
    public class ParticipanteCreacionDTO
    {
        [Required]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        
    }
}
