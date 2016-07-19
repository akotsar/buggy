import { Injectable } from '@angular/core';

@Injectable()
export class BrokenService {
    private _isTwitterBroken = false;
    private _isHomeLinkBroken = false;
    private _isLogoutBroken = false;

    isTwitterBroken() {
        return this._isTwitterBroken;
    }

    isHomeLinkBroken() {
        return this._isHomeLinkBroken;
    }

    isLogoutBroken() {
        return this._isLogoutBroken;
    }

    reset() {
        this._isTwitterBroken = false;
        this._isHomeLinkBroken = false;
        this._isLogoutBroken = false;
    }

    breakTwitter() {
        this._isTwitterBroken = true;
    }

    breakHomeLink() {
        this._isHomeLinkBroken = true;
    }

    breakLogout() {
        this._isLogoutBroken = true;
    }
}
