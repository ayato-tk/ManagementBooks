import { Inject, Injectable } from '@angular/core';
import { AppStoragePrefixToken } from '../../tokens';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class UserService {
  private _jwtHelper = new JwtHelperService();

  constructor(
    @Inject(AppStoragePrefixToken) private storagePrefix: string,
    private route: Router,
  ) {}

  public logoff(): void {
    this.clearStorage();
    this.route.navigateByUrl('auth');
  }

  public clearStorage(): void {
    localStorage.clear();
    sessionStorage.clear();
  }

  public tokenIsValid(token: string): boolean {
    return !this._jwtHelper.isTokenExpired(token);
  }

  public getToken(): string | null {
    return localStorage.getItem(`${this.storagePrefix}.token`);
  }

  public setToken(token: string): void {
    localStorage.setItem(`${this.storagePrefix}.token`, token);
  }
}
