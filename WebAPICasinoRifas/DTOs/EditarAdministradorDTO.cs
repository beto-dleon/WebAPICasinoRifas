using System.ComponentModel.DataAnnotations;

namespace WebAPICasinoRifas.DTOs
{
    public class EditarAdministradorDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
