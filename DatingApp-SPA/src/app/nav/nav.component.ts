import { Component, OnInit } from "@angular/core";
import { AuthService } from '../service/auth.service';
import { error } from '@angular/compiler/src/util';
import { from } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { Alertifyjs } from '../service/alertify.service';

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.css"]
})
export class NavComponent implements OnInit {
  constructor(public authService: AuthService, private router: Router, private alertify: Alertifyjs) { }
  model: any = {};
  val: any;
  ngOnInit() {
    // this.getData();

    //console.log(this.authService.username);
  }

  login() {
    this.authService.login(this.model).subscribe(
      response => {
        this.alertify.message("Logged in Successfully");
      }, (error) => {
        this.alertify.error("you're not authorized to log in");
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
    this.router.navigate(['/']);
  }
}
