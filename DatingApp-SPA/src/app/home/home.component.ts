import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  values: any;
  constructor(private httpClient:HttpClient) { }
  form: FormGroup;

  ngOnInit() {
  }

  registerToggle() {
    this.registerMode = true;
  }
  
  cancelRegisterMode(registerMode: boolean){
    this.registerMode = registerMode;
  }
}
