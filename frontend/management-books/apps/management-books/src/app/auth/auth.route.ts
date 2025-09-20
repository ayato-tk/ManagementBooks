import { Route } from '@angular/router';
import { LoginComponent } from './containers/login/login.component';
import { SignupComponent } from './containers/signup/signup.component';
import { ResetPasswordComponent } from './containers/reset-password/reset-password.component';

export const authRoutes: Route[] = [
  {
    path: '',
    component: LoginComponent,
  },
  {
    path: 'signup',
    component: SignupComponent,
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
  },
];
