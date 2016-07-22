import { Injectable } from '@angular/core';
import { Http, Response, RequestOptionsArgs } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../config.service';
import { BaseApiService } from './base.api.service';
import { LoginService } from '../login.service';
import { Dashboard } from '../models/dashboard/dashboard';
import { ModelList } from '../models/model-list';
import { ModelDetails } from '../models/model-details';
import { MakeDetails } from '../models/make-details';
import { RegistrationRequest } from '../models/register/registration-request';
import { UserProfile } from '../models/user-profile';
import { User } from '../models/admin/user';

@Injectable()
export class ApiService extends BaseApiService {
    constructor(
        private http: Http,
        private login: LoginService,
        config: ConfigService) {
        super(config);
    }

    public getDashboard(): Observable<Dashboard> {
        return this.get('/api/dashboard')
            .map(res => <Dashboard>res.json());
    }

    public getModels(page?: number, orderBy?: string, makeId?: number): Observable<ModelList> {
      let url = '/api/models?' + this.urlEncode({
          page: page,
          orderBy: orderBy,
          makeId: makeId
      });

      return this.get(url)
          .map(res => <ModelList>res.json());
    }

    public getModel(id: number): Observable<ModelDetails> {
        return this.get('/api/models/' + id)
            .map(res => <ModelDetails>res.json());
    }

    public getMake(id: number, modelsPage?: number, modelsOrderBy?: string): Observable<MakeDetails> {
        return this.get(`/api/makes/${id}?` + this.urlEncode({
            modelsPage: modelsPage,
            modelsOrderBy: modelsOrderBy
        }))
        .map(res => <MakeDetails>res.json());
    }

    public vote(id: number, comment: string): Observable<any> {
        return this.post(`/api/models/${id}/vote`, { comment: comment });
    }

    public register(req: RegistrationRequest): Observable<boolean> {
        return this.post('/api/users', req)
            .map(res => true);
    }

    public getProfile(): Observable<UserProfile> {
        return this.get('/api/users/profile')
            .map(res => <UserProfile>res.json());
    }

    public saveProfile(profile: UserProfile): Observable<any> {
        return this.put('/api/users/profile', profile);
    }

    public getUsers(): Observable<Array<User>> {
        return this.get('/api/admin/users')
            .map(res => <Array<User>>res.json());
    }

    public lockUser(username: string): Observable<any> {
        return this.put('/api/admin/users/' + encodeURIComponent(username) + '/lock');
    }

    public unlockUser(username: string): Observable<any> {
        return this.put('/api/admin/users/' + encodeURIComponent(username) + '/unlock');
    }

    public resetPassword(username: string, password: string): Observable<any> {
        return this.put('/api/admin/users/' + encodeURIComponent(username) + '/password', password);
    }

    public deleteUser(username: string): Observable<any> {
        return this.delete('/api/admin/users/' + encodeURIComponent(username));
    }

    private get(url: string, options?: RequestOptionsArgs): Observable<Response> {
        options = options || {};
        options.headers = this.getAuthHeaders(this.login.getToken());

        return this.http.get(this.config.serviceUrl + url, options)
            .catch(err => this.handleError<any>(err));
    }

    private post(url: string, body?: any, options?: RequestOptionsArgs): Observable<Response> {
        options = options || {};
        options.headers = this.getAuthHeaders(this.login.getToken());
        options.headers.append('Content-Type', 'application/json');

        return this.http.post(this.config.serviceUrl + url, JSON.stringify(body), options)
            .catch(err => this.handleError<any>(err));
    }

    private put(url: string, body?: any, options?: RequestOptionsArgs): Observable<Response> {
        options = options || {};
        options.headers = this.getAuthHeaders(this.login.getToken());
        options.headers.append('Content-Type', 'application/json');

        return this.http.put(this.config.serviceUrl + url, JSON.stringify(body), options)
            .catch(err => this.handleError<any>(err));
    }

    private delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
        options = options || {};
        options.headers = this.getAuthHeaders(this.login.getToken());

        return this.http.delete(this.config.serviceUrl + url, options)
            .catch(err => this.handleError<any>(err));
    }
}
