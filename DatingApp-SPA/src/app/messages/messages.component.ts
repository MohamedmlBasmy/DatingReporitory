import { Component, OnInit } from '@angular/core';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { Message } from '../models/message';
import { Pagination, PaginationResult } from '../models/pagination';
import { UserService } from '../service/user.service';
import { Alertifyjs } from '../service/alertify.service';
import { AuthService } from '../service/auth.service';


@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[];
  pagination: Pagination;
  messageContainer: string;
  
  constructor(private routerLink: ActivatedRoute,
    private userServcie: UserService, private alterify: Alertifyjs
    , private authSerive: AuthService, private alert: Alertifyjs) { }

  ngOnInit(): void {
    this.messageContainer = "Unread";
    this.routerLink.data.subscribe(response => {
      this.messages = response['messages'].result;
      this.pagination = response['messages'].pagination;
    });
  }

  loadMessages(messageContainer: string) {
    this.messageContainer = messageContainer
    console.log(messageContainer);
    this.userServcie.getMessages(this.authSerive.username.nameid, this.pagination.pageNumber
      , this.pagination.pageSize, messageContainer).subscribe((response : any) => {
        this.messages = response.result;
        this.pagination = response.pagination;
      })
  }

  PageChanged(event: any): void{
    this.pagination.pageNumber = event.page;
    this.loadMessages(this.messageContainer);
  }

  deleteMessage(messageId: number) {
    this.alterify.confirm("Are you Sure you want to delete this message", () => {
      this.userServcie.deleteMessage(messageId, this.authSerive.username.nameid).subscribe(() => {
        this.messages.splice(this.messages.findIndex(x => x.id == messageId), 1);
      }, (error) => {
        this.alterify.error(error);
      })
    })
  }

}
