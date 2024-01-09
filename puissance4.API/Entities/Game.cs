using System.Drawing;

namespace puissance4.API.Entities
{
    public class Game
    {
        #region Start
        public string RedPlayer { get; set; } = null!;
        public string YellowPlayer { get; set; }= null!;

        //la griglia é 7x6 
        //e puo' avere o no colori --> niente all'inizio
        public Color?[,] Grid { get; set; } = new Color?[7, 6];

        #endregion
        //2 giocatori --> 2 tokens

        public Color Turn { get; set; } = Color.Yellow; //== Color.Yellow --> inizioa il giocatore Yellow
    }


}
