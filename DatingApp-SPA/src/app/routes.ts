import { Route, Routes } from '@angular/router'
import { ListsComponent } from './lists/lists.component'
import { MembersComponent } from './members/member-list/member-list.component'
import { MessagesComponent } from './messages/messages.component'
import { HomeComponent } from './home/home.component'
import { AuthGuard } from './_guards/auth.guard'
import { MemberDetailComponent } from './members/member-detail/member-detail.component'
import { MemberDetailsResolver } from './_resolvers/members-details.resolver'
import { MembersListResolver } from './_resolvers/members-list.resolver'
import { MemberEditComponent } from './members/member-edit/member-edit.component'
import { MembersEditResolver } from './_resolvers/members-edit.resolver'
import { PerventClose } from './_guards/preventClose.guard'

export const routes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        canActivate: [AuthGuard],
        runGuardsAndResolvers: "always",
        children: [
            { path: 'members', component: MembersComponent, resolve: { users: MembersListResolver } },
            { path: 'lists', component: ListsComponent, },
            { path: 'messages', component: MessagesComponent },
            { path: 'details/:id', component: MemberDetailComponent, resolve: { user: MemberDetailsResolver } },
            {
                path: 'member/edit',
                component: MemberEditComponent,
                canDeactivate: [PerventClose],
                resolve: { userToEdit: MembersEditResolver }
            },
        ]
    },
    { path: '**', redirectTo: 'home', pathMatch: 'full' },
] 