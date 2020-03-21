import { Component, OnInit } from '@angular/core';
import { AuthService } from './service/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingApp-SPA';
  username: any;
  /**
   *
   */
  constructor(private authService:AuthService) {
    
  }
  ngOnInit(): void {
    var token = localStorage.getItem('token');
    if (token) {
    this.authService.username = this.authService.jwtHelper.decodeToken(token);
    }
  }


}
