import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../config.service';
import { BaseApiService } from './base.api.service';
import { CurrentUser } from '../models/current-user';

@Injectable()
export class AuthApiService extends BaseApiService {

    constructor(
        private http: Http,
        config: ConfigService
    ) {
        super(config);
    }

    public authenticate(username: string, password: string): Observable<string> {
        let headers = new Headers({
            'Content-Type': 'application/x-www-form-urlencoded'
        });

        let request = {
            grant_type: 'password',
            username: username,
            password: password
        };

        return this.http.post(this.config.serviceUrl + '/oauth/token', this.urlEncode(request), { headers: headers })
            .map(res => <string>res.json().access_token);
    }

    public getCurrentUser(token: string): Observable<CurrentUser> {
        return this.http.get(this.config.serviceUrl + '/api/users/current', { headers: this.getAuthHeaders(token) })
            .map(res => <CurrentUser>res.json())
            .catch(err => this.handleError<CurrentUser>(err));
    }
}
