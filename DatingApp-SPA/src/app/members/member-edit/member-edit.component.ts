import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/models/user';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/service/user.service';
import { Alertifyjs } from 'src/app/service/alertify.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  user: User;

  @ViewChild("editForm")
  editForm: NgForm

  constructor(private route: ActivatedRoute, private userService: UserService, private alertify: Alertifyjs) { }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.user = data['userToEdit'];
    })
  }

  saveChanges() {
    this.userService.updateUser(this.user.id, this.user).subscribe((response) => {
      this.alertify.message("Data Updated Successfully");
      //this.editForm.dirty = false;
      this.editForm.reset(this.user);
    }, (error) => {
      console.log(error)
    })
  }
}
