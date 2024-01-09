import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GameService } from '../../services/game.service';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
join() {
throw new Error('Method not implemented.');
}

  message: string = "";
  games: any[] = [];
  id : string = '0';

  constructor(
    private readonly _gameServices: GameService,
    private readonly _store: Store<{ session: any, game: any }>,
    private readonly _router : Router
  ) { }
  ngOnInit(): void {
    //ogni volta che modifico la lista, la modifico quà
    this._store.select(state => state.game.games).subscribe(games => this.games = games);

    //
    this._store.select(state => state.game.selectedGame).subscribe(game => {
      if(game){
        //TODO recuperare id
        this._router.navigate(['/game',this.id]);
      }
    })
  }

  send() {
    this._gameServices.send(this.message);
  }
  createGame() {
    this._gameServices.createGame();
  }

  Join(gameId: string){
    this._gameServices.join(gameId);
  }
  
}
