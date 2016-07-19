import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { ApiService } from './shared';
import { LoginComponent } from './login';
import { BrokenService } from './shared/broken.service';

import '../../node_modules/bootstrap/dist/css/bootstrap.css';
import '../style/app.scss';

/*
 * App Component
 * Top Level Component
 */
@Component({
  selector: 'my-app', // <my-app></my-app>
  providers: [ApiService, BrokenService],
  directives: [...ROUTER_DIRECTIVES, LoginComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {

  constructor(
    private broken: BrokenService
  ) {
  }
}
