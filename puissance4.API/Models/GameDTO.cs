namespace puissance4.API.Models
{
    //per visualizzare le partite "giocabili o giocandi"
    public class GameDTO
    {
        public string GameId { get; set; }
        public string RedPlayer { get; set; }
        public string YellowPlayer { get; set; }
    }
}
