import { Component, OnInit } from '@angular/core';

import { ApiService } from '../../shared/api';
import { User } from '../../shared/models/admin/user';

@Component({
    moduleId: module.id,
    selector: 'my-users',
    templateUrl: 'users.component.html'
})
export class UsersComponent implements OnInit {
    users: Array<User>;
    error: string;

    constructor(
        private api: ApiService
    ) { }

    ngOnInit() {
        this.refresh();
    }

    lock(username: string) {
        this.error = null;

        this.api.lockUser(username)
            .subscribe(() => this.refresh(), err => this.error = err);
    }

    unlock(username: string) {
        this.error = null;

        this.api.unlockUser(username)
            .subscribe(() => this.refresh(), err => this.error = err);
    }

    resetPassword(user: User, newPassword: string) {
        (<any>user).sendingPassword = true;
        this.error = null;

        this.api.resetPassword(user.username, newPassword)
            .finally(() => (<any>user).sendingPassword = false)
            .subscribe(() => this.refresh(), err => this.error = err);
    }

    deleteUser(user: User) {
        (<any>user).sendingDelete = true;
        this.error = null;

        this.api.deleteUser(user.username)
            .finally(() => (<any>user).sendingDelete = false)
            .subscribe(() => this.refresh(), err => this.error = err);
    }

    private refresh() {
        this.error = null;
        this.api.getUsers().subscribe(u => this.users = u);
    }
}
