import { state } from '@angular/animations';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Store } from '@ngrx/store';
import { leaveGame, refreshGames, selectGame } from '../store/game.state';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  connection!: HubConnection

  //il ctor recupera il mio Store x potergli far memorizzare il mio token --> token resta into Context
  constructor(private readonly store: Store<{ session: any }>) {
    //ogni sessione ha token --> no connesso == token = null
    store.select(state => state.session.token).subscribe(tkn => {
      //se token == null --> non sono connesso e CHIUDO la sessione
      if (!tkn) {
        this.connection!.stop();
      }
      else {
        //sono connesso
        this.connection = new HubConnectionBuilder()
          .withUrl('https://puissance4.azurewebsites.net/ws/game',
            { accessTokenFactory: () => tkn })    //accessTokenFactory--> Recupero il mio token x poterlo reinviare al ASP.Net prj
          .withAutomaticReconnect()
          .build()
      }
    })
    this.connection.on('OnMessage', console.log);
    this.connection.on('OnError', (er) => alert(er));
    //voglio la lista dei games
    this.connection.on('OnGames', games => store.dispatch(refreshGames({games}))); //partita memorizzata nello store ( invece che in un Signal)
    this.connection.on('OnGames', game => store.dispatch(selectGame({game})) ); //partita 
    this.connection.on('Leave', ()=> store.dispatch(leaveGame()));

  }
  send(message: string) {
    this.connection.send('SayHello', message);
  }

  //enum into ASP.Net --> valori x default: Yellow = 0; Red = 1 (x! é un enum!)
  //x ora chi crea la partita é Yellow

  //'CreateGame' method into 'GameHub' di Asp.net API
  createGame(){
    this.connection.send('CreateGame', {color : 0});
  }

  join(gameId: string){
    this.connection.send('Join', gameId);
  }

  play(obj : any){
    //obj --> che ha gameId e la colonna, col 

  }
}
