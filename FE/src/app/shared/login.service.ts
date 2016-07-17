import { provide, Injectable, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ApiService } from './api.service';
import { User } from './models/user';

@Injectable()
export class LoginService {
    private static instance: LoginService = null;

    @Output() loggedIn = new EventEmitter();
    @Output() loggedOut = new EventEmitter();
    user: User;

    public static getInstance(api: ApiService): LoginService {
        if (LoginService.instance === null) {
            LoginService.instance = new LoginService(api);
        }

        return LoginService.instance;
    }

    constructor(
        private api: ApiService
    ) {
        let token = localStorage.getItem('token');
        if (token) {
            this.updateUser(token).subscribe(u => {}, e => {});
        }
    }

    public getToken(): string {
        return localStorage.getItem('token');
    }

    public getIsLoggedIn(): boolean {
        return this.user != null;
    }

    public login(username: string, password: string): Observable<User> {
        return this.api.authenticate(username, password)
            .flatMap(token => this.updateUser(token));
    }

    public logout() {
        this.user = null;
        this.clearToken();
        this.loggedOut.emit({});
    }

    private updateUser(token: string): Observable<User> {
        return this.api.getCurrentUser(token)
            .map(user => {
                this.setToken(token);
                this.user = user;
                this.loggedIn.emit(user);
                return user;
            })
            .catch(err => {
                this.logout();
                return Observable.throw('invalid token');
            });
    }

    private setToken(token: string) {
        localStorage.setItem('token', token);
    }

    private clearToken() {
        localStorage.removeItem('token');
    }
}

export const LOGIN_SERVICE_PROVIDER = [
  provide(LoginService, {
    deps: [ApiService],
    useFactory: (api: ApiService): LoginService => {
      return LoginService.getInstance(api);
    }
  })
];
