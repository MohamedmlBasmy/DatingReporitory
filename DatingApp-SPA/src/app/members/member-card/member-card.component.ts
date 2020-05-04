import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/models/user';
import { AuthService } from 'src/app/service/auth.service';
import { Alertifyjs } from 'src/app/service/alertify.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input()
  user: User;

  constructor(private authService: AuthService, private alertify: Alertifyjs, private userService: UserService) { }

  ngOnInit(): void {
  }

  like(recepientId: number) {
    this.userService.getLikes(this.authService.username.nameid, recepientId).subscribe((response)=>{
      console.log(response);
      this.alertify.message("You Liked " + this.user.knownAs);
    }, (error)=>{
      console.log(error);
    });
  }

}
