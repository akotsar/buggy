import { Injectable } from '@angular/core';

@Injectable()
export class LoginService {
  public isLoggedIn = false;
  public userName: string;

  constructor() {
      this.userName = <string>localStorage.getItem('username');
      this.isLoggedIn = !!this.userName;
  }
}
