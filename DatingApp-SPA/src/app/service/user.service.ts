import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../models/user';

//  const options = {
//    headers : new HttpHeaders({
//     "Authorization": "Bearer " + localStorage.getItem('token')
//   })
//  };

@Injectable({
  providedIn: 'root'
})

export class UserService {
  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(environment.APIUrl + "users");
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(environment.APIUrl + "users/" + id);
  }

  updateUser(id: number, user: User): Observable<User> {
    return this.http.put<User>(environment.APIUrl + "users/" + id , user);
  }
}
