import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private readonly _httpClient: HttpClient
  ) { }

  //method login --> TODO considerare il Role
  login(form:{username:string, password: string}){
    return this._httpClient.post<{token: string}>('https://puissance4.azurewebsites.net/api/login', form)
  }
}
