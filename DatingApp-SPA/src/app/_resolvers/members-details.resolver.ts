import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/service/user.service';
import { Alertifyjs } from 'src/app/service/alertify.service';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class MemberDetailsResolver implements Resolve<User> {

    constructor(private userService: UserService, private alertify: Alertifyjs) {
    }

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(route.params['id']).pipe(
            catchError(function(error) {
                this.alertify.error(error);
                //return of(null);
                // return Observable.throw(error.statusText);
                return of(null);
            })
        );
    }
    
}