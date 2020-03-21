import { Component, OnInit } from "@angular/core";
import { AuthService } from '../service/auth.service';
import { error } from '@angular/compiler/src/util';
import { from } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.css"]
})
export class NavComponent implements OnInit {
  constructor(public authService: AuthService) { }
  model: any = {};
  val: any;
  ngOnInit() {
    // this.getData();

  }

  login() {
    this.authService.login(this.model).subscribe(
      response => {
        console.log("logged in successfully");
      }, error => {
        console.log("faild to login ");
      }
    );
  }

  // getData() {
  //   const data = from(fetch('http://localhost:6100/api/values'));
  //   // Subscribe to begin listening for async result
  //   data.subscribe({
  //     next(response) { console.log(response); },
  //     error(err) { console.error('Error: ' + err); },
  //     complete() { console.log('Completed'); }
  //   });
  // }

  IsLoggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem("token");
  }
}
