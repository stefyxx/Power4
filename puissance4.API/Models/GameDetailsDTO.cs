using puissance4.API.Entities;
using System.Drawing;

namespace puissance4.API.Models
{
    public class GameDetailsDTO : GameDTO
    {
        //invece della matrive, ho ,una lista di liste
        public List<List<Color?>> Grid { get; set; }

        public GameDetailsDTO(Game game)
        {
            YellowPlayer = game.YellowPlayer;
            RedPlayer = game.RedPlayer;

            Grid = new List<List<Color?>>();
            for (int x = 0; x < game.Grid.GetLength(0); x++)
            {
                Grid.Add(new List<Color?>());
                for (int y = 0; y < game.Grid.GetLength(1); y++)
                {
                    Grid[x].Add(game.Grid[x, y]);
                }
            }

        }
    }
}
