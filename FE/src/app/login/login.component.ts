import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { LoginService } from '../shared/login.service';

import './login.component.scss';

@Component({
    moduleId: module.id,
    selector: 'my-login',
    templateUrl: 'login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [LoginService],
    directives: [ROUTER_DIRECTIVES]
})
export class LoginComponent implements OnInit {
    login: string;
    password: string;
    isInvalidLogin = false;
    isLoggingIn = false;

    constructor(
        private loginService: LoginService
    ) { }

    ngOnInit() { }

    doLogin() {
        this.isInvalidLogin = false;
        this.isLoggingIn = true;

        this.loginService.login(this.login, this.password)
            .finally(() => this.isLoggingIn = false)
            .subscribe(
                user => { },
                err => {
                    this.isInvalidLogin = true;
                });
    }

    doLogout() {
        this.loginService.logout();
    }
}
