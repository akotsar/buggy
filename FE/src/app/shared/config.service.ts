import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
    serviceUrl: string;

    constructor() {
        this.serviceUrl = process.env.ENV === 'build' ? 'https://vkapg54b8j.execute-api.ap-southeast-2.amazonaws.com/Prod' : 'http://localhost:3000';
    }
}
