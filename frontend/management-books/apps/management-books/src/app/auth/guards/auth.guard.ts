import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService, UserService } from '@management-books/data-access';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private userService: UserService,
    private router: Router,
  ) {}

  canActivate(): boolean {
    const token = this.userService.getToken();

    if (token && this.userService.tokenIsValid(token)) {
      return true;
    }

    this.userService.logoff();
    return false;
  }
}
