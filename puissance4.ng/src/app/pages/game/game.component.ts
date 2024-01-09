import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { GameService } from '../../services/game.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [ CommonModule],
  templateUrl: './game.component.html',
  styleUrl: './game.component.scss'
})
export class GameComponent implements OnInit {
  //é la partita specifica
  //ho bisogno dopo di mettere a giorno la mia lista e se il secondo utilizzatore clicca sulla partita, vedrà questo component

  currentGame: any|null

  constructor(
    private readonly _store: Store<{game: any}>,
    private readonly _gameServices : GameService,
    private readonly _router : Router
  ) { }

  ngOnInit(): void {
    this._store.select(state => state.game.selectedGame).subscribe(g => {
      this.currentGame = g;
      if(!g){
        alert("Il vost(ro avversario ha abbandonato la partita");
        this._router.navigate(['/']);
      }
    })
  }

  play(){
    this._gameServices.play()
  }
}
