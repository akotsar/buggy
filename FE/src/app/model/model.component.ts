import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, ROUTER_DIRECTIVES } from '@angular/router';

import { ModelDetails } from '../shared/models/model-details';
import { BrokenService } from '../shared/broken.service';
import { LoginService } from '../shared/login.service';
import { ApiService } from '../shared/api.service';
import { ShowdownPipe } from '../shared/showdown/showdown.pipe';

@Component({
    moduleId: module.id,
    selector: 'my-model',
    templateUrl: 'model.component.html',
    directives: [ROUTER_DIRECTIVES],
    pipes: [ShowdownPipe]
})
export class ModelComponent implements OnInit, OnDestroy {
    private idSub: any;
    private loginSub: any;
    private logoutSub: any;

    voting = false;
    id: number;
    model: ModelDetails;
    error: string;

    constructor(
        private route: ActivatedRoute,
        private broken: BrokenService,
        private login: LoginService,
        private api: ApiService
    ) {
        this.broken.breakHomeLink();
     }

    ngOnInit() {
        this.idSub = this.route.params
            .subscribe(params => {
                this.id = params['id'];
                this.refresh();
            });

        this.loginSub = this.login.loggedIn.subscribe(() => this.refresh());
        this.logoutSub = this.login.loggedOut.subscribe(() => this.refresh());
     }

     ngOnDestroy() {
         this.idSub.unsubscribe();
         this.loginSub.unsubscribe();
         this.logoutSub.unsubscribe();
         this.broken.reset();
     }

     onVote(comment: string) {
         this.voting = true;
         this.error = '';
         this.api.vote(this.id, comment, this.login.getToken())
            .finally(() => this.voting = false)
            .subscribe(
                r => this.refresh(),
                err => this.error = err);
     }

     private refresh() {
        if (!this.id) {
            return;
        }

        this.api.getModel(this.id, this.login.getToken())
            .subscribe(model => this.model = model);
     }
}
