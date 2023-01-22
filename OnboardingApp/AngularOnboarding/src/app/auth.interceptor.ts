import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { EMPTY, Observable } from "rxjs";
import { SharedService } from "./shared.service";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(private service:SharedService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      if (req.headers.get("skip")){
        const modifiedReq = req.clone({ 
          headers: req.headers.delete('skip'),
        });
        return next.handle(modifiedReq);
      }
      else{
        if(this.service.isUserAuthenticated()){
          const userToken = localStorage.getItem("token");
          const modifiedReq = req.clone({ 
            headers: req.headers.set('Authorization', `Bearer ${userToken}`),
          });
          return next.handle(modifiedReq);
        }
        else{
            this.service.TimeOut();
            return EMPTY;
        }
      }


      /**
        if(localStorage.getItem("token") != null){
        const userToken = localStorage.getItem("token");
        const modifiedReq = req.clone({ 
          headers: req.headers.set('Authorization', `Bearer ${userToken}`),
        });
        return next.handle(modifiedReq);
      }
      else{
        return next.handle(req);
      }

      **/
    }
  }