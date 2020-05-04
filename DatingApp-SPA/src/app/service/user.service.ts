import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { Pagination, PaginationResult } from '../models/pagination';
import { map, catchError } from 'rxjs/operators';
import { JsonPipe } from '@angular/common';
import { Alertifyjs } from './alertify.service';
import { Message } from '../models/message';

//  const options = {
//    headers : new HttpHeaders({
//     "Authorization": "Bearer " + localStorage.getItem('token')
//   })
//  };
const PaginationMessageResult: PaginationResult<Message[]> = new PaginationResult<Message[]>();
const paginationResult: PaginationResult<User[]> = new PaginationResult<User[]>();

@Injectable({
  providedIn: 'root'
})

export class UserService {
  constructor(private http: HttpClient, private alterify: Alertifyjs) { }

  getAllUsers(pageNumber?, pageSize?, userParams?, likesParams?): Observable<PaginationResult<User[]>> {

    let httpParams = new HttpParams();
    httpParams = httpParams.append('pageNumber', pageNumber);
    httpParams = httpParams.append('pageSize', pageSize);

    if (userParams != null) {
      httpParams = httpParams.append('minAge', userParams.minAge);
      httpParams = httpParams.append('maxage', userParams.maxAge);
      httpParams = httpParams.append('gender', userParams.gender);
    }

    if (likesParams === "likees") {
      httpParams = httpParams.append('likees', 'true');

    }
    if (likesParams === "likers") {
      httpParams = httpParams.append('likers', 'true');
    }

    return this.http.get<User[]>(environment.APIUrl + "users", { observe: 'response', params: httpParams })
      .pipe(
        map(response => {
          paginationResult.result = response.body['responseBody'];
          if (paginationResult.result == null && response.body['exception'] != null) {
            this.alterify.error(response.body['exception']);
          }
          if (response.headers.get('Pagination') != null) {
            var paginationHeaderValue = response.headers.get("Pagination");
            paginationResult.pagination = JSON.parse(paginationHeaderValue);
          }
          return paginationResult;
        })
      )
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(environment.APIUrl + "users/" + id);
  }

  updateUser(id: number, user: User): Observable<User> {
    return this.http.put<User>(environment.APIUrl + "users/" + id, user);
  }

  getLikes(id: number, recepientId: number) {
    return this.http.post(environment.APIUrl + 'users/' + id + '/like/' + recepientId, {});
  }

  getMessages(userId: number, pageNumber?, pageSize?, messageContainer?): Observable<PaginationResult<Message[]>> {
    var httpParams = new HttpParams();

    //httpParams = httpParams.append("MessageType", "Unread");

    if (pageNumber != null && pageSize != null) {
      httpParams = httpParams.append("PageNumber", pageNumber);
      httpParams = httpParams.append("PageSize", pageSize);
    }

    if (messageContainer != null) {
      httpParams = httpParams.append("MessageType", messageContainer);
    }

    return this.http.get<Message[]>(environment.APIUrl + "users/" + userId + "/Messages",
      { observe: 'response', params: httpParams }).pipe(
        map(response => {
          PaginationMessageResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            //console.log(response.headers.get('response'));
            PaginationMessageResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }

          return PaginationMessageResult;

        })
      );
  }

  getThread(id, recepientId) {
    return this.http.get<Message[]>(environment.APIUrl + "users/" + id + "/Messages/thread/" + recepientId);
  }

  createMessage(id: number, message: Message) {
    return this.http.post<Message>(environment.APIUrl + "users/" + id + "/messages", message);
  }

  deleteMessage(id: number, userId: number) {
    return this.http.post(environment.APIUrl + "users/" + userId + "/messages/" + id, {});
  }

  readMessage(messageId: number, userId) {
    var url = environment.APIUrl + "users/" + userId + "/messages/" + messageId + "/read"
    return this.http.post(url, {}).subscribe();
  }
}
