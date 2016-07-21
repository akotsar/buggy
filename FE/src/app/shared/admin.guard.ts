import { Injectable }   from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LoginService }	from './login.service';

@Injectable()
export class AdminGuard implements CanActivate {
    constructor(
        private login: LoginService,
        private router: Router) {
    }

    canActivate() {
        if (this.login.getIsLoggedIn() && this.login.user.isAdmin) {
            return true;
        }

        this.router.navigate(['/']);

        return false;
    }
}
