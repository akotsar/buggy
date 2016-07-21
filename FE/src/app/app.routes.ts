import { provideRouter, RouterConfig } from '@angular/router';

import { AuthGuard } from './shared/auth.guard';

import { HomeComponent } from './home';
import { MakeComponent } from './make';
import { ModelComponent } from './model';
import { OverallComponent } from './overall';
import { RegisterComponent } from './register';
import { ProfileComponent } from './profile';
import { AdminComponent, UsersComponent } from './admin';

export const routes: RouterConfig = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'make/:id', component: MakeComponent },
  { path: 'model/:id', component: ModelComponent },
  { path: 'overall', component: OverallComponent },
  { path: 'admin', component: AdminComponent, children: [
    { path: '', redirectTo: 'users' },
    { path: 'users', component: UsersComponent }
  ] },
];

export const APP_ROUTER_PROVIDERS = [
  provideRouter(routes),
  AuthGuard
];
