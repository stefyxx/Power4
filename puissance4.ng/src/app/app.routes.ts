import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { isLoggedGuard } from './guards/is-logged.guard';
import { GameComponent } from './pages/game/game.component';

export const routes: Routes = [
    {path:'', redirectTo: 'home', pathMatch:'full'},
    {path:'home', component:HomeComponent, canActivate : [isLoggedGuard]},
    {path:'login', component:LoginComponent},
    {path:'game/:id', component:GameComponent}
    //{path:'**', mi sono sblagiata a scirvere url}
];
