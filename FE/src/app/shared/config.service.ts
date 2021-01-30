import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
    serviceUrl: string;

    constructor() {
        this.serviceUrl = process.env.ENV === 'build' ? 'http://buggyapi.azurewebsites.net' : 'http://localhost:3000';
    }
}
