import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
    serviceUrl: string;

    constructor() {
        this.serviceUrl = process.env.ENV === 'build'
            ? 'https://k51qryqov3.execute-api.ap-southeast-2.amazonaws.com/prod'
            : 'http://localhost:3000';
    }
}
