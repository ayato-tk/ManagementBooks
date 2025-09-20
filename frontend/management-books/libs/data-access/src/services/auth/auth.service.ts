import { Inject, Injectable } from '@angular/core';
import { AppStoragePrefixToken } from '../../tokens';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { AuthRequestDTO } from '../../dtos/auth-request.dto';
import { AuthResetPasswordRequestDTO, AuthSignUpRequestDTO } from '../../dtos';

@Injectable()
export class AuthService {
  constructor(
    @Inject(AppStoragePrefixToken) private storagePrefix: string,
    private http: HttpClient,
  ) {}

  public authenticate(formAuth: AuthRequestDTO) {
    return this.http.post(`${environment.api_base_url}/auth/signin`, formAuth, {
      responseType: 'text',
    });
  }

  public signUp(formAuth: AuthSignUpRequestDTO) {
    return this.http.post(`${environment.api_base_url}/auth/signup`, formAuth);
  }

  public requestChangePassword(email: string) {
    return this.http.post(
      `${environment.api_base_url}/auth/request-reset`,
      { email },
      { responseType: 'text' },
    );
  }

  public resetPassword(request: AuthResetPasswordRequestDTO) {
    return this.http.post(`${environment.api_base_url}/auth/reset-password`, request, {
      responseType: 'text',
    });
  }
}
