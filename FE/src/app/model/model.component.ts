import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'my-model',
    templateUrl: 'model.component.html'
})
export class ModelComponent implements OnInit {
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
