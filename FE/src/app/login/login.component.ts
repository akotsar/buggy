import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { FORM_DIRECTIVES } from '@angular/forms';

import { LoginService } from '../shared/login.service';
import { BrokenService } from '../shared/broken.service';

import './login.component.scss';

@Component({
    moduleId: module.id,
    selector: 'my-login',
    templateUrl: 'login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [],
    directives: [ROUTER_DIRECTIVES, FORM_DIRECTIVES]
})
export class LoginComponent implements OnInit {
    isInvalidLogin = false;
    isLoggingIn = false;

    constructor(
        private loginService: LoginService,
        private brokenService: BrokenService
    ) { }

    ngOnInit() {
    }

    doLogin(creds: any) {
        this.isInvalidLogin = false;
        this.isLoggingIn = true;

        this.loginService.login(creds.login, creds.password)
            .finally(() => this.isLoggingIn = false)
            .subscribe(
                user => { },
                err => {
                    this.isInvalidLogin = true;
                });
    }

    doLogout() {
        if (this.brokenService.isLogoutBroken()) {
            return;
        }

        this.loginService.logout();
    }
}
