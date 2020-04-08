import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/service/user.service';

@Injectable({ providedIn: 'root' })

export class MembersListResolver implements Resolve<User[]> {

    constructor(private userService: UserService) {
        
    }
    resolve(route: ActivatedRouteSnapshot): Observable<User[]>  {
        return this.userService.getAllUsers();
    }
}