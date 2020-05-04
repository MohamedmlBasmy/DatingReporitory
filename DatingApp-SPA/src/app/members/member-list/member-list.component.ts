import { Component, OnInit } from '@angular/core';
import { UserService } from '../../service/user.service';
import { User } from '../../models/user';
import { Alertifyjs } from '../../service/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginationResult, Pagination } from 'src/app/models/pagination';
import { UserParams } from 'src/app/models/user-params';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-members',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MembersComponent implements OnInit {

  users: User[];
  paginationInfo: Pagination;
  userParams: UserParams = new UserParams();
  gender: any;
  defaultValue: string;

  constructor(private userService: UserService, private authService: AuthService, private route: ActivatedRoute, private router: Router, private alertify: Alertifyjs) { }

  ngOnInit(): void {

    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.paginationInfo = data['users'].pagination;
    });
    this.gender = [{ name: 'male', value: 'males' }, { name: 'female', value: 'females' }];
    //console.log(this.gender);
    this.getCurrentUser();
    //this.userParams.gender = 
  }

  getCurrentUser() {
    this.userService.getUser(this.authService.username.nameid).subscribe((response) => {
      this.userParams.gender = response.gender == "male" ? "female" : "male";;
    }, (error) => {
      console.log(error);
    })
  }

  getUsers() {
    this.userService.getAllUsers(this.paginationInfo.pageNumber, this.userParams.pageSize, this.userParams).subscribe(response => {
      this.users = response.result;
      this.paginationInfo = response.pagination;
    }, error => {
      this.alertify.error(error);
    })
  }

  pageChanged(event: any): void {
    this.paginationInfo.pageNumber = event.page;
    this.getUsers();
  }

}
