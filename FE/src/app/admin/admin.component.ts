import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { LoginService } from '../shared/login.service';

@Component({
    moduleId: module.id,
    selector: 'my-admin',
    templateUrl: 'admin.component.html',
    directives: [ROUTER_DIRECTIVES]
})
export class AdminComponent implements OnInit, OnDestroy {
    logoutSub: any;

    constructor(
        private login: LoginService,
        private router: Router
    ) { }

    ngOnInit() {
        this.logoutSub = this.login.loggedOut.subscribe(() => {
            this.router.navigate(['/']);
        });
    }

    ngOnDestroy() {
        this.logoutSub.unsubscribe();
    }
}
