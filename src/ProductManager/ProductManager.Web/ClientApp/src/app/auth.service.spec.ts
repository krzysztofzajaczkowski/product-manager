import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService, BASE_URL } from './auth.service';
import { JwtDto } from './models/jwtDto';
import { LoginRequest } from './models/loginRequest';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;


  afterEach(()=> {
    httpMock.verify();
  });

  beforeEach(() =>{
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        { provide: BASE_URL, useValue: 'https://localhost/' }
      ]
    });
    service = TestBed.get(AuthService);
    httpMock = TestBed.get(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should send a POST request with loginRequest interface and return jwtDto when credentials are correct', (done) => {
    const email = "user@email.com";
    const password = "secret";
    const role = "user";
    let body: LoginRequest = {
      email: email,
      password: password,
      role: role
    };

    service.login(email, password, role).subscribe(
      (data: JwtDto) => {
        expect(data).toBeDefined();
        expect(data.role).toBe(role);
        done();
      },
      (error) => {
        fail(error.message);
      }
    );

    const testRequest = httpMock.expectOne('https://localhost/account/login');
    expect(testRequest.request.method).toBe('POST');
    expect(testRequest.request.body).toEqual(body);
    testRequest.flush({
      token: "123testToken",
      role: role,
      expires: 3600
    });
  })

  it('should send a POST request with loginRequest interface and return 404 Not Found error when user does not exist', (done) => {
    const email = "user@email.com";
    const password = "secret";
    const role = "user";
    let body: LoginRequest = {
      email: email,
      password: password,
      role: role
    };

    service.login(email, password, role).subscribe(
      (data: JwtDto) => {
        fail('Request should return 404 Not Found');
      },
      (error) => {
        expect(error.status).toBe(404);
        done();
      }
    );

    const testRequest = httpMock.expectOne('https://localhost/account/login');
    expect(testRequest.request.method).toBe('POST');
    expect(testRequest.request.body).toEqual(body);
    testRequest.flush(null, { status: 404, statusText: 'Not found'});
  })

  it('should send a POST request with loginRequest interface and return 400 Bad Request error when credentials are invalid', (done) => {
    const email = "user@email.com";
    const password = "secret";
    const role = "user";
    let body: LoginRequest = {
      email: email,
      password: password,
      role: role
    };

    service.login(email, password, role).subscribe(
      (data: JwtDto) => {
        fail('Request should return 400 Bad Request');
      },
      (error) => {
        expect(error.status).toBe(400);
        done();
      }
    );

    const testRequest = httpMock.expectOne('https://localhost/account/login');
    expect(testRequest.request.method).toBe('POST');
    expect(testRequest.request.body).toEqual(body);
    testRequest.flush(null, { status: 400, statusText: 'Bad Request'});
  })
});
