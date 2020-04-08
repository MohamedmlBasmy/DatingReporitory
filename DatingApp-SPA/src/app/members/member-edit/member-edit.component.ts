import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/models/user';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  user: User;
  @ViewChild("editForm")
  editForm: { dirty: any; }
  constructor(private route: ActivatedRoute, private userService: UserService) { }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.user = data['userToEdit'];
      console.log("User Known As: " + this.user.introduction);
    })
  }
  saveChanges() {
    this.userService.updateUser(this.user.id, this.user).subscribe((response) => {
      console.log(response)
    }, (error) => {
      console.log(error)
    })
  }
}
