import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { Dashboard } from '../shared/models/dashboard/dashboard';
import { ApiService } from '../shared/api.service';

@Component({
  selector: 'my-home',
  directives: [ROUTER_DIRECTIVES],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [ApiService]
})
export class HomeComponent implements OnInit {
  loading = true;
  dashboard: Dashboard;

  constructor(
    private api: ApiService
  ) {
    // Do stuff
  }

  ngOnInit() {
    this.api.getDashboard()
      .subscribe(d => {
        this.dashboard = d;
        this.loading = false;
      });
  }
}
