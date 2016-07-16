import { Component, OnInit } from '@angular/core';

import { LoginService } from '../shared/login.service';
import { ApiService } from '../shared/api.service';

@Component({
    moduleId: module.id,
    selector: 'my-login',
    templateUrl: 'login.component.html',
    providers: [LoginService, ApiService]
})
export class LoginComponent implements OnInit {
    login: string;
    password: string;

    constructor(
        private loginService: LoginService,
        private apiService: ApiService
    ) { }

    ngOnInit() { }

    doLogin() {
        alert(this.login);
    }
}
