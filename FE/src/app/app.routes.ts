import { provideRouter, RouterConfig } from '@angular/router';

import { HomeComponent } from './home';
import { AboutComponent } from './about';
import { MakeComponent } from './make';
import { ModelComponent } from './model';
import { OverallComponent } from './overall';

export const routes: RouterConfig = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent},
  { path: 'make/:id', component: MakeComponent},
  { path: 'model/:id', component: ModelComponent},
  { path: 'overall', component: OverallComponent}
];

export const APP_ROUTER_PROVIDERS = [
  provideRouter(routes)
];
