using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using puissance4.API.Entities;
using puissance4.API.Models;
using puissance4.API.Services;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace puissance4.API.Hubs
{
    //la mia partita
    public class GameHub(GameService _gameServices) : Hub
    {
        //tutti possono vederla
        //posso vedere tutti i clients appartenenti a questo Hub e posso inviarli un sms
        public void SayHello(string sms)
        {
            Clients.Others.SendAsync("OnMessage", sms); //test per vedere se 'chi non é nel gruppo mi 'sente'
            //Clients.Group("").SendAsync(""); //sms invito SOLO al gruppo in cui sei connesso
        }

        //x m'identificare --> cosi' sono autorizzato a creare una partita
        [Authorize]
        public void CreateGame(CreateGameDTO dto)
        {
            try
            {
                //Angular prj recupera il token --> salvaguardato nel Context

                //recupero User connesso
                //Add l'aggiunge nel Games
                string idGame = _gameServices.Add(ConnectedUser, dto.SelectedColor);

                //il giocatore farà parte di un gruppo; e tutti quelli del gruppo devono poter veder la partita creata:

                //x ogni token == user connesso; la Connessione crea un ConnectionId -->
                //ogni user connesso avrà un Context.ConnectionId
                Groups.AddToGroupAsync(Context.ConnectionId, idGame);

                //!!!!!!!!!!!!!!!!!!!!!!!!! IMPORTANT
                //tutti i connessi vedono le partite disponibili ET 'SendAsync' ANCHE PUSHA nella list la nupova partita
                Clients.All.SendAsync("OnGames", GameService.Games.Select((kvp) =>
                new GameDTO
                {
                    GameId = kvp.Key,
                    YellowPlayer = kvp.Value.YellowPlayer,
                    RedPlayer = kvp.Value.RedPlayer,
                }
                )); //sms per tutti i giocatori connessi
                Clients.Caller.SendAsync("OnGame", _gameServices.FindById(idGame)); //a chi ha creato la partita diciamo che ha un avversario

            }
            catch (Exception ex)
            {
                //user non loggato OR partita già piena
                Clients.Caller.SendAsync("OnError", ex.Message);

            }
        }

        //recupero User connesso
        private string ConnectedUser
        {
            get => Context.User.FindFirst(ClaimTypes.Name).Value;
        }

        //parlo al mio gruppo di partita --> 2 giocatori

        //mostro le partite create al connesso
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnGames", GameService.Games.Select((kvp) =>
                 new GameDTO
                 {
                     GameId = kvp.Key,
                     YellowPlayer = kvp.Value.YellowPlayer,
                     RedPlayer = kvp.Value.RedPlayer,
                 }
                 ));
            return base.OnConnectedAsync();
            //return Task.CompletedTask;

        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //il giocatore fa parte di una partita?
            string? gameId = _gameServices.FindByPlayer(ConnectedUser);
            if (gameId is not null)
            {
                Clients.Group(gameId).SendAsync("Leave");
                _gameServices.Delete(gameId);
                Clients.All.SendAsync("OnGames", GameService.Games.Select((kvp) =>
                new GameDTO
                {
                    GameId = kvp.Key,
                    YellowPlayer = kvp.Value.YellowPlayer,
                    RedPlayer = kvp.Value.RedPlayer,
                }
                ));
            }

            return base.OnDisconnectedAsync(exception);
        }

        public void Join(string gameId)
        {
            try
            {
                _gameServices.Join(ConnectedUser, gameId);
                Clients.All.SendAsync("OnGames", GameService.Games.Select((kvp) =>
                new GameDTO
                {
                    GameId = kvp.Key,
                    YellowPlayer = kvp.Value.YellowPlayer,
                    RedPlayer = kvp.Value.RedPlayer,
                }
                ));
                //aggiungo il secondo giocatore al gruppo
                Groups.AddToGroupAsync(ConnectedUser, gameId);
                Clients.Group(gameId).SendAsync("OnGame", _gameServices.FindById(gameId));

            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("OnError", ex.Message);
            }
        }

        public void Play(PlayDTO dto)
        {
            try 
            { 
                //metto a giorno la Game x tutto il gruppo della game (i 2 giocatori)
            }
            catch
            {

            }
        }
    }
}
//posso usarlo x semplificare il code in alto
public static class ClientProxyEstenction
{
    public static void BroadCastGame(this IClientProxy clients)
    {
        //Dictionary<string, Game> games
        clients.SendAsync("OnGames", GameService.Games.Select((kvp) =>
            new GameDTO
            {
                GameId = kvp.Key,
                YellowPlayer = kvp.Value.YellowPlayer,
                RedPlayer = kvp.Value.RedPlayer,
            }
            ));
    }
}
