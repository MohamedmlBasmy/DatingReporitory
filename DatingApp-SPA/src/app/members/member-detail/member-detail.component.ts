import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/service/user.service';
import { ActivatedRoute } from '@angular/router';
import { Alertifyjs } from 'src/app/service/alertify.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  userDetails: User;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // this.getUserDetails();
    this.route.data.subscribe( (data) => {
      this.userDetails = data['user'];
      //console.log(this.userDetails);
    });
  }

  // getUserDetails() {
  //   this.userService.getUser(this.route.snapshot.params['id']).subscribe((response) => {
  //     this.userDetails = response;
  //   }, (error) => {
  //     this.alertify.error(error);
  //   })
  // }


}
