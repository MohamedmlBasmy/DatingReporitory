import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, Routes, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  constructor(private router: ActivatedRoute) { }

  ngOnInit(): void {
     
  }

}
