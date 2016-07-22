import { Headers, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../config.service';

export class BaseApiService {

    constructor(protected config: ConfigService) {

    }

    protected getAuthHeaders(token?: string): Headers {
        let headers = new Headers();

        if (token) {
            headers.append('Authorization', 'Bearer ' + token);
        }

        return headers;
    }

    protected urlEncode(obj: Object): string {
        let urlSearchParams = new URLSearchParams();
        for (let key in obj) {
            if (!obj.hasOwnProperty(key) || obj[key] === undefined) {
                continue;
            }

            urlSearchParams.append(key, obj[key]);
        }

        return urlSearchParams.toString();
    }

    protected handleError<T>(err: any): Observable<T> {
        if (err.status === 400) {
            let response = err.json();
            let message = '';
            if (response.modelState) {
                for (let field in response.modelState) {
                    if (response.modelState.hasOwnProperty(field)) {
                        for (let fieldMessage of response.modelState[field]) {
                            message = message + (message ? ', ' : '') + fieldMessage;
                        }
                    }
                }
            } else {
                message = response.message;
            }

            return Observable.throw(message);
        }

        return Observable.throw('Unknown error');
    }
}
