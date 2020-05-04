import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of, pipe } from 'rxjs';
import { Message } from '../models/message';
import { UserService } from '../service/user.service';
import { AuthService } from '../service/auth.service';
import { PaginationResult } from '../models/pagination';
import { catchError } from 'rxjs/operators';
import { Alertifyjs } from '../service/alertify.service';

@Injectable({ providedIn: 'root' })



export class MessagesResolver implements Resolve<PaginationResult<Message[]>> {

    pageSize: number = 6;
    pageNumber: number = 1;
    MessageType: string = "Unread";

    constructor(private userService: UserService, private authService: AuthService, private alertify: Alertifyjs, private router: Router) {
    }

    resolve(route: ActivatedRouteSnapshot): Observable<PaginationResult<Message[]>> {
        return this.userService.getMessages(this.authService.username.nameid, this.pageNumber, this.pageSize, this.MessageType).pipe(
            catchError(error => {
                this.alertify.error("Error retriving messages");
                this.router.navigate(['/home']);
                return of(null);
            }
            )
        );

    }
}