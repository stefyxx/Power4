import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { sessionReducers } from './store/session.state';
import { gameReducers } from './store/game.state';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
    provideHttpClient(), provideStore(),
    provideStore({session: sessionReducers, gameReducers})]
};
