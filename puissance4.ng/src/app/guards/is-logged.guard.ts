import { isNgContent } from '@angular/compiler';
import { Inject, inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, map, tap } from 'rxjs';

export const isLoggedGuard: CanActivateFn = (route, state ) => {
  const store = inject(Store<{session: {token:string}}>);
  //const token : store.select(state=> state.token); --> ma deve essere Observable
  const token$ : Observable<string | null> =  store.select(state=> state.sesion.token);

  const router = inject(Router);


  //return true; // entro nella page 
  return token$.pipe(
    map(t=>!!t),    //t=>!!t --> trasformo il token, string in bool
    tap(isConnected =>{
      if(!isConnected) router.navigate(['login'])
    })
    ); 
  
};
