import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/service/user.service';
import { AuthService } from 'src/app/service/auth.service';

@Injectable({ providedIn: 'root' })

export class MembersEditResolver implements Resolve<User> {

    constructor(private userService: UserService, private route: ActivatedRoute, private authService: AuthService) {
        
    }

    resolve(route: ActivatedRouteSnapshot): Observable<User>  {
        return this.userService.getUser(this.authService.username.nameid);
    }

    
}