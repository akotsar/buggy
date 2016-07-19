import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ROUTER_DIRECTIVES } from '@angular/router';

import { PagerComponent } from '../shared/pager/pager.component';
import { MakeDetails } from '../shared/models/make-details';
import { ApiService } from '../shared/api.service';

@Component({
    selector: 'my-make',
    templateUrl: './make.component.html',
    directives: [ROUTER_DIRECTIVES, PagerComponent]
})
export class MakeComponent implements OnInit {
    private _id: number;

    make: MakeDetails;
    page = 1;
    orderby: string;

    constructor(
        private route: ActivatedRoute,
        private api: ApiService
    ) { }

    ngOnInit() {
        this.route.params
          .map(params => params['id'])
          .subscribe(id => {
              this._id = id;
              this.updateData();
          });
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
        this.api.getMake(this._id, this.page, this.orderby)
          .subscribe(make => this.make = make);
    }
}
