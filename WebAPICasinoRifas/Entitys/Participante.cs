namespace WebAPICasinoRifas.Entitys
{
    public class Participante
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<RifaConParticipante> Participaciones { get; set; }

    }
}
