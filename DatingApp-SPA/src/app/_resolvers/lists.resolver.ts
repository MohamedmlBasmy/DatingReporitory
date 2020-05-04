import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/service/user.service';
import { PaginationResult } from '../models/pagination';
import { UserParams } from '../models/user-params';
import { AuthService } from '../service/auth.service';

@Injectable({ providedIn: 'root' })

export class listsResolver implements Resolve<PaginationResult<User[]>> {

    constructor(private userService: UserService, private authService: AuthService) {  
    }

    userparams: UserParams = new UserParams();
    likesParams= "likees";
    
    resolve(route: ActivatedRouteSnapshot): Observable<PaginationResult<User[]>>  {
        return this.userService.getAllUsers(this.userparams.pageNumber, this.userparams.pageSize, null, this.likesParams);
        //return null;
    }
}