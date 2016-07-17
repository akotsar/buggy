import { Component, OnInit, OnDestroy } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { ApiService } from '../shared/api.service';
import { Model } from '../shared/models/model';
import { BrokenService } from '../shared/broken.service';

import './overall.component.scss';

@Component({
    moduleId: module.id,
    selector: 'my-overall',
    templateUrl: 'overall.component.html',
    styleUrls: ['./overall.component.scss'],
    directives: [ROUTER_DIRECTIVES],
    providers: [ApiService]
})
export class OverallComponent implements OnInit, OnDestroy {
    loading = true;
    page = 1;
    orderby: string;
    models: Model[];
    totalPages: number;
    enteredPage: number;

    constructor(
        private api: ApiService,
        private broken: BrokenService
    ) { }

    ngOnInit() {
        this.broken.breakTwitter();
        this.updateData();
    }

    ngOnDestroy() {
        this.broken.reset();
    }

    sortBy(field: string) {
        this.orderby = field;
        this.updateData();
    }

    prevPage() {
        this.page--;
        this.updateData();
    }

    nextPage() {
        this.page++;
        this.updateData();
    }

    pageNumberPress(ev: any) {
        if (ev.keyCode === 13 && !isNaN(parseFloat(<any>this.enteredPage))) {
            // Intentional bug:
            if (this.enteredPage === 1 || this.enteredPage === this.totalPages) {
                return;
            }

            if (this.enteredPage > this.totalPages) {
                this.enteredPage = this.totalPages;
            }

            this.page = this.enteredPage;
            this.updateData();
        }
    }

    private updateData() {
        if (this.page <= 1) {
             this.page = 1;
        }

        this.enteredPage = this.page;
        this.api.getModels(this.page, this.orderby)
            .subscribe(models => {
                this.models = models.models;
                this.totalPages = models.totalPages;
                this.loading = false;
        });
    }
}
