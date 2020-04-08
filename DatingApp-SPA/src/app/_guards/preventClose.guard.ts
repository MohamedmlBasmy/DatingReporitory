import { CanDeactivate } from '@angular/router';
import { Injectable } from '@angular/core';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable()

export class PerventClose implements CanDeactivate<MemberEditComponent>{
    canDeactivate(component: MemberEditComponent) {
        if (component.editForm.dirty) {
            return confirm("Your changes will not gonna be saved");
        }
        return true;
    }

}