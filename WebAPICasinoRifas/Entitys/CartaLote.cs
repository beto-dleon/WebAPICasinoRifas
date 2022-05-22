namespace WebAPICasinoRifas.Entitys
{
    public class CartaLote
    {
        int Id { get; set; }
        string nombre { get; set; }

        public CartaLote(int id, string nombre)
        {
            this.Id = id;
            this.nombre = nombre;
        }
    }
}
