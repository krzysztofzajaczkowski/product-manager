import { HttpInterceptor } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {

  constructor(
    private authService: AuthService
  ) { }

  intercept(req, next) {
    let authToken = this.authService.authorizationToken;
    let authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${authToken}`}
    });

    return next.handle(authReq);
  }
}
