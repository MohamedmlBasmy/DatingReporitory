import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url = 'http://localhost:6100/api/auth';
  jwtHelper = new JwtHelperService();
  //decodedToken = this.jwtHelper.decodeToken('token');

  username: any;
  constructor(private httpclient: HttpClient) { }

  login(model: any) {
    return this.httpclient.post(this.url + '/login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.username = this.jwtHelper.decodeToken(user.token);
          console.log(this.username.unique_name);
        }
      })
    );
  }

  register(model: any) {
    return this.httpclient.post(this.url + "/register", model);
  }

  loggedIn() {
    var token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
