import { Injectable } from '@angular/core';
import { Http, Headers, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { User } from './models/user';
import { Dashboard } from './models/dashboard/dashboard';
import { ModelList } from './models/model-list';
import { ModelDetails } from './models/model-details';
import { MakeDetails } from './models/make-details';
import { RegistrationRequest } from './models/register/registration-request';
import { UserProfile } from './models/user-profile';

@Injectable()
export class ApiService {
  private _serviceUrl = process.env.ENV === 'build' ? 'http://buggyapi.azurewebsites.net' : 'http://localhost:8888';

  constructor(
    private http: Http) {
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

    return this.http.post(this._serviceUrl + '/oauth/token', this.urlEncode(request), { headers: headers })
      .map(res => <string>res.json().access_token);
  }

  public getCurrentUser(token: string): Observable<User> {
    return this.http.get(this._serviceUrl + '/api/users/current', { headers: this.getAuthHeaders(token) })
      .map(res => <User>res.json())
      .catch(err => this.handleError<User>(err));
  }

  public getDashboard(): Observable<Dashboard> {
    return this.http.get(this._serviceUrl + '/api/dashboard')
      .map(res => <Dashboard>res.json());
  }

  public getModels(page?: number, orderBy?: string, makeId?: number): Observable<ModelList> {
    let url = this._serviceUrl + '/api/models?' + this.urlEncode({
      page: page,
      orderBy: orderBy,
      makeId: makeId
    });

    return this.http.get(url)
      .map(res => <ModelList>res.json());
  }

  public getModel(id: number, token?: string): Observable<ModelDetails> {
    return this.http.get(this._serviceUrl + '/api/models/' + id, { headers: this.getAuthHeaders(token) })
      .map(res => <ModelDetails>res.json());
  }

  public getMake(id: number, modelsPage?: number, modelsOrderBy?: string): Observable<MakeDetails> {
    return this.http.get(this._serviceUrl + `/api/makes/${id}?` + this.urlEncode({
        modelsPage: modelsPage,
        modelsOrderBy: modelsOrderBy
      }))
      .map(res => <MakeDetails>res.json());
  }

  public vote(id: number, comment: string, token: string): Observable<any> {
    return this.http.post(this._serviceUrl + `/api/models/${id}/vote`, { comment: comment }, { headers: this.getAuthHeaders(token) })
      .catch(err => this.handleError<boolean>(err));
  }

  public register(req: RegistrationRequest): Observable<boolean> {
    return this.http.post(this._serviceUrl + '/api/users', req)
      .map(res => true)
      .catch(err => this.handleError<boolean>(err));
  }

  public getProfile(token: string): Observable<UserProfile> {
    return this.http.get(this._serviceUrl + '/api/users/profile', { headers: this.getAuthHeaders(token) })
      .map(res => <UserProfile>res.json())
      .catch(err => this.handleError<UserProfile>(err));
  }

  public saveProfile(token: string, profile: UserProfile): Observable<any> {
    return this.http.put(this._serviceUrl + '/api/users/profile', profile, { headers: this.getAuthHeaders(token) })
      .catch(err => this.handleError<any>(err));
  }

  private getAuthHeaders(token?: string): Headers {
    let headers = new Headers();

    if (token !== undefined) {
      headers.append('Authorization', 'Bearer ' + token);
    }

    return headers;
  }

  private urlEncode(obj: Object): string {
      let urlSearchParams = new URLSearchParams();
      for (let key in obj) {
          if (!obj.hasOwnProperty(key) || obj[key] === undefined) {
            continue;
          }

          urlSearchParams.append(key, obj[key]);
      }

      return urlSearchParams.toString();
  }

  private handleError<T>(err: any): Observable<T> {
    if (err.status === 400) {
      let response = err.json();
      let message = '';
      if (response.modelState) {
        for (let field in response.modelState) {
          if (response.modelState.hasOwnProperty(field)) {
            for (let fieldMessage of response.modelState[field]) {
              message = message + (message ? ', ' : '') + fieldMessage;
            }
          }
        }
      }  else {
        message = response.message;
      }

      return Observable.throw(message);
    }

    return Observable.throw('Unknown error');
  }
}
