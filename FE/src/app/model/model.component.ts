import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, ROUTER_DIRECTIVES } from '@angular/router';

import { ModelDetails } from '../shared/models/model-details';
import { LoginService } from '../shared/login.service';
import { ApiService } from '../shared/api';
import { ShowdownPipe } from '../shared/showdown/showdown.pipe';

@Component({
    moduleId: module.id,
    selector: 'my-model',
    templateUrl: 'model.component.html',
    directives: [ROUTER_DIRECTIVES],
    pipes: [ShowdownPipe]
})
export class ModelComponent implements OnInit, OnDestroy {
    private loginSub: any;
    private logoutSub: any;

    voting = false;
    id: string;
    model: ModelDetails;
    error: string;

    constructor(
        private route: ActivatedRoute,
        private login: LoginService,
        private api: ApiService
    ) {
     }

    ngOnInit() {
        this.route.params
            .subscribe(params => {
                this.id = params['id'];
                this.refresh();
            });

        this.loginSub = this.login.loggedIn.subscribe(() => this.refresh());
        this.logoutSub = this.login.loggedOut.subscribe(() => this.refresh());
     }

     ngOnDestroy() {
         this.loginSub.unsubscribe();
         this.logoutSub.unsubscribe();
     }

     onVote(comment: string) {
         this.voting = true;
         this.error = '';
         this.api.vote(this.id, comment)
            .finally(() => this.voting = false)
            .subscribe(
                r => this.refresh(),
                err => this.error = err);
     }

     private refresh() {
        if (!this.id) {
            return;
        }

        this.api.getModel(this.id)
            .subscribe(model => this.model = model);
     }
}
