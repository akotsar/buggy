import { Component, OnInit, OnDestroy } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { ApiService } from '../shared/api';
import { Model } from '../shared/models/model';
import { BrokenService } from '../shared/broken.service';
import { PagerComponent } from '../shared/pager/pager.component';

import './overall.component.scss';

@Component({
    moduleId: module.id,
    selector: 'my-overall',
    templateUrl: 'overall.component.html',
    styleUrls: ['./overall.component.scss'],
    directives: [ROUTER_DIRECTIVES, PagerComponent],
    providers: [ApiService]
})
export class OverallComponent implements OnInit, OnDestroy {
    loading = true;
    page = 1;
    orderby: string;
    models: Model[];
    totalPages: number;

    constructor(
        private api: ApiService,
        private broken: BrokenService
    ) { }

    ngOnInit() {
        this.broken.breakTwitter();
        this.broken.breakLogout();
        this.updateData();
    }

    ngOnDestroy() {
        this.broken.reset();
    }

    sortBy(field: string) {
        this.orderby = field;
        this.updateData();
    }

    goToPage(page: number) {
        this.page = page;
        this.updateData();
    }

    private updateData() {
        this.api.getModels(this.page, this.orderby)
            .subscribe(models => {
                this.models = models.models;
                this.totalPages = models.totalPages;
                this.loading = false;
        });
    }
}
