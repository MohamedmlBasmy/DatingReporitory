
import * as alertifyjs from 'alertifyjs'
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class Alertifyjs {
  /**
   *
   */
  constructor() {
 
  }

  confirm(message: string, ok: () => any) {
    alertifyjs.confirm(message,
      function (e: any) {
        if (e) {
          ok();
        }
        else { }
      });
  }

  error(message: string) {
    alertifyjs.error(message);
  }

  warning(message: string) {
    alertifyjs.warning(message);
  }

  message(message: string) {
    alertifyjs.message(message);
  }

  success() {
    alertifyjs.error("success");
  }
}