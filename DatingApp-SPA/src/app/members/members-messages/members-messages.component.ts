import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/app/models/message';
import { UserService } from 'src/app/service/user.service';
import { AuthService } from 'src/app/service/auth.service';
import { Alertifyjs } from 'src/app/service/alertify.service';
import { tap, map } from 'rxjs/operators';

@Component({
  selector: 'app-members-messages',
  templateUrl: './members-messages.component.html',
  styleUrls: ['./members-messages.component.css']
})
export class MembersMessagesComponent implements OnInit {


  messages: Message[];

  newMessage: any = {};

  @Input()
  recipientId: number;

  constructor(private userService: UserService, private authService: AuthService, private alertify: Alertifyjs) { }

  ngOnInit(): void {
    this.getThread();
  }
  getThread() {
    var currentId = +this.authService.username.nameid;
    this.userService.getThread(this.authService.username.nameid, this.recipientId)
      .pipe(
        tap(messages => {
          for (let i = 0; i < messages.length; i++) {
            //const element = this.messages[i];
            if (messages[i].isRead === false && messages[i].recipientId === currentId) {
              this.userService.readMessage(messages[i].id, currentId);
            }
          }
        })
        ).subscribe((response) => {
          this.messages = response;
          // for (let i = 0; i < this.messages.length; i++) {
          //   const element = this.messages[i];
          //   this.alertify.message(this.messages[i].senderKnowsAs);
          // }
        }, (error) => {
          this.alertify.error(error.message);
        })
  }

  sendMessage() {
    this.newMessage.RecipientId = this.recipientId;
    this.userService.createMessage(this.authService.username.nameid, this.newMessage).subscribe((response) => {
      console.log(response);
    }, (error => {
      this.alertify.error(error.message); 
    }), () => {
      this.getThread();
      this.newMessage.content = "";
    }
    )
  }
}
