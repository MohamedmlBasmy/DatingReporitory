import { Component, OnInit } from '@angular/core';
import { UserService } from '../../service/user.service';
import { User } from '../../models/user';
import { Alertifyjs } from '../../service/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-members',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MembersComponent implements OnInit {

  users: User[];

  constructor(private userService: UserService, private route: ActivatedRoute, private router: Router, private alertify: Alertifyjs) { }

  ngOnInit(): void {
    //this.getUsers();
    this.route.data.subscribe(data=> {
      this.users = data['users'];
    });
  }

  getUsers() {
    this.userService.getAllUsers().subscribe(response => {
      this.users = response;
    }, error => {
      this.alertify.error(error);
    })
  }

}
