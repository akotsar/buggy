import { Pipe, PipeTransform  } from '@angular/core';
import * as showdown from 'showdown';

@Pipe({ name: 'myShowdown'})
export class ShowdownPipe implements PipeTransform {
    transform(markup: string) {
        let converter = new showdown.Converter();
        return converter.makeHtml(markup);
    }
}
