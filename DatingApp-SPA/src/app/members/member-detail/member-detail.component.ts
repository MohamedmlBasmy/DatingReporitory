import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/app/service/user.service';
import { ActivatedRoute } from '@angular/router';
import { Alertifyjs } from 'src/app/service/alertify.service';
import { User } from 'src/app/models/user';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  userDetails: User;
  recipientId: number;
  @ViewChild('memberTabs', { static: true }) staticTabs: TabsetComponent;
  
  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // this.getUserDetails();
    this.route.data.subscribe( (data) => {
      this.userDetails = data['user'];
      //console.log(this.userDetails);
    });

    this.route.queryParams.subscribe((response)=>{
      const selectedTab = response['tab'];
      this.staticTabs.tabs[selectedTab].active = true;
    })
  }

  // getUserDetails() {
  //   this.userService.getUser(this.route.snapshot.params['id']).subscribe((response) => {
  //     this.userDetails = response;
  //   }, (error) => {
  //     this.alertify.error(error);
  //   })
  // }


}
