import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { Alertifyjs } from '../service/alertify.service';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private alertify: Alertifyjs) {
  }

  canActivate () : boolean {
      if (this.authService.loggedIn()) {
        return true;        
      }
      else {
        this.alertify.error("please login first");
        this.router.navigate(["/home"]);
        return false;
      }
  }
  
}
