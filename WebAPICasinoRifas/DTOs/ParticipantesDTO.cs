using WebAPICasinoRifas.Entitys;

namespace WebAPICasinoRifas.DTOs
{
    public class ParticipantesDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<RifaConParticipante> Participaciones { get; set; }
    }
}
