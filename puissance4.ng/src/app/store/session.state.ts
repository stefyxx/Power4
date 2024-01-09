import { createAction, createReducer, on, props } from "@ngrx/store";

//state --> contiene una parte di datas che voglio salvare nell'applicaz
//Sarà composto da 2 parti : Actions e Reduces

//Actions
//sessionStart -->cosa mi serve per aprire una sessione? Mi serve username e token
export const sessionStart = createAction('/session/start', props<{username:string, token:string}>())
//sessionStop --> NON voglio salvare username e password
export const sessionStop = createAction('/session/stop')


//Reduces
export const sessionReducers = createReducer(
    {username:null, token:null},  //initial state
    on(sessionStart, (state : any, payload) => ({...state, ...payload})),    // rimpiazzo nei vecchi datas di 'state' con i nuovi di payload --> aggiungo allo 'state' l'username e token del loggato
    on(sessionStop,()=>({username:null, token:null})),     //return {username:null, token:null}
)