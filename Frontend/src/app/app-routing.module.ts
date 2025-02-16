import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

const routes: Routes = [
    { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
    { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) },
    { path: 'users', loadChildren: () => import('./users/users.module').then(m => m.UsersModule), canActivate: [authGuard] },
    { path: 'admin', loadChildren: () => import('./users/users.module').then(m => m.UsersModule), canActivate: [adminGuard] },
    { path: '**', redirectTo: 'auth/login' }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
