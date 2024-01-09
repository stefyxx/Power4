using Microsoft.Extensions.Diagnostics.HealthChecks;
using puissance4.API.Entities;
using puissance4.API.Models;

namespace puissance4.API.Services
{
    //mia BLL --> tutte le regole da quando creo una partita al vincitore
      //lista partite
      //regole di riempimento
      // ..
    public class GameService
    {
        //lista partite --> lista statica x! no DB
        //{ get; init; } -->init cosi' da fuori NON posso modificarla
        //static --> in tutto il project una sola lista
        //lista riempita in GameHub che implementa i miei servizi
        public static Dictionary<string, Game> Games { get; } = new Dictionary<string, Game>(); 

        //un giocatore puo' creare una partita
        public string Add(string userId, Color color)
        {
            //Controllo: 1 giovcatore puo' creare 1 partita alla volta, la partita deve prima finire --> scomparire dalla lista
            if (Games.Values.Any(g => g.RedPlayer == userId || g.YellowPlayer == userId))
            {
                Console.WriteLine(" Non puoi giocare 2 partite contemporaneamete!");
            }

            Game game = new Game();
            //test grid --> si visualizza?
            //game.Grid[2, 0];

            if(color == Color.Red) game.RedPlayer = userId;
            else game.YellowPlayer = userId;

            //id partita aleatorio --> perché NON ho DB
            string idGame = Guid.NewGuid().ToString();
            Games.Add(idGame, game);
            return idGame;
        }

        public void Join(string userId, string gameId) 
        {
            Game? g = Games.GetValueOrDefault(gameId);

            //la partita non esiste
            if(g is null)
            {
                throw new Exception($"{gameId} is not a game");
            }

            //partita già con 2 giocatori
            if (g.YellowPlayer != null && g.RedPlayer != null)
            {
                throw new Exception("La partita ha già tutti i giocatori");
            }

            //sono già un giocatore --> non posso essere Yelow e Red nella stessa partita
            if (g.YellowPlayer == userId || g.RedPlayer == userId)
            {
                throw new Exception("Non puoi essre Yellow e Red nella stessa partita");
            }

            //prendo il posto libero
            if (g.YellowPlayer == null) g.YellowPlayer = userId;
            else g.RedPlayer = userId;
        }

        public string? FindByPlayer(string userId)
        {
            return Games.Values.FirstOrDefault((g => g.Value.RedPlayer == userId || g.Value.YellowPlayer == userId));
        }
        public void Delete(string gameId)
        {
            Games.Remove(gameId);
        }

        public Game FindById(string gameId) 
        {
            return Games.GetValueOrDefault(gameId);
        }


        //giochiamo la partita --> faccio ANCHE qui' il controllo, cosi' non posso barare
        public (int,int) Play(string userId, string gameId, int col)      //userId --> giocatore corrente
        {
            //color--> puo' essere Yellow OR red OR null


            return (1, 3);
        }

    }
}
