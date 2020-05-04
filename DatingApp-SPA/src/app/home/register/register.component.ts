import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../../service/auth.service';
import { error } from '@angular/compiler/src/util';
import { Alertifyjs } from 'src/app/service/alertify.service'
import { FormGroup, FormControl, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from 'src/app/models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {

  model: any = {};
  registrationForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;
  colorTheme = 'theme-red';
  user: User;

  @Output()
  cancelRegister = new EventEmitter();

  constructor(private AuthService: AuthService, private notifyService: Alertifyjs, private route: Router) { }

  ngOnInit() {
    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme, isAnimated: true });

    this.registrationForm = new FormGroup({
      gender: new FormControl('male', Validators.required),
      username: new FormControl('', Validators.required),
      knownAs: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      dateOfBirth: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.min(4), Validators.max(10)]),
      confirmPassword: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required)
    }, this.validateConfirmPassword);
  }

  validateConfirmPassword(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatch': true };
  }

  get getUsername(): FormControl {
    return this.registrationForm.get('username') as FormControl;
  }

  get getPassword(): FormControl {
    return this.registrationForm.get('password') as FormControl;
  }

  get getEmail(): FormControl {
    return this.registrationForm.get('email') as FormControl;
  }

  get getConfirmPassword(): FormControl {
    return this.registrationForm.get('confirmPassword') as FormControl;
  }

  register() {
    if (this.registrationForm.valid) {
        var user = Object.assign({}, this.registrationForm.value);
        this.AuthService.register(user).subscribe(() => {
        this.route.navigate['/members'];
      }, () => {
        this.notifyService.error("Make sure that you run your api first");
      });
    }
  }

  Cancel() {
    this.cancelRegister.emit(false);
  }

  static cannotContainSpace(control: AbstractControl): ValidationErrors | null {
    if ((control.value as string).indexOf(' ') >= 0) {
      return { cannotContainSpace: true }
    }
    return null;
  }

}