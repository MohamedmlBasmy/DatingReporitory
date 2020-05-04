import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './home/register/register.component';
import { BsDropdownModule } from 'ngx-bootstrap';

import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MembersComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { RouterModule } from '@angular/router';
import { routes } from './routes';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { TabsModule } from 'ngx-bootstrap';
import { MembersListResolver } from './_resolvers/members-list.resolver';
import { MemberDetailsResolver } from './_resolvers/members-details.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MembersEditResolver } from './_resolvers/members-edit.resolver';
import { AuthService } from './service/auth.service';
import { UserService } from './service/user.service';
import { PerventClose } from './_guards/preventClose.guard';
import { PaginationModule } from 'ngx-bootstrap';
import { listsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { TimeagoModule } from 'ngx-timeago';
import { MembersMessagesComponent } from './members/members-messages/members-messages.component';

export function getToken() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MembersComponent,
      MessagesComponent,
      ListsComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      MembersMessagesComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      BrowserAnimationsModule,
      RouterModule.forRoot(routes),
      TabsModule.forRoot(),
      PaginationModule.forRoot(),
      BrowserAnimationsModule,
      BsDatepickerModule.forRoot(),
      TimeagoModule.forRoot(),
      JwtModule.forRoot({
         config : {
            tokenGetter : getToken
         }
      }),
      ReactiveFormsModule
   ],
   providers: [
      MembersListResolver,
      MemberDetailsResolver,
      MembersEditResolver,
      AuthService,
      UserService,
      PerventClose,
      listsResolver,
      MessagesResolver
   ],
   bootstrap: [
      AppComponent
   ]
   
})
export class AppModule { }
