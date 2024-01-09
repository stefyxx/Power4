namespace puissance4.API.Models
{
    public enum Color
    {
        Yellow,
        Red
    }
    public class CreateGameDTO
    {
        //quando m'inscrivo a una partita, scelgo di che colore voglio essere
        public Color SelectedColor { get; set; }

    }
}
