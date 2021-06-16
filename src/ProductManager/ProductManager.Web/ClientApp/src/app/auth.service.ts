import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { LoginRequest } from './models/loginRequest';
import { JwtDto } from './models/jwtDto';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private httpClient: HttpClient,
    @Inject(BASE_URL) private baseUrl: string
    ) { }

    login(email: string, password: string, role: string): Observable<any> {
      let loginRequest: LoginRequest = {
        email: email,
        password: password,
        role: role
      };
      return this.httpClient.post<JwtDto>(this.baseUrl + 'account/login', loginRequest)
    }
}
