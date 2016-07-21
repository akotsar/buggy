import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { BrokenService } from '../shared/broken.service';
import { PagerComponent } from '../shared/pager/pager.component';
import { MakeDetails } from '../shared/models/make-details';
import { ApiService } from '../shared/api';
import { ShowdownPipe } from '../shared/showdown/showdown.pipe';

@Component({
    selector: 'my-make',
    templateUrl: './make.component.html',
    directives: [ROUTER_DIRECTIVES, PagerComponent],
    pipes: [ShowdownPipe]
})
export class MakeComponent implements OnInit, OnDestroy {
    private _id: number;

    make: MakeDetails;
    page = 1;
    orderby: string;

    constructor(
        private route: ActivatedRoute,
        private api: ApiService,
        private broken: BrokenService
    ) {
        this.broken.breakHomeLink();
    }

    ngOnInit() {
        this.route.params
          .map(params => params['id'])
          .subscribe(id => {
              this._id = id;
              this.updateData();
          });
    }

    ngOnDestroy() {
         this.broken.reset();
     }

    sortBy(field: string) {
        if (!field) {
            return;
        }

        this.orderby = field;
        this.updateData();
    }

    goToPage(page: number) {
        this.page = page;
        this.updateData();
    }

    private updateData() {
        this.api.getMake(this._id, this.page, this.orderby)
          .subscribe(make => this.make = make);
    }
}
