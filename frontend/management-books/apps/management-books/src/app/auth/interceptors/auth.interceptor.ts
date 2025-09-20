import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { UserService } from '@management-books/data-access';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const userService = inject(UserService);
  const token = userService.getToken();

  const isPublic = req.url.includes('/auth');

  if (token && !isPublic) {
    const cloned = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });
    return next(cloned);
  }

  return next(req);
};
