import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../../service/auth.service';
import { error } from '@angular/compiler/src/util';
import { Alertifyjs } from 'src/app/service/alertify.service'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {

  model: any = {};

  @Output()
  cancelRegister = new EventEmitter();

  constructor(private AuthService: AuthService, private notifyService: Alertifyjs) { }

  ngOnInit() {

  }

  register() {
    this.AuthService.register(this.model).subscribe(response => {
      this.notifyService.success()
    }, error => {
      this.notifyService.error("something happend")
    })
  }

  Cancel() {
    this.cancelRegister.emit(false);
  }

}