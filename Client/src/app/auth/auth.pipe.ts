import { Pipe, PipeTransform } from '@angular/core';
import { Observable, of as observableOf } from 'rxjs';
import { map } from 'rxjs/operators';
import {NbAuthService} from "../sharebook-nebular/auth/services/auth.service";
import {NbAuthToken} from "../sharebook-nebular/auth/services/token/token";

@Pipe({ name: 'ngxAuthToken' })
export class AuthPipe implements PipeTransform {
  constructor(private authService: NbAuthService) {}

  transform(url: string): Observable<string> {
    if (!url) {
      return observableOf(url);
    }
    return this.authService.getToken()
      .pipe(map((token: NbAuthToken) => {
        const separator = url.indexOf('?') > 0 ? '&' : '?';
        return `${url}${separator}token=${token.getValue()}`;
      }));

  }
}
