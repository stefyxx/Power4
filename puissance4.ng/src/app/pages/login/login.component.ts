import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Route, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { sessionStart } from '../../store/session.state';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

  fg:FormGroup

  constructor(
    private readonly _fb: FormBuilder, 
    private readonly _authService: AuthService,
    private readonly _router: Router,
    private readonly _store: Store) {} // store é il dispatcher

  ngOnInit(): void {
    this.fg = this._fb.group({
      username: [null, [Validators.required]],
      password: [null, [Validators.required]],
    });
  }

  login(){
    //usernm et psw esistono?
    if(this.fg.invalid){return ;}

    //questo é un Observable; lui puo' donare una o più risposte (quante volte cambia) o errore
    //ma puo' essere cloturato
    this._authService.login(this.fg.value).subscribe({
      next: data =>{
        //in caso di successo --> redirection verso Home page ,navigate(['/'])
        this._router.navigate(['/']),
        this._store.dispatch(sessionStart(
          {
            token: data.token,
            username : (<any>jwtDecode(data.token))['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
          }
        ))      //API reinvia il token --> in project API in method 'Login' return token and USERNAME 
                //(or lo recupero dal token) npm i jwt-decode
                //(<any>jwtDecode(data.token)) --> casting == (jwtDecode(data.token) as any)
      },
      error : err =>{
        //in caso di errore
        alert('Impossible to connect!')
      },
      /*complete: ()=>{
        //in caso in cui l'Observable é cloturato == ha finito di emettere
      }*/
    })
  }

}
