import { createAction, createReducer, on, props } from "@ngrx/store";

//Actions
export const refreshGames = createAction('games/refresh', props<{games : any}>()); //any[]
export const selectGame = createAction('games/select', props<{game : any}>());
export const leaveGame = createAction('games/leave');

//Reducers --> imported into app.config.ts
export const gameReducers = createReducer(
    {games:[], selectedGame: null}, //as {games: any[], selectedGame: any | null},
    on(refreshGames,(state,payload) =>{
        return {...state, games:payload.games}
    }),
    // on(refreshGames,(state : any, payload : any) => ({...state, ...payload})),
    on(selectGame,(state,payload) =>{
        return {...state, selectedGame:payload.game};
    }),
    on(leaveGame, (state)=>{
        return {...state, selectedGame: null};
    })
)