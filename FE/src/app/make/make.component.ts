import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'my-make',
    templateUrl: './make.component.html'
})
export class MakeComponent implements OnInit {
    private _id: string;

    constructor(
        private route: ActivatedRoute
    ) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['id'] !== undefined) {
                this._id = params['id'];
            }
        });
    }

}
