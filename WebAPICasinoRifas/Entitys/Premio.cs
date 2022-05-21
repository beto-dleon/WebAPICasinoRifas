namespace WebAPICasinoRifas.Entitys
{
    public class Premio
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public bool Disponibilidad { get; set; }
        public int Orden { get; set; }
        public int  RifaId { get; set; }
        public Rifa Rifa { get; set; }
        
    }
}
