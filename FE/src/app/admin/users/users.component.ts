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
        this.api.lockUser(username)
            .subscribe(() => this.refresh(), err => this.error = err);
    }

    unlock(username: string) {
        this.api.unlockUser(username)
            .subscribe(() => this.refresh(), err => this.error = err);
    }

    private refresh() {
        this.api.getUsers().subscribe(u => this.users = u);
    }
}
