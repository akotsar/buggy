import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';

import './pager.component.scss';

@Component({
    moduleId: module.id,
    selector: 'my-pager',
    templateUrl: 'pager.component.html',
    styleUrls: ['pager.component.scss']
})
export class PagerComponent implements OnInit, OnChanges {
    @Input() page: number;
    @Input() totalPages: number;
    @Input() buggy = false;
    @Output() goToPage: EventEmitter<number>;
    enteredPage: any;

    constructor() {
        this.goToPage = new EventEmitter<any>();
    }

    ngOnInit() {
        this.enteredPage = this.page;
    }

    ngOnChanges(changes: SimpleChanges) {
        this.resetEnteredPage();
    }

    prevPage() {
        this.emitUpdate(this.page - 1);
    }

    nextPage() {
        this.emitUpdate(this.page + 1);
    }

    resetEnteredPage() {
        this.enteredPage = this.page;
    }

    onPageNumberEnter() {
        if (!isNaN(parseFloat(this.enteredPage))) {
            let enteredPageNumber = parseInt(this.enteredPage, 10);
            if (this.buggy && (enteredPageNumber === 1 || enteredPageNumber === this.totalPages)) {
                return;
            }

            this.emitUpdate(this.enteredPage);
        }
    }

    private emitUpdate(page: number) {
        if (page < 1) {
            page = 1;
        }

        if (!this.buggy && page > this.totalPages) {
            page = this.totalPages;
        }

        this.enteredPage = page;
        this.goToPage.emit(page);
    }
}
