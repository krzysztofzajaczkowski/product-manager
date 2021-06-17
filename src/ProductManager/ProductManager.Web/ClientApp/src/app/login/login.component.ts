import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { JwtDto } from '../models/jwtDto';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  error;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
    ) { }

  ngOnInit() {
    if(localStorage.getItem("access_token") != null) {
      this.router.navigate(['/']);
    }
    this.error = "";
    this.loginForm = this.formBuilder.group({
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      password: ['', [
        Validators.required
      ]],
      role: ['',[
        Validators.required
      ]]
    });
  }

  get formValid() {
    return this.loginForm.valid && this.loginForm.touched;
  }

  submitLogin() {
    let formValue = this.loginForm.value;
    this.authService.login(formValue.email, formValue.password, formValue.role).subscribe(
      (data: JwtDto) => {
        this.authService.setLoginItems(data.token, data.role);
        this.router.navigate(['/']);
      },
      (error) => {
        this.error = error.error.message;
      }
    )
  }

}
