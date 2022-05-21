using System.ComponentModel.DataAnnotations;
using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas.DTOs
{
    public class ParticipanteCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }
        
    }
}
