import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'my-admin',
    templateUrl: 'admin.component.html',
    directives: [ROUTER_DIRECTIVES]
})
export class AdminComponent implements OnInit {
    constructor() { }

    ngOnInit() { }
}
