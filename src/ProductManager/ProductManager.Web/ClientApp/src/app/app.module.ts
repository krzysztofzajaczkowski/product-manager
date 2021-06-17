import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';


import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatCheckboxModule, MatChipsModule, MatFormFieldModule, MatInputModule, MatSelectModule } from '@angular/material';
import { AuthService } from './auth.service';
import { BrowseComponent } from './browse/browse.component';
import { AuthGuardService } from './auth-guard.service';
import { ProductService } from './product.service';
import { TokenInterceptorService } from './token-interceptor.service';
import { ProductEditComponent } from './product-edit/product-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    LogoutComponent,
    BrowseComponent,
    ProductEditComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'login', component: LoginComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'browse', component: BrowseComponent, canActivate: [AuthGuardService] },
      { path: 'product/edit/:sku', component: ProductEditComponent, canActivate: [AuthGuardService] }
    ]),
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCheckboxModule,
    MatChipsModule
  ],
  providers: [
    AuthService,
    ProductService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
