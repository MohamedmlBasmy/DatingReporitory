import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, Routes, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { User } from '../models/user';
import { PaginationResult, Pagination } from '../models/pagination';
import { UserService } from '../service/user.service';
import { Alertifyjs } from '../service/alertify.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  users: User[];
  pagination: Pagination;
  likesParam: string;

  constructor(private router: ActivatedRoute, private userService: UserService, private alterify: Alertifyjs) { }

  ngOnInit(): void {
    this.router.data.subscribe((response) => {
      this.users = response['users'].result;
      this.pagination = response['users'].pagination;
    });
    this.likesParam = 'Likers';
  }

  loadUsers() {
    this.userService
      .getAllUsers(
        this.pagination.pageNumber,
        this.pagination.pageSize,
        null,
        this.likesParam).subscribe((res: PaginationResult<User[]>) => {
          this.users = res.result;
          this.pagination = res.pagination;
        },
          error => {
            this.alterify.error(error);
          }
        );
  }

  pageChanged(event: any): void {
    this.pagination.pageNumber = event.page;
    this.loadUsers();
  }

}
