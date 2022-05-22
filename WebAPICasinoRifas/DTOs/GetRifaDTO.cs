using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas.DTOs
{
    public class GetRifaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Disponibilidad { get; set; }
        public List<RifaConParticipante> Participaciones { get; set; }
        public List<Premio> Premios { get; set; }
    }
}
