import { enableProdMode } from '@angular/core';
import { bootstrap } from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';
import { disableDeprecatedForms, provideForms } from '@angular/forms';

import { AppComponent } from './app/app.component';
import { APP_ROUTER_PROVIDERS } from './app/app.routes';
import { ConfigService } from './app/shared';
import { AuthApiService } from './app/shared/api';
import { LOGIN_SERVICE_PROVIDER } from './app/shared/login.service';

// depending on the env mode, enable prod mode or add debugging modules
if (process.env.ENV === 'build') {
  enableProdMode();
}

bootstrap(AppComponent, [
    // These are dependencies of our App
    HTTP_PROVIDERS,
    APP_ROUTER_PROVIDERS,
    disableDeprecatedForms(),
    provideForms(),
    ConfigService,
    AuthApiService,
    LOGIN_SERVICE_PROVIDER
  ])
  .catch(err => console.error(err));
