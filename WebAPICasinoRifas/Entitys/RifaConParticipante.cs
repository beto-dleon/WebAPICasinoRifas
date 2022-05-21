namespace WebAPICasinoRifas.Entitys
{
    public class RifaConParticipante
    {
        public int Id { get; set; }
        public int ParticipanteId { get; set; }
        public int RifaId { get; set; }
        public int NumeroLoteria { get; set; }
        public int PremioId { get; set; }
        public bool Ganador { get; set; }
        public Participante Participantes { get; set; }
    }
}
