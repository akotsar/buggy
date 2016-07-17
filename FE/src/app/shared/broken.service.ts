import { Injectable } from '@angular/core';

@Injectable()
export class BrokenService {
    private _isTwitterBroken = false;
    private _isHomeLinkBroken = false;

    isTwitterBroken() {
        return this._isTwitterBroken;
    }

    isHomeLinkBroken() {
        return this._isHomeLinkBroken;
    }

    reset() {
        this._isTwitterBroken = false;
        this._isHomeLinkBroken = false;
    }

    breakTwitter() {
        this._isTwitterBroken = true;
    }

    breakHomeLink() {
        this._isHomeLinkBroken = true;
    }
}
